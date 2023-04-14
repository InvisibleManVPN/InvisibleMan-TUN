using System;
using System.Threading;

namespace InvisibleManTUN.Managers
{
    using Handlers;
    using Values;

    public class ServiceManager
    {
        private HandlersManager handlersManager;
        private Func<int> getPort;

        private static Mutex mutex;
        private const string APP_GUID = "{8I7n9VIs-s9i2-84bl-A1em-12A5vN6eDH8M}";

        public ServiceManager(Func<int> getPort)
        {
            this.getPort = getPort;
        }

        public void Initialize()
        {
            AvoidRunningMultipleInstances();
            RegisterHandlers();
            SetupHandlers();
            StartService();
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

        private void RegisterHandlers()
        {
            handlersManager = new HandlersManager();

            handlersManager.AddHandler(new SocketHandler());
            handlersManager.AddHandler(new TunnelHandler());
        }

        private void SetupHandlers()
        {
            TunnelHandler tunnelHandler = handlersManager.GetHandler<TunnelHandler>();

            handlersManager.GetHandler<SocketHandler>().Setup(
                getPort: getPort,
                onStartTunneling: tunnelHandler.Start,
                onStopTunneling: tunnelHandler.Stop
            );
        }

        private void StartService()
        {
            handlersManager.GetHandler<SocketHandler>().Start();
        }
    }
}