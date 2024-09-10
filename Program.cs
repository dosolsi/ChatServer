using ChatServer.Net.IO;
using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;

namespace ChatServer
{
    class Program
    {
        static List<Client> _users;
        static TcpListener _listner;
        static void Main(string[] args)
        {
            _users = new List<Client> ();
            _listner = new TcpListener(IPAddress.Parse("127.0.0.1"), 8080);
            _listner.Start();

            while (true) 
            {
                var client = new Client(_listner.AcceptTcpClient());
                _users.Add(client);
                /*
                  broadcast the connection to everyone on the server 
                    클라이언트에 접속한 사람 리스트 보내주기

                 */
                BroadCastConnection();
            }

        }
        static void BroadCastConnection()
        {
            foreach (var users in _users)
            {
                foreach(var usr in _users)
                {
                    var broadcastPacket = new PacketBuilder();
                    broadcastPacket.WriteOpCode(1);
                    broadcastPacket.WriteMessage(usr.UserName);
                    broadcastPacket.WriteMessage(usr.UID.ToString());
                    users.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
                }
            }
        }


        public static void BroadcastMessage(string message)
        {
            foreach (var user in _users) 
            {
                var msgPacket = new PacketBuilder();
                msgPacket.WriteOpCode(5);
                msgPacket.WriteMessage(message);
                user.ClientSocket.Client.Send(msgPacket.GetPacketBytes());
            }
        }

        public static void BroadcastDisconnect(string uid)
        {
            var disconnectedUser = _users.Where(x => x.UID.ToString() == uid).FirstOrDefault();
            _users.Remove(disconnectedUser);
            foreach (var user in _users)
            {
                var broadcastPakcet = new PacketBuilder();
                broadcastPakcet.WriteOpCode(10);
                broadcastPakcet.WriteMessage(uid);
                user.ClientSocket.Client.Send(broadcastPakcet.GetPacketBytes());
            }
            BroadcastMessage($"[{disconnectedUser.UserName}] : 연결해제");
        }
    }
}