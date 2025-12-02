using System.Net;
using System.Net.Sockets;

namespace ChatServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 7777);
            TcpListener listener = new TcpListener(endPoint);
            Socket socket = listener.AcceptSocket();
            
            ServerSession serverSession = new ServerSession(socket);
            serverSession.Start();
        }
    }
}

