namespace InvisibleManTUN.Handlers.Tunnel
{
    using Foundation;
    using Values;

    public class Tun2Socks
    {
        private Processor processor;
        private const string PROCESS_NAME = "tun2socks";

        public Tun2Socks()
        {
            this.processor = new Processor();
        }

        public void RunProcess(string device, string proxy)
        {
            StopProcess();
            
            processor.StartProcessAsThread(
                processName: PROCESS_NAME,
                fileName: Path.TUN2SOCKS_EXE,
                command: $"-device {device} -proxy socks5://{proxy}"
            );
        }

        public void StopProcess()
        {
            processor.StopProcess(PROCESS_NAME);
            processor.StopSystemProcesses(PROCESS_NAME);
        }
    }
}