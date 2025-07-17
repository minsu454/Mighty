using Google.Protobuf;
using Google.Protobuf.GameProtocol;
using Google.Protobuf.Protocol;
using Google.Protobuf.UnityProtocol;
using ServerCore;
using TMPro;
using UnityEngine;

class PacketHandler
{
    public static void S_LoginHandler(PacketSession session, IMessage packet)
    {
        S_Login loginPacket = packet as S_Login;

        Managers.Network.myId = loginPacket.PlayerId;
        Managers.Network.nickName = loginPacket.NickName;

        //Managers.UI.SetMyIdUI(loginPacket.NickName);
    }

    public static void S_ErrorHandler(PacketSession session, IMessage packet)
    {
        S_Error errorPacket = packet as S_Error;

        //Debug.Log(errorPacket.Message);
    }

    public static void S_EnterLobbyHandler(PacketSession session, IMessage packet)
    {
        S_EnterLobby enterLobbyPacket = packet as S_EnterLobby;

    }

    public static void S_LeaveLobbyHandler(PacketSession session, IMessage packet)
    {
        S_LeaveLobby leaveLobbyPacket = packet as S_LeaveLobby;
            
    }

    public static void S_CreateRoomHandler(PacketSession session, IMessage packet)
    {
        S_CreateRoom createPacket = packet as S_CreateRoom;

        Managers.Network.roomId = createPacket.RoomId;

        //Managers.UI.SetEnterRoomUI(createPacket.RoomId.ToString(), createPacket.MaxPlayerCount);
    }

    public static void S_ConnectRoomHandler(PacketSession session, IMessage packet)
    {
        S_ConnectRoom connectPacket = packet as S_ConnectRoom;

        //Managers.UI.AddTeamMember(connectPacket);
    }

    public static void S_LeaveRoomHandler(PacketSession session, IMessage packet)
    {
        S_LeaveRoom leaveRoomPacket = packet as S_LeaveRoom;

        Managers.Network.roomId = 0;
        //Managers.UI.SetleaveRoomUI();
    }

    public static void S_DisconnectPlayerHandler(PacketSession session, IMessage packet)
    {
        S_DisconnectPlayer disconnectPacket = packet as S_DisconnectPlayer;

        //Managers.UI.RemoveTeamMember(disconnectPacket);
    }
    
    public static void S_EnterGameHandler(PacketSession session, IMessage packet)
    {
        S_EnterGame enterGamePacket = packet as S_EnterGame;

        //Managers.Scene.LoadScene(SceneType.InGame);

        //캐릭터 생성
        //Managers.Object.Add(enterGamePacket.Player, true);
    }

    public static void S_LeaveGameHandler(PacketSession session, IMessage packet)
    {
        S_LeaveGame leaveGamePacket = packet as S_LeaveGame;

        //Managers.Object.Clear();
    }

    public static void S_SpawnPlayerHandler(PacketSession session, IMessage packet)
    {
        S_SpawnPlayer spawnPacket = packet as S_SpawnPlayer;

        //Managers.Object.Add(spawnPacket.ObjectList);
    }

    public static void S_DespawnPlayerHandler(PacketSession session, IMessage packet)
    {
        S_DespawnPlayer despawnPacket = packet as S_DespawnPlayer;

        //Managers.Object.Remove(despawnPacket.ObjectIdList);
    }
}
