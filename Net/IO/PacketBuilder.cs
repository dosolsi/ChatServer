using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace ChatServer.Net.IO
{
    internal class PacketBuilder
    {
        MemoryStream _ms;
        public PacketBuilder()
        {
            _ms = new MemoryStream();
        }

        public void WriteOpCode(byte opcode)
        {
            _ms.WriteByte(opcode);
        }

        public void WriteMessage(string msg)
        {
            // UTF-8로 인코딩된 바이트 배열의 길이를 계산해야 함 
            //아오 안하면 깨짐 한글
            var msgBytes = Encoding.UTF8.GetBytes(msg);
            var msgLength = msgBytes.Length;

            // 메시지의 길이를 4바이트로 기록
            _ms.Write(BitConverter.GetBytes(msgLength), 0, 4);

            // 메시지 내용을 UTF-8로 기록
            _ms.Write(msgBytes, 0, msgLength);

            /*
            수정전

            var msgLength = msg.Length;
            _ms.Write(BitConverter.GetBytes(msgLength));
            _ms.Write(Encoding.Default.GetBytes(msg));
            */
        }

        public byte[] GetPacketBytes()
        {
            return _ms.ToArray();
        }
       
    }
}
