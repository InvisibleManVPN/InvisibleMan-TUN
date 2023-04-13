using System;
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
            }

            void InitializeServiceManager()
            {
                ServiceManager serviceManager = new ServiceManager();
                serviceManager.Initialize();
            }

            string GetCurrentReleaseVersion() 
            {
                Version version = Assembly.GetExecutingAssembly().GetName().Version;
                return $"{version.Major}.{version.Minor}.{version.Build}";
            }
        }
    }
}