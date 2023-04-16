using System;
using System.Threading.Tasks;

namespace InvisibleManTUN.Handlers
{
    using Foundation;
    using Utilities;

    public class TunnelHandler : Handler
    {
        private Scheduler scheduler;

        private Action onStopTunnel;
        private Action<string, string> onStartTunnel;
        private Action<string, string> onSetInterfaceAddress;
        private Action<string, string> onSetInterfaceDns;
        private Action<string, string, string, int> onSetRoutes;

        public TunnelHandler()
        {
            this.scheduler = new Scheduler();
        }

        public void Setup(
            Action onStopTunnel,
            Action<string, string> onStartTunnel,
            Action<string, string> onSetInterfaceAddress,
            Action<string, string> onSetInterfaceDns,
            Action<string, string, string, int> onSetRoutes
        )
        {
            this.onStopTunnel = onStopTunnel;
            this.onStartTunnel = onStartTunnel;
            this.onSetInterfaceAddress = onSetInterfaceAddress;
            this.onSetInterfaceDns = onSetInterfaceDns;
            this.onSetRoutes = onSetRoutes;
        }

        public void Start(string device, string proxy, string address, string server, string dns)
        {
            try
            {
                StartTunnel();
                WaitUntilInterfaceCreated();
                SetInterfaceAddress();
                WaitUntilInterfaceAddressSet();
                SetInterfaceDns();
                SetRoutes();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            void StartTunnel()
            {
                new Task(() => {
                    onStartTunnel.Invoke(device, proxy);
                }).Start();
            }

            void WaitUntilInterfaceCreated()
            {
                scheduler.WaitUntil(
                    condition: IsInterfaceExists,
                    millisecondsTimeout: 6000,
                    $"Device with the name '{device}' was not found."
                );
            }

            void SetInterfaceAddress()
            {
                onSetInterfaceAddress.Invoke(device, address);
            }

            void WaitUntilInterfaceAddressSet()
            {
                scheduler.WaitUntil(
                    condition: IsInterfaceAddressWasSet,
                    millisecondsTimeout: 6000,
                    $"'{address}' was not set to '{device}' device."
                );
            }

            void SetInterfaceDns()
            {
                onSetInterfaceDns.Invoke(device, dns);
            }

            void SetRoutes()
            {
                onSetRoutes.Invoke(
                    server, 
                    address, 
                    NetworkUtility.GetDefaultGateway(address), 
                    NetworkUtility.GetNetworkInterfaceIndex(device)
                );
            }

            bool IsInterfaceExists() => NetworkUtility.IsInterfaceExists(device);

            bool IsInterfaceAddressWasSet() => NetworkUtility.IsInterfaceAddressWasSet(device, address);
        }

        public void Stop()
        {
            onStopTunnel.Invoke();
        }
    }
}