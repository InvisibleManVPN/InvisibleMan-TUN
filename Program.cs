using System;
using System.Linq;
using System.Reflection;

namespace InvisibleManTUN
{
    using Foundation;
    using Managers;
    using Values;

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
                Parser parser = new Parser(validFlags: new[] { Global.PORT });
                parser.Parse(args);

                if (!IsPortFlagExists())
                    return -1;

                try
                {
                    return Convert.ToInt32(parser.GetFlag(Global.PORT).Value);
                }
                catch
                {
                    return -1;
                }

                bool IsPortFlagExists() => parser.GetFlag(Global.PORT) != null;
            }
        }
    }
}