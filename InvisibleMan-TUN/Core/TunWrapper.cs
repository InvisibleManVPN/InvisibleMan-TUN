using System.Runtime.InteropServices;

namespace InvisibleManTUN.Core
{
    using Values;

    internal class TunWrapper
    {
        public static void StartTunnel(string device, string proxy)
        {
            StartTunnel(device, proxy);

            [DllImport(Path.TUN_DLL, EntryPoint = "StartTunnel")]
            static extern void StartTunnel(string device, string proxy);
        }

        public static void StopTunnel()
        {
            StopTunnel();

            [DllImport(Path.TUN_DLL, EntryPoint = "StopTunnel")]
            static extern void StopTunnel();
        }

        public static bool IsTunnelRunning()
        {
            return IsTunnelRunning();

            [DllImport(Path.TUN_DLL, EntryPoint = "IsTunnelRunning")]
            static extern bool IsTunnelRunning();
        }

        public static void SetInterfaceAddress(string device, string address)
        {
            SetInterfaceAddress(device, address);

            [DllImport(Path.TUN_DLL, EntryPoint = "SetInterfaceAddress")]
            static extern void SetInterfaceAddress(string device, string address);
        }

        public static void SetInterfaceDns(string device, string dns)
        {
            SetInterfaceDns(device, dns);

            [DllImport(Path.TUN_DLL, EntryPoint = "SetInterfaceDns")]
            static extern void SetInterfaceDns(string device, string dns);
        }

        public static void SetRoutes(string server, string address, string gateway, int index)
        {
            SetRoutes(server, address, gateway, index);

            [DllImport(Path.TUN_DLL, EntryPoint = "SetRoutes")]
            static extern void SetRoutes(string server, string address, string gateway, int index);
        }
    }
}