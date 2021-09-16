﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Peernet.Browser.Infrastructure.Tools
{
    public class CmdRunner : IRunable, IDisposable
    {
        private bool _wasRun;
        private Process _p;
        private const string _processName = "Cmd";

        public bool FileExist { get; }

        public CmdRunner(string path = "")
        {
            if (File.Exists(path))
            {
                _p = new Process();
                _p.StartInfo.FileName = path;
                FileExist = true;
            };
        }

        public string Path { get; }

        public bool IsRunning { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Run()
        {
            if (IsRunningCheck()) IsRunning = true;
            else RunProcess();
        }

        private bool IsRunningCheck() => Process.GetProcesses().Any(x => x.ProcessName.Contains(_processName));

        private void RunProcess()
        {
            try
            {
                _p.Start();
                _p.Refresh();
                IsRunning = true;
                _wasRun = true;
            }
            catch (Win32Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_p != null)
                {
                    if (_wasRun) _p.Kill();
                    _p.Dispose();
                    _p = null;
                    IsRunning = false;
                    _wasRun = false;
                }
            }
        }
    }
}