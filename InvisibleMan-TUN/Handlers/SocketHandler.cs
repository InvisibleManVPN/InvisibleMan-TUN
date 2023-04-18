using System;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace InvisibleManTUN.Handlers
{
    using Foundation;
    using Values;

    public class SocketHandler : Handler
    {
        private IPHostEntry hostEntry;
        private IPAddress address;
        private IPEndPoint endPoint;
        private Socket listener;

        private Func<int> getPort;
        private Action<string, string, string, string, string> onStartTunneling;
        private Action onStopTunneling;

        public SocketHandler()
        {
            this.hostEntry = Dns.GetHostEntry(Dns.GetHostName());
            this.address = hostEntry.AddressList.First();
        }

        public void Setup(
            Func<int> getPort, 
            Action<string, string, string, string, string> onStartTunneling, 
            Action onStopTunneling
        )
        {
            this.getPort = getPort;
            this.onStartTunneling = onStartTunneling;
            this.onStopTunneling = onStopTunneling;
        }

        public void Start()
        {
            try
            {
                endPoint = new IPEndPoint(address, getPort.Invoke());
                listener = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                listener.Bind(endPoint);
                listener.Listen(1);

                Console.WriteLine(Message.WAITING_FOR_CONNECTION);
                Socket clientSocket = listener.Accept();

                Console.WriteLine(Message.CLIENT_WAS_CONNECTED);
                Listen(clientSocket);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                onStopTunneling.Invoke();
            }

            void Listen(Socket socket)
            {
                try
                {
                    while (IsSocketConnected())
                    {
                        byte[] bytes = new byte[1024];
                        string command = "";

                        while (true)
                        {
                            int bytesCount = socket.Receive(bytes);
                            command += Encoding.ASCII.GetString(bytes, 0, bytesCount);

                            if (command.IndexOf(Command.EOF) > -1)
                                break;
                        }

                        Console.WriteLine($"Receive command: '{command}'");
                        Execute(command.Replace(Command.EOF, string.Empty));
                    }

                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine();
                }

                bool IsSocketConnected()
                {
                    return socket != null && (
                        !(
                            socket.Poll(1000, SelectMode.SelectRead) && socket.Available == 0
                        ) || socket.Connected
                    );
                }
            }

            void Execute(string command)
            {
                string firstArgument = command.Split(" ").FirstOrDefault();

                switch(firstArgument)
                {
                    case Command.ENABLE:
                        Enable();
                        break;
                    case Command.DISABLE:
                        Disable();
                        break;
                    default:
                        break;
                }

                void Enable()
                {
                    Parser parser = new Parser(new[] {
                        Global.COMMAND,
                        Global.DEVICE,
                        Global.PROXY,
                        Global.ADDRESS,
                        Global.SERVER,
                        Global.DNS
                    });

                    parser.Parse(command);

                    onStartTunneling.Invoke(
                      parser.GetFlag(Global.DEVICE).Value,
                      parser.GetFlag(Global.PROXY).Value,
                      parser.GetFlag(Global.ADDRESS).Value,
                      parser.GetFlag(Global.SERVER).Value,
                      parser.GetFlag(Global.DNS).Value  
                    );
                }

                void Disable()
                {
                    onStopTunneling.Invoke();
                }
            }
        }
    }
}