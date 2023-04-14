namespace InvisibleManTUN.Handlers
{
    using Tunnel;

    public class TunnelHandler : Handler
    {
        private Tun2Socks tun2Socks;
        private Network network;

        public TunnelHandler()
        {
            this.tun2Socks = new Tun2Socks();
            this.network = new Network();
        }

        public void Start(string device, string proxy, string address, string server, string dns)
        {
            tun2Socks.RunProcess(device, proxy);
            network.RunProcess(device, address, server, dns);
        }

        public void Stop()
        {
            tun2Socks.StopProcess();
        }
    }
}