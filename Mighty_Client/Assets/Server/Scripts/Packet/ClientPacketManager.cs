using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Collections.Generic;

class PacketManager
{
	#region Singleton
	static PacketManager _instance = new PacketManager();
	public static PacketManager Instance { get { return _instance; } }
	#endregion

	PacketManager()
	{
		Register();
	}

	Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>> _onRecv = new Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>>();
	Dictionary<ushort, Action<PacketSession, IMessage>> _handler = new Dictionary<ushort, Action<PacketSession, IMessage>>();
		
	public Action<PacketSession, IMessage, ushort> CustomHandler { get; set; }

	public void Register()
	{		
		_onRecv.Add((ushort)MsgId.SLogin, MakePacket<S_Login>);
		_handler.Add((ushort)MsgId.SLogin, PacketHandler.S_LoginHandler);		
		_onRecv.Add((ushort)MsgId.SError, MakePacket<S_Error>);
		_handler.Add((ushort)MsgId.SError, PacketHandler.S_ErrorHandler);		
		_onRecv.Add((ushort)MsgId.SEnterLobby, MakePacket<S_EnterLobby>);
		_handler.Add((ushort)MsgId.SEnterLobby, PacketHandler.S_EnterLobbyHandler);		
		_onRecv.Add((ushort)MsgId.SLeaveLobby, MakePacket<S_LeaveLobby>);
		_handler.Add((ushort)MsgId.SLeaveLobby, PacketHandler.S_LeaveLobbyHandler);		
		_onRecv.Add((ushort)MsgId.SCreateRoom, MakePacket<S_CreateRoom>);
		_handler.Add((ushort)MsgId.SCreateRoom, PacketHandler.S_CreateRoomHandler);		
		_onRecv.Add((ushort)MsgId.SConnectRoom, MakePacket<S_ConnectRoom>);
		_handler.Add((ushort)MsgId.SConnectRoom, PacketHandler.S_ConnectRoomHandler);		
		_onRecv.Add((ushort)MsgId.SLeaveRoom, MakePacket<S_LeaveRoom>);
		_handler.Add((ushort)MsgId.SLeaveRoom, PacketHandler.S_LeaveRoomHandler);		
		_onRecv.Add((ushort)MsgId.SDisconnectPlayer, MakePacket<S_DisconnectPlayer>);
		_handler.Add((ushort)MsgId.SDisconnectPlayer, PacketHandler.S_DisconnectPlayerHandler);		
		_onRecv.Add((ushort)MsgId.SEnterGame, MakePacket<S_EnterGame>);
		_handler.Add((ushort)MsgId.SEnterGame, PacketHandler.S_EnterGameHandler);		
		_onRecv.Add((ushort)MsgId.SLeaveGame, MakePacket<S_LeaveGame>);
		_handler.Add((ushort)MsgId.SLeaveGame, PacketHandler.S_LeaveGameHandler);		
		_onRecv.Add((ushort)MsgId.SSpawnPlayer, MakePacket<S_SpawnPlayer>);
		_handler.Add((ushort)MsgId.SSpawnPlayer, PacketHandler.S_SpawnPlayerHandler);		
		_onRecv.Add((ushort)MsgId.SDespawnPlayer, MakePacket<S_DespawnPlayer>);
		_handler.Add((ushort)MsgId.SDespawnPlayer, PacketHandler.S_DespawnPlayerHandler);
	}

	public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer)
	{
		ushort count = 0;

		ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
		count += 2;
		ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
		count += 2;

		Action<PacketSession, ArraySegment<byte>, ushort> action = null;
		if (_onRecv.TryGetValue(id, out action))
			action.Invoke(session, buffer, id);
	}

	void MakePacket<T>(PacketSession session, ArraySegment<byte> buffer, ushort id) where T : IMessage, new()
	{
		T pkt = new T();
		pkt.MergeFrom(buffer.Array, buffer.Offset + 4, buffer.Count - 4);

		if(CustomHandler != null)
		{
			CustomHandler.Invoke(session, pkt, id);
		}
		else
		{
			Action<PacketSession, IMessage> action = null;
			if (_handler.TryGetValue(id, out action))
				action.Invoke(session, pkt);
		}
	}

	public Action<PacketSession, IMessage> GetPacketHandler(ushort id)
	{
		Action<PacketSession, IMessage> action = null;
		if (_handler.TryGetValue(id, out action))
			return action;
		return null;
	}
}