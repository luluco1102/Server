﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    /// <summary>
    /// TCP 통신 소켓의 생명주기 및 소켓을 통한 데이터 송수신 처리
    /// </summary>
    public abstract class TcpSession : IDisposable
    {
        public TcpSession(Socket socket, int recvBufferSize = DEFAULT_RECV_BUFFER)
        {
            Socket = socket;
            _recvBuffer = new byte[recvBufferSize];
            _sendQueue = new Queue<ArraySegment<byte>>(); // TODO : Reserving
        }


        const int KB = 1_024;
        const int HEADER_SIZE = sizeof(ushort);
        const int MAX_PACKET_SIZE = 1 * KB;
        const int DEFAULT_RECV_BUFFER = 4 * KB;

        public bool IsConnected => Socket.Connected;

        protected Socket Socket;

        // Send
        Queue<ArraySegment<byte>> _sendQueue;

        // Receive
        byte[] _recvBuffer;
        int _recvBufferCount;

        // Events
        public event Action OnConnected;
        public event Action OnDisconnected;


        public void Start()
        {
            OnConnected?.Invoke();
            RecvLoop();
            OnDisconnected?.Invoke();
            CloseSocket();
        }


        public void Send(byte[] body)
        {
            int total = HEADER_SIZE + body.Length;
            byte[] segment = new byte[total];
            Buffer.BlockCopy(BitConverter.GetBytes((ushort)body.Length), 0, segment, 0, HEADER_SIZE);
            Buffer.BlockCopy(body, 0, segment, HEADER_SIZE, body.Length);
            _sendQueue.Enqueue(new ArraySegment<byte>(segment, 0, segment.Length));
            Send();
        }

        void Send()
        {
            if (_sendQueue.TryDequeue(out var segement) == false)
                return;

            int sent = 0; // 실제 전송된 길이

            // Socket.Send 요청에 넣은 버퍼데이터가 반드시 전송 보장이되는것이 아니기 때문에, 
            // OS 가 실제로 얼마만큼 보냈는지 추적하면서 완전히 전송을 보장해주는 구문을 써야한다.
            while (sent < segement.Count)
            {
                sent += Socket.Send(segement);
                segement = new ArraySegment<byte>(segement.Array, segement.Offset + sent, segement.Count - sent);
            }
        }

        void RecvLoop()
        {
            while (IsConnected)
            {
                ArraySegment<byte> remainBufferSegment = new ArraySegment<byte>(_recvBuffer, _recvBufferCount, _recvBuffer.Length - _recvBufferCount);

                int read = Socket.Receive(remainBufferSegment);

                // 연결에 문제있음
                if (read <= 0)
                {
                    break;
                }

                _recvBufferCount += read;
                ParsePacket();
            }
        }

        /// <summary>
        /// 현재 RecvBuffer 에 쌓인 모든 패킷을 다 처리 하고
        /// 남은 데이터 앞으로 밀착
        /// </summary>
        void ParsePacket()
        {
            int offset = 0; // RecvBuffer 현재 탐색 인덱스
            
            while (true)
            {
                // 헤더 길이도 안되는 데이터는 파싱이 안되니까 데이터가 더 쌓일때까지 기다려야함
                if (_recvBufferCount - offset < HEADER_SIZE)
                    return;

                ushort bodyLength = BitConverter.ToUInt16(_recvBuffer, offset);

                // 유효한 데이터인지
                if (bodyLength <= 0 || bodyLength > MAX_PACKET_SIZE)
                    return;

                // body 가 완전히 다 도착했는지
                if (_recvBufferCount - offset - HEADER_SIZE < bodyLength)
                    return;

                byte[] body = new byte[bodyLength];
                Buffer.BlockCopy(_recvBuffer, offset + HEADER_SIZE, body, 0, bodyLength);
                OnPacket(body);
                offset += HEADER_SIZE + bodyLength;
            }

            // 처리하고 남은 데이터 앞으로 옮김
            Buffer.BlockCopy(_recvBuffer, offset, _recvBuffer, 0, _recvBufferCount - offset);
            _recvBufferCount -= offset;
        }

        protected abstract void OnPacket(byte[] body);


        public void CloseSocket()
        {
            try
            {
                Socket?.Shutdown(SocketShutdown.Both);
            }
            catch
            {
                // Nothing to do ...
            }

            try
            {
                Socket?.Close();
            }
            catch
            {
                // Nothing to do ...
            }

            Socket = null;
        }

        public void Dispose()
        {
            Socket.Dispose();
        }
    }
}