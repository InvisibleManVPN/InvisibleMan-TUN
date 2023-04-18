using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace InvisibleManTUN.Utilities
{
    using Values;

    internal class IPHelperWrapper
    {
        [DllImport(Path.IPHLPAPI_DLL, CharSet = CharSet.Auto)]
        private static extern int GetBestInterface(UInt32 destAddr, out UInt32 bestIfIndex);

        public static string GetGatewayForDestination(string destinationAddress)
        {
            UInt32 destaddr = BitConverter.ToUInt32(
                value: IPAddress.Parse(destinationAddress).GetAddressBytes(), 
                startIndex: 0
            );

            uint interfaceIndex;
            int result = GetBestInterface(destaddr, out interfaceIndex);
            if (result != 0)
                throw new Exception(result.ToString());

            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                var niprops = ni.GetIPProperties();
                if (niprops == null)
                    continue;

                var gateway = niprops.GatewayAddresses?.FirstOrDefault()?.Address;
                if (gateway == null)
                    continue;

                if (ni.Supports(NetworkInterfaceComponent.IPv4))
                {
                    var v4props = niprops.GetIPv4Properties();
                    if (v4props == null)
                        continue;

                    if (v4props.Index == interfaceIndex)
                        return gateway.ToString();
                }

                if (ni.Supports(NetworkInterfaceComponent.IPv6))
                {
                    var v6props = niprops.GetIPv6Properties();
                    if (v6props == null)
                        continue;

                    if (v6props.Index == interfaceIndex)
                        return gateway.ToString();
                }
            }

            return null;
        }
    }
}