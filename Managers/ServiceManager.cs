using System;
using System.Threading;

namespace InvisibleManTUN.Managers
{
    using Core;
    using Handlers;
    using Values;

    public class ServiceManager
    {
        private InvisibleManTunCore core;
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

            RegisterCore();
            RegisterHandlers();

            SetupHandlers();
            SetupCore();

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

        private void RegisterCore()
        {
            core = new InvisibleManTunCore();
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
            SocketHandler socketHandler = handlersManager.GetHandler<SocketHandler>();

            SetupSocketHandler();
            SetupTunnelHandler();

            void SetupSocketHandler()
            {
                socketHandler.Setup(
                    getPort: getPort,
                    onStartTunneling: tunnelHandler.Start,
                    onStopTunneling: tunnelHandler.Stop
                );
            }

            void SetupTunnelHandler()
            {
                tunnelHandler.Setup(
                    onStopTunnel: core.StopTunnel,
                    onStartTunnel: core.StartTunnel,
                    onSetInterfaceAddress: core.SetInterfaceAddress,
                    onSetInterfaceDns: core.SetInterfaceDns,
                    onSetRoutes: core.SetRoutes
                );
            }
        }

        private void SetupCore()
        {
            SocketHandler socketHandler = handlersManager.GetHandler<SocketHandler>();

            core.Setup(
                onStartSocket: socketHandler.Start
            );
        }

        private void StartService()
        {
            core.Start();
        }
    }
}