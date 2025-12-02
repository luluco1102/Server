using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    public class ServerSession : TcpSession
    {
        public ServerSession(Socket socket, int recvBufferSize = 4096) : base(socket, recvBufferSize)
        {
        }

        protected override void OnPacket(byte[] body)
        {
            throw new NotImplementedException();
        }
    }
}
