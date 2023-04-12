using System;
using System.Reflection;

namespace InvisibleManTUN
{
    public class Program
    {
        private static void Main(string[] args)
        {
            PrintHeadLines();

            void PrintHeadLines()
            {
                Console.WriteLine("Invisible Man TUN service");
                Console.WriteLine($"version {GetCurrentReleaseVersion()}\n");
            }

            string GetCurrentReleaseVersion() 
            {
                Version version = Assembly.GetExecutingAssembly().GetName().Version;
                return $"{version.Major}.{version.Minor}.{version.Build}";
            }
        }
    }
}