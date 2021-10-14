using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Peernet.Browser.Infrastructure.Tools
{
    public class CmdRunner : IRunable, IDisposable
    {
        private const string processName = "Cmd.exe";
        private readonly bool fileExist;
        private Process process;
        private bool wasRun;

        public CmdRunner(string path = "")
        {
            if (Directory.Exists(path))
            {
                process = new Process();
                process.StartInfo = new ProcessStartInfo($"{path}\\{processName}")
                {
                    UseShellExecute = false,
                    WorkingDirectory = path
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
    }
}