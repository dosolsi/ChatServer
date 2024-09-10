using System;
using System.Net.Sockets;
using System.Text;

namespace ChatServer.Net.IO
{
    class PacketReader : BinaryReader
    {
        private NetworkStream _ns;
        public PacketReader(NetworkStream ns) : base(ns) 
        {
            _ns = ns;
        }


        public string ReadMessage()
        {
            var length = ReadInt32();
            byte[] msgBuffer;

            msgBuffer = new byte[length];
            _ns.Read(msgBuffer, 0, length);
            
            var msg = Encoding.UTF8.GetString(msgBuffer);
            return msg;

        }
    }
}
