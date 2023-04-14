using System.Linq;
using System.Net.NetworkInformation;

namespace InvisibleManTUN.Handlers.Tunnel
{
    using Foundation;
    using Values;

    public class Network
    {
        private string device;
        private string address;
        private string server;
        private string dns;

        private Scheduler scheduler;
        private Processor processor;

        public Network()
        {
            this.scheduler = new Scheduler();
            this.processor = new Processor();
        }

        public void RunProcess(string device, string address, string server, string dns)
        {
            Setup();
            WaitUntilInterfaceCreated();
            SetInterfaceAddress();
            WaitUntilInterfaceAddressSet();
            SetInterfaceDNS();
            SetRoutes();

            void Setup()
            {
                this.device = device;
                this.address = address;
                this.server = server;
                this.dns = dns;
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
                processor.StartProcess(
                    processName: "interface_address",
                    fileName: Path.NETSH_EXE,
                    command: $"interface ip set address name={device} source=static addr={address} mask=255.255.255.0 gateway=none"
                );
            }

            void WaitUntilInterfaceAddressSet()
            {
                scheduler.WaitUntil(
                    condition: IsInterfaceAddressWasSet,
                    millisecondsTimeout: 6000,
                    $"'{address}' was not set to '{device}' device."
                );
            }

            void SetInterfaceDNS()
            {
                processor.StartProcess(
                    processName: "interfacec_dns",
                    fileName: Path.NETSH_EXE,
                    command: $"interface ip set dns name={device} static {dns}"
                );
            }

            void SetRoutes()
            {
                processor.StartProcess(
                    processName: "routing_phase1",
                    fileName: Path.CMD_EXE,
                    command: $"/c route add {server} mask 255.255.255.255 {GetDefaultGateway()}"
                );

                processor.StartProcess(
                    processName: "routing_phase2",
                    fileName: Path.CMD_EXE,
                    command: $"/c route add 0.0.0.0 mask 0.0.0.0 {address} IF {GetNetworkInterfaceIndex()} metric 5"
                );
            }
        }

        private bool IsInterfaceExists()
        {
            NetworkInterface networkInterface = FindNetworkInterface();
            return networkInterface != null && networkInterface.OperationalStatus == OperationalStatus.Up;
        }

        private bool IsInterfaceAddressWasSet()
        {
            NetworkInterface networkInterface = FindNetworkInterface();

            if (networkInterface == null)
                return false;

            foreach (UnicastIPAddressInformation unicastIP in networkInterface.GetIPProperties().UnicastAddresses)
            {
                if (unicastIP.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    return unicastIP.Address.ToString() == address;
            }

            return false;
        }

        private NetworkInterface FindNetworkInterface()
        {
            return NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(
                ni => ni.Name.StartsWith(device) 
            );
        }

        private int GetNetworkInterfaceIndex() 
        {
            return FindNetworkInterface().GetIPProperties().GetIPv4Properties().Index;
        }

        private string GetDefaultGateway() => IPHelperWrapper.GetGatewayForDestination(address);
    }
}