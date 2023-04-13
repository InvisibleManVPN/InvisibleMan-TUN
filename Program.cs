using System;
using System.Linq;
using System.Reflection;

namespace InvisibleManTUN
{
    using Managers;

    public class Program
    {
        private static void Main(string[] args)
        {
            PrintHeadLines();
            InitializeServiceManager();

            void PrintHeadLines()
            {
                Console.WriteLine("Invisible Man TUN service");
                Console.WriteLine($"version {GetCurrentReleaseVersion()}\n");
                Console.WriteLine("usage: InvisibleMan-TUN -port={port}\n");
            }

            void InitializeServiceManager()
            {
                ServiceManager serviceManager = new ServiceManager(GetPort);
                serviceManager.Initialize();
            }

            string GetCurrentReleaseVersion() 
            {
                Version version = Assembly.GetExecutingAssembly().GetName().Version;
                return $"{version.Major}.{version.Minor}.{version.Build}";
            }

            int GetPort()
            {
                if (!IsPortFlagExists())
                    return -1;

                try
                {
                    return Convert.ToInt32(GetPortFlag().Split("=")[1]);
                }
                catch
                {
                    return -1;
                }

                bool IsPortFlagExists() => GetPortFlag() != null;

                string GetPortFlag() => args.FirstOrDefault(argument => argument.StartsWith("-port="));
            }
        }
    }
}