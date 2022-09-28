using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace splatoon3Tester.core
{
    public class SocketConnection : IConnection
    {
        private NetworkStream socket;

        public SocketConnection(string server, int port)
        {
            this.socket = ConnectSocket(server, port);
        }

        private static NetworkStream ConnectSocket(string server, int port)
        {
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(server), port);
            Socket tempSocket =
                new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tempSocket.Connect(server, port);
            return new NetworkStream(tempSocket, true);
        }

        public override void close()
        {
            flush();
            socket.Close();
        }

        public override bool connected()
        {
            return socket.Socket.Connected;
        }

        public override void flush()
        {
            socket.Flush();
        }

        public override int read(byte[] b, int off, int len)
        {
            int count = socket.Read(b, off, len);
            int left = len - count;
            if (count != len)
                count += socket.Read(b, count, left);
            if (count != len) throw new SocketException();
            return count;
        }

        public override int readByte()
        {
            return socket.ReadByte();
        }

        public override void write(byte[] data, int off, int len)
        {
            socket.Write(data, off, len);
        }

        public override void writeByte(int i)
        {
            socket.WriteByte((byte)i);
        }
    }
}
