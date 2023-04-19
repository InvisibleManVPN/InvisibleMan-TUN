using System;

namespace InvisibleManTUN.Core
{
    public class InvisibleManTunCore
    {
        private Action onStartSocket;

        public void Setup(Action onStartSocket)
        {
            this.onStartSocket = onStartSocket;
        }

        public void Start()
        {
            onStartSocket.Invoke();
        }

        public void StartTunnel(string device, string proxy)
        {
            TunWrapper.StartTunnel(device, proxy);
        }

        public void StopTunnel()
        {
            TunWrapper.StopTunnel();
        }

        public bool IsTunnelRunning()
        {
            return TunWrapper.IsTunnelRunning();
        }

        public void SetInterfaceAddress(string device, string address)
        {
            TunWrapper.SetInterfaceAddress(device, address);
        }

        public void SetInterfaceDns(string device, string dns)
        {
            TunWrapper.SetInterfaceDns(device, dns);
        }

        public void SetRoutes(string server, string address, string gateway, int index)
        {
            TunWrapper.SetRoutes(server, address, gateway, index);
        }
    }
}