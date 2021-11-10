using Peernet.Browser.Application.Managers;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Peernet.Browser.Infrastructure.Tools
{
    public class CmdRunner : IRunable, IDisposable
    {
        private const string processName = "Cmd.exe";
        private readonly bool fileExist;
        private Process process;
        private bool wasRun;

        public CmdRunner(ISettingsManager settingsManager)
        {
            var path = settingsManager.CmdPath;
            if (Directory.Exists(path))
            {
                process = new Process();
                var apiUrl = $"127.0.0.1:{GetFreeTcpPort()}";
                settingsManager.ApiUrl = $"http://{apiUrl}";
                process.StartInfo = new ProcessStartInfo($"{path}\\{processName}")
                {
                    UseShellExecute = false,
                    WorkingDirectory = path,
                    Arguments = $"-webapi={apiUrl}"
                };

                fileExist = true;
            };
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
                    process.Kill();
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