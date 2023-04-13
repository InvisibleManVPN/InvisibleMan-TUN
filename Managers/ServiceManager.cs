using System;
using System.Threading;

namespace InvisibleManTUN.Managers
{
    using Values;

    public class ServiceManager
    {
        private static Mutex mutex;
        private const string APP_GUID = "{8I7n9VIs-s9i2-84bl-A1em-12A5vN6eDH8M}";

        public void Initialize()
        {
            AvoidRunningMultipleInstances();
        }

        private void AvoidRunningMultipleInstances()
        {
            mutex = new Mutex(true, APP_GUID, out bool isCreatedNew);
            if(!isCreatedNew)
            {
                Console.WriteLine(Message.SERVICE_ALREADY_RUNNING);
                Environment.Exit(1);
            }
        }
    }
}