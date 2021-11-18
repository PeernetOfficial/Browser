using Peernet.Browser.Application.Managers;
using Peernet.Browser.Infrastructure.Services;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Peernet.Browser.Infrastructure.Tools
{
    public class CmdRunner : IRunable, IDisposable
    {
        private readonly string processName;
        private readonly bool fileExist;
        private Process process;
        private bool wasRun;
        private readonly ISettingsManager settingsManager;

        public CmdRunner(ISettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
            var backend = settingsManager.Backend;
            string fullPath = Path.GetFullPath(backend);
            processName = Path.GetFileName(fullPath);
            process = new Process();
            var apiUrl = $"127.0.0.1:{GetFreeTcpPort()}";
            settingsManager.ApiUrl = $"http://{apiUrl}";
            var currentProcess = Process.GetCurrentProcess();
            process.StartInfo = new ProcessStartInfo($"{fullPath}")
            {
                UseShellExecute = false,
                WorkingDirectory = Path.GetDirectoryName(fullPath),
                Arguments = $"-webapi={apiUrl} -apikey={settingsManager.ApiKey} -watchpid={currentProcess.Id}"
            };

            fileExist = true;
        }

        public bool IsRunning { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Run()
        {
            if (IsRunningCheck())
            {
                IsRunning = true;
            }
            if (fileExist)
            {
                RunProcess();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && process != null)
            {
                if (wasRun)
                {
                    try
                    {
                        new ShutdownService(settingsManager).Shutdown();
                    }
                    catch (Exception)
                    {
                        // handle
                    }

                    for (var i = 0; i < 25 && !process.HasExited; i++)
                    {
                        Thread.Sleep(200);
                    }

                    if (!process.HasExited)
                    {
                        process.Kill();
                    }
                }

                process.Dispose();
                process = null;
                IsRunning = false;
                wasRun = false;
            }
        }

        private bool IsRunningCheck() => Process.GetProcesses().Any(x => x.ProcessName.Contains(processName));

        private void RunProcess()
        {
            try
            {
                process.Start();
                process.Refresh();
                IsRunning = true;
                wasRun = true;
            }
            catch (Win32Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static int GetFreeTcpPort()
        {
            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }
    }
}