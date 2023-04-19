using System.Linq;
using System.Net.NetworkInformation;

namespace InvisibleManTUN.Utilities
{
    public class NetworkUtility
    {
        public static bool IsInterfaceExists(string name)
        {
            NetworkInterface networkInterface = FindNetworkInterface(name);
            return networkInterface != null && networkInterface.OperationalStatus == OperationalStatus.Up;
        }

        public static bool IsInterfaceAddressWasSet(string name, string address)
        {
            NetworkInterface networkInterface = FindNetworkInterface(name);

            if (networkInterface == null)
                return false;

            foreach (UnicastIPAddressInformation unicastIP in networkInterface.GetIPProperties().UnicastAddresses)
            {
                if (unicastIP.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    return unicastIP.Address.ToString() == address;
            }

            return false;
        }

        public static string GetDefaultGateway(string address)
        {
            return IPHelperWrapper.GetGatewayForDestination(address);
        }

        public static int GetNetworkInterfaceIndex(string name) 
        {
            return FindNetworkInterface(name).GetIPProperties().GetIPv4Properties().Index;
        }

        private static NetworkInterface FindNetworkInterface(string name)
        {
            return NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(
                ni => ni.Name.StartsWith(name) 
            );
        }
    }
}