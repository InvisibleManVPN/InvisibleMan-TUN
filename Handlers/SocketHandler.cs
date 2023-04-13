using System;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace InvisibleManTUN.Handlers
{
    using Values;

    public class SocketHandler : Handler
    {
        private IPHostEntry hostEntry;
        private IPAddress address;
        private IPEndPoint endPoint;
        private Socket listener;

        private Func<int> getPort;

        public SocketHandler()
        {
            this.hostEntry = Dns.GetHostEntry(Dns.GetHostName());
            this.address = hostEntry.AddressList.First();
        }

        public void Setup(Func<int> getPort)
        {
            this.getPort = getPort;
        }

        public void Start()
        {
            try
            {
                endPoint = new IPEndPoint(address, getPort.Invoke());
                listener = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                listener.Bind(endPoint);
                listener.Listen(1);

                while (true)
                {
                    Console.WriteLine(Message.WAITING_FOR_CONNECTION);
                    
                    Socket clientSocket = listener.Accept();
                    ReadCommand(clientSocket);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            void ReadCommand(Socket socket)
            {
                byte[] bytes = new byte[1024];
                string command = "";

                while (true)
                {
                    int bytesCount = socket.Receive(bytes);
                    command += Encoding.ASCII.GetString(bytes, 0, bytesCount);

                    if (command.IndexOf("<EOF>") > -1)
                        break;
                }

                Console.WriteLine($"Command -> '{command}'");

                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }
    }
}