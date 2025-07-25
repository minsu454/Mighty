﻿using System;
using System.Collections.Generic;
using ServerCore;
using System.Net;
using Google.Protobuf.Protocol;
using Google.Protobuf;
using Server.Game.Room;
using Server.Data;
using Server.Game.Object;

namespace Server
{
    public class ClientSession : PacketSession
	{
		public Player MyPlayer { get; set; }
		public int SessionId { get; set; }

		public void Send(IMessage packet)
		{
			string msgName = packet.Descriptor.Name.Replace("_", string.Empty);

            MsgId msgId = (MsgId)Enum.Parse(typeof(MsgId), msgName);

            ushort size = (ushort)packet.CalculateSize();
            byte[] sendBuffer = new byte[size + 4];
            Array.Copy(BitConverter.GetBytes(size + 4), 0, sendBuffer, 0, sizeof(ushort));
            Array.Copy(BitConverter.GetBytes((ushort)msgId), 0, sendBuffer, 2, sizeof(ushort));
            Array.Copy(packet.ToByteArray(), 0, sendBuffer, 4, size);

            Send(new ArraySegment<byte>(sendBuffer));
        }

		public override void OnConnected(EndPoint endPoint)
		{
			Console.WriteLine($"OnConnected : {endPoint}");

        }

		public override void OnRecvPacket(ArraySegment<byte> buffer)
		{
			PacketManager.Instance.OnRecvPacket(this, buffer);
		}

		public override void OnDisconnected(EndPoint endPoint)
		{
            //R_Game room = RoomManagers.R_Game.Find(0);
            //room.Push(room.ExitGame, MyPlayer.Id);

            //SessionManager.Instance.Remove(this);

            R_Lobby room = RoomManagers.R_Lobby.Find(0);
            room.Push(room.Logout, MyPlayer);

            Console.WriteLine($"OnDisconnected : {endPoint}");
		}

		public override void OnSend(int numOfBytes)
		{
			//Console.WriteLine($"Transferred bytes: {numOfBytes}");
		}
	}
}
