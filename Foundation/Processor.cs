using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;

namespace InvisibleManTUN.Foundation
{
    public class Processor
    {
        private Dictionary<string, Process> processes;

        public Processor()
        {
            this.processes = new Dictionary<string, Process>();
        }

        public void StartProcess(string processName, string fileName, string command)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo(fileName);
            processInfo.Arguments = command;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            try
            {
                Process process = Process.Start(processInfo);

                AddProcess(process, processName);
                Console.WriteLine($"'{processName}' process was started successfully...");

                process.OutputDataReceived += (sender, e) => { };
                process.ErrorDataReceived += (sender, e) => { };
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();

                Console.WriteLine($"'{processName}' process has been exited with the code: '{process.ExitCode}'.");
                RemoveProcess(processName);
            }
            catch
            {
                Console.WriteLine($"Could not start the '{processName}' process.");
                
                StopProcess(processName);
                Environment.Exit(1);
            }
        }

        public void StartProcessAsThread(string processName, string fileName, string command)
        {
            new Thread(() => {
                StartProcess(processName, fileName, command);
            }).Start();
        }

        public void StopProcess(string processName)
        {
            try
            {
                Process process = processes[processName];
                RemoveProcess(processName);
                process.Kill();
            }
            catch
            {

            }
        }

        private void AddProcess(Process process, string processName)
        {
            processes.Add(processName, process);
        }

        private void RemoveProcess(string processName)
        {
            if (!IsProcessExists(processName))
                return;
            
            processes.Remove(processName);
        }

        private bool IsProcessExists(string processName) => processes.ContainsKey(processName);
    }
}