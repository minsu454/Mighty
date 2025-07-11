using Google.Protobuf.GameProtocol;
using Google.Protobuf.Protocol;
using Google.Protobuf.UnityProtocol;
using Google.Protobuf;
using Server.Game.Job;
using Server.Game.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Diagnostics;

namespace Server.Game.Room
{
    public class R_Lobby : JobSerializer, IRoom
    {
        public int RoomId { get; set; }

        Dictionary<int, Player> _playerDic = new Dictionary<int, Player>();

        public void Update()
        {
            Flush();
        }

        /// <summary>
        /// 로그인 함수
        /// </summary>
        public void Login(ClientSession clientSession)
        {
            Player myPlayer = ObjectManager.Instance.Add<Player>();
            clientSession.MyPlayer = myPlayer;
            clientSession.MyPlayer.NickName = $"P_{myPlayer.Id}";

            _playerDic.Add(myPlayer.Id, myPlayer);

            clientSession.MyPlayer.Session = clientSession;

            S_Login login = new S_Login();

            login.PlayerId = myPlayer.Id;
            login.NickName = myPlayer.NickName;

            myPlayer.Session.Send(login);
        }

        /// <summary>
        /// 로그아웃 함수
        /// </summary>
        /// <param name="myPlayer"></param>
        public void Logout(Player myPlayer)
        {
            if (myPlayer == null)
            {
                return;
            }

            R_Game room = myPlayer.Room;

            if (room != null)
            {
                int playerNumber = room.GetPlayerIndex(myPlayer);
                room.Remove(myPlayer);

                List<Player> playerList = room.GetPlayerList();

                if (playerList.Count == 0)
                {
                    TimeManager.RemoveTickRoom(room);
                    RoomManagers.R_Game.ReMove(room.RoomId);
                }
                else
                {
                    S_DisconnectPlayer disconnectPlayer = new S_DisconnectPlayer();

                    disconnectPlayer.DisconnectPlayerId = playerNumber;

                    foreach (Player player in playerList)
                    {
                        player.Session.Send(disconnectPlayer);
                    }
                }
            }

            _playerDic.Remove(myPlayer.Id);
            myPlayer.Room = null;
            myPlayer.Session.MyPlayer = null;
            myPlayer.Session = null;
        }

        /// <summary>
        /// 게임방 만들어주는 함수
        /// </summary>
        public void CreateGameRoom(ClientSession clientSession)
        {
            Player myPlayer = clientSession.MyPlayer;
            if (myPlayer == null)
            {
                Console.WriteLine("player is Null");
                return;
            }

            RoomManagers.R_Game.Add(out R_Game room, 2);
            TimeManager.AddTickRoom(room);

            myPlayer.Room = room;
            room.Add(myPlayer);

            S_CreateRoom createRoom = new S_CreateRoom();

            createRoom.RoomId = room.RoomId;
            createRoom.MaxPlayerCount = room.maxPlayerCount;
            createRoom.PlayerId = myPlayer.Id;

            myPlayer.Session.Send(createRoom);

            S_ConnectRoom connectRoom = new S_ConnectRoom();

            connectRoom.PlayerNumber = room.GetPlayerIndex(myPlayer);
            connectRoom.IsFirst = false;
            connectRoom.NickNameList.Add(myPlayer.NickName);

            myPlayer.Session.Send(connectRoom);
        }

        /// <summary>
        /// 게임방 연결하는 함수
        /// </summary>
        public void ConnectGameRoom(int roomId, ClientSession clientSession)
        {
            Player myPlayer = clientSession.MyPlayer;
            if (myPlayer == null)
            {
                Console.WriteLine("player is Null");
                return;
            }

            R_Game room = RoomManagers.R_Game.Find(roomId);

            if (room == null)
            {
                Console.WriteLine("room is Null");
                return;
            }

            if (room.IsPlayerListMax())
            {
                S_Error error = new S_Error();
                error.Message = "꽉찼습니다";

                myPlayer.Session.Send(error);
                return;
            }

            myPlayer.Room = room;
            room.Add(myPlayer);

            List<Player> playerList = room.GetPlayerList();

            //자신한테 보내는 패킷
            {
                S_ConnectRoom connectRoom = new S_ConnectRoom();

                connectRoom.PlayerNumber = room.GetPlayerIndex(myPlayer);
                connectRoom.MaxPlayerCount = room.maxPlayerCount;
                connectRoom.IsFirst = true;

                foreach (Player player in playerList)
                {
                    connectRoom.NickNameList.Add(player.NickName);
                }

                myPlayer.Session.Send(connectRoom);
            }
            //남에게 보내는 패킷
            {
                S_ConnectRoom connectRoom = new S_ConnectRoom();
                connectRoom.NickNameList.Add(myPlayer.NickName);
                connectRoom.IsFirst = false;

                foreach (Player player in playerList)
                {
                    if (player.Id != myPlayer.Id)
                        player.Session.Send(connectRoom);
                }
            }
        }

        /// <summary>
        /// 게임방 떠나는 함수
        /// </summary>
        public void LeaveGameRoom(ClientSession clientSession)
        {
            Player myPlayer = clientSession.MyPlayer;
            if (myPlayer == null)
            {
                Console.WriteLine("player is Null");
                return;
            }

            R_Game room = clientSession.MyPlayer.Room;
            if (room == null)
            {
                Console.WriteLine("room is Null");
                return;
            }

            int playerNumber = room.GetPlayerIndex(myPlayer);

            room.Remove(myPlayer);

            List<Player> playerList = room.GetPlayerList();

            if (playerList.Count == 0)
            {
                TimeManager.RemoveTickRoom(room);
                RoomManagers.R_Game.ReMove(room.RoomId);
            }

            //자신한테 보내는 패킷
            {
                S_LeaveRoom leaveRoom = new S_LeaveRoom();

                myPlayer.Room = null;

                myPlayer.Session.Send(leaveRoom);
            }
            //남에게 보내는 패킷
            {
                S_DisconnectPlayer disconnectPlayer = new S_DisconnectPlayer();

                disconnectPlayer.DisconnectPlayerId = playerNumber;

                foreach (Player player in playerList)
                {
                    player.Session.Send(disconnectPlayer);
                }
            }
        }

        public void Broadcast(IMessage packet)
        {
            foreach (Player player in _playerDic.Values)
            {
                player.Session.Send(packet);
            }
        }
    }
}
