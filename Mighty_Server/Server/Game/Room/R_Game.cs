using Google.Protobuf;
using Google.Protobuf.GameProtocol;
using Google.Protobuf.Protocol;
using Google.Protobuf.UnityProtocol;
using Server.Data;
using Server.Game.CharacterClass;
using Server.Game.Job;
using Server.Game.Object;
using System;
using System.Collections.Generic;

namespace Server.Game.Room
{
    public class R_Game : JobSerializer, IRoom
    {
        public int RoomId { get; set; }
        public int maxPlayerCount;

        public bool isRun = false;

        public Dictionary<ObjectTag, List<GameObject>> collisionDic = new Dictionary<ObjectTag, List<GameObject>>();

        private List<Player> _playerList = new List<Player>();


        public R_Game(int maxPlayerCount)
        {
            this.maxPlayerCount = maxPlayerCount;
        }

        //// 누군가 주기적으로 호출해줘야 한다
        public void Update()
        {

            foreach (Player player in _playerList)
            {
                player.Update();
            }

            Flush();
        }

        /// <summary>
        /// 게임 시작 함수
        /// </summary>
        public void StartGame()
        {
            
            foreach (Player myPlayer in _playerList)
            {
                //if (myPlayer.Job == JobType.JobNone)
                //{
                //    Console.WriteLine("Not everyone has chosen a profession.");
                //    return;
                //}
            }

            foreach (Player myPlayer in _playerList)
            {
                //myPlayer.CharacterClass = CharacterClassManager.Create(myPlayer.Job);

                //S_EnterGame enterPacket = new S_EnterGame();

                //enterPacket.Player = myPlayer.unit;

                //myPlayer.Session.Send(enterPacket);

                //S_SpawnPlayer spawnPacket = new S_SpawnPlayer();

                //foreach (Player player in _playerList)
                //{
                //    if (myPlayer != player)
                //        spawnPacket.ObjectList.Add(player.unit);
                //}
                //myPlayer.Session.Send(spawnPacket);
            }

            isRun = true;
        }

        /// <summary>
        /// 게임 나가기 함수
        /// </summary>
        public void ExitGame(int objectId)
        {
            ObjectTag type = ObjectManager.GetObjectTagById(objectId);

            switch (type)
            {
                case ObjectTag.TagPlayer:
                    Player player = null;
                    if (_playerList.Remove(player) == false)
                        return;

                    player.Room = null;

                    // 본인한테 정보 전송
                    {
                        S_LeaveGame leavePacket = new S_LeaveGame();
                        player.Session.Send(leavePacket);
                    }
                    break;
                case ObjectTag.TagMonster:
                    break;
                case ObjectTag.TagProjectile:
                    break;
                default:
                    break;
            }

            // 타인한테 정보 전송
            {
                S_DespawnPlayer despawnPacket = new S_DespawnPlayer();
                //despawnPacket.ObjectIdList.Add(objectId);
                foreach (Player p in _playerList)
                {
                    if (p.Id != objectId)
                        p.Session.Send(despawnPacket);
                }
            }

        }

        /// <summary>
        /// 재장전하는 함수
        /// </summary>
        public void HandleReload(Player player)
        {
            if (player == null)
                return;

            if (player.CharacterClass == null)
                return;

            player.CharacterClass.Reload();
        }

        /// <summary>
        /// 플레이어 방에 추가해주는 함수
        /// </summary>
        public void Add(Player player)
        {
            if (_playerList.Count > maxPlayerCount)
            {
                Console.WriteLine("MaxPlayer Over.");
                return;
            }
            _playerList.Add(player);
        }

        /// <summary>
        /// 플레이어 제거해주는 함수
        /// </summary>
        public void Remove(Player player)
        {
            _playerList.Remove(player);
        }

        /// <summary>
        /// list<Player> 리턴해주는 함수
        /// </summary>
        public List<Player> GetPlayerList()
        {
            return _playerList;
        }

        /// <summary>
        /// player인덱스 가져오는 함수
        /// </summary>
        public int GetPlayerIndex(Player player)
        {
            return _playerList.IndexOf(player);
        }


        /// <summary>
        /// 
        /// </summary>
        private void RemoveDic(int id, GameObject obj)
        {
            if (obj == null)
                return;

            var objectTag = ObjectManager.GetObjectTagById(id);
            switch (objectTag)
            {
                case ObjectTag.TagMonster:
                    break;
                case ObjectTag.TagProjectile:
                    
                    break;
                default:

                    break;
            }
        }

        /// <summary>
        /// 플레이어가 방에 가득 차있는지 알려주는 함수
        /// </summary>
        public bool IsPlayerListMax()
        {
            if (_playerList.Count >= maxPlayerCount)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 방에 있는 모든 플레이어에게 packet전송하는 함수
        /// </summary>
        public void Broadcast(IMessage packet)
        {
            foreach (Player player in _playerList)
            {
                player.Session.Send(packet);
            }
        }
    }
}
