using System;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server;
using Server.Game.Object;
using Server.Game.Room;
using ServerCore;

class PacketHandler
{
    public static void C_LoginHandler(PacketSession session, IMessage packet)
    {
        ClientSession clientSession = session as ClientSession;

        R_Lobby room = RoomManagers.R_Lobby.Find(0);
        room.Push(room.Login, clientSession);
    }
    
    public static void C_CreateRoomHandler(PacketSession session, IMessage packet)
    {
        ClientSession clientSession = session as ClientSession;

        R_Lobby room = RoomManagers.R_Lobby.Find(0);
        room.Push(room.CreateGameRoom, clientSession);
    }

    public static void C_ConnectRoomHandler(PacketSession session, IMessage packet)
    {
        C_ConnectRoom connectRoomPacket = packet as C_ConnectRoom;
        ClientSession clientSession = session as ClientSession;

        R_Lobby room = RoomManagers.R_Lobby.Find(0);
        room.Push(room.ConnectGameRoom, connectRoomPacket.RoomId, clientSession);
    }

    public static void C_LeaveRoomHandler(PacketSession session, IMessage packet)
    {
        ClientSession clientSession = session as ClientSession;

        R_Lobby room = RoomManagers.R_Lobby.Find(0);
        room.Push(room.LeaveGameRoom, clientSession);
    }

    public static void C_JobChoiceHandler(PacketSession session, IMessage packet)
    {
        C_JobChoice jobChoicePacket = packet as C_JobChoice;
        ClientSession clientSession = session as ClientSession;

        Player player = clientSession.MyPlayer;
        if (player == null)
            return;
        R_Game room = clientSession.MyPlayer.Room;
        if (room == null)
            return;

        room.Push(room.SetPlayerJob, jobChoicePacket.Job, player);
    }

    public static void C_EnterGameHandler(PacketSession session, IMessage packet)
    {
        C_EnterGame gamePacket = packet as C_EnterGame;
        ClientSession clientSession = session as ClientSession;

        R_Game room = clientSession.MyPlayer.Room;
        if (room == null)
            return;

        room.Push(room.StartGame);
    }

    public static void C_MovePlayerHandler(PacketSession session, IMessage packet)
	{
        C_MovePlayer movePacket = packet as C_MovePlayer;
		ClientSession clientSession = session as ClientSession;

        Player player = clientSession.MyPlayer;
        if (player == null)
            return;
        R_Game room = clientSession.MyPlayer.Room;
        if (room == null)
            return;

        room.Push(room.HandleMove, player, movePacket);
    }

    public static void C_JumpPlayerHandler(PacketSession session, IMessage packet)
    {
        C_JumpPlayer jumpPacket = packet as C_JumpPlayer;
        ClientSession clientSession = session as ClientSession;

        Player player = clientSession.MyPlayer;
        if (player == null)
            return;
        R_Game room = clientSession.MyPlayer.Room;
        if (room == null)
            return;

        room.Push(room.HandleJump, player, jumpPacket);
    }

    public static void C_AttackPlayerHandler(PacketSession session, IMessage packet)
    {
        C_AttackPlayer attackPacket = packet as C_AttackPlayer;
        ClientSession clientSession = session as ClientSession;

        Player player = clientSession.MyPlayer;
        if (player == null)
            return;
        R_Game room = clientSession.MyPlayer.Room;
        if (room == null)
            return;

        room.Push(room.HandleAttack, player, attackPacket);
    }

    public static void C_ReloadPlayerHandler(PacketSession session, IMessage packet)
    {
        ClientSession clientSession = session as ClientSession;

        Player player = clientSession.MyPlayer;
        if (player == null)
            return;
        R_Game room = clientSession.MyPlayer.Room;
        if (room == null)
            return;

        room.Push(room.HandleReload, player);
    }
}
