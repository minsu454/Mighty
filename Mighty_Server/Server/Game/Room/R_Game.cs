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
        private List<Wall> _wallList = new List<Wall>();

        //Dictionary<int, Monster> _monsterDic = new Dictionary<int, Monster>();
        private Dictionary<int, Projectile> _projectileDic = new Dictionary<int, Projectile>();

        public R_Game(int maxPlayerCount)
        {
            this.maxPlayerCount = maxPlayerCount;
            _wallList = DataManager.WallList;
        }

        //// 누군가 주기적으로 호출해줘야 한다
        public void Update()
        {

            foreach (Player player in _playerList)
            {
                player.Update();
            }   

            foreach (Projectile projectile in _projectileDic.Values)
            {
                projectile.Update();

                foreach (Wall wall in _wallList)
                {
                    if (Collision.ISObbCollision(projectile.gameObject, wall.gameObject))
                    {
                        S_DespawnProjectile despawnPacket = new S_DespawnProjectile();

                        despawnPacket.ObjectId = projectile.Id;
                        RemoveDic(projectile.Id, projectile);

                        Broadcast(despawnPacket);
                    }
                }
            }

            Flush();
        }

        public void SetPlayerJob(JobType type, Player player)
        {
            player.Job = type;

            S_JobChoice jobChoice = new S_JobChoice();
            jobChoice.Job = type;

            Broadcast(jobChoice);
        }

        /// <summary>
        /// 게임 시작 함수
        /// </summary>
        public void StartGame()
        {
            
            foreach (Player myPlayer in _playerList)
            {
                if (myPlayer.Job == JobType.JobNone)
                {
                    Console.WriteLine("Not everyone has chosen a profession.");
                    return;
                }
            }

            foreach (Player myPlayer in _playerList)
            {
                myPlayer.CharacterClass = CharacterClassManager.Create(myPlayer.Job);

                S_EnterGame enterPacket = new S_EnterGame();

                enterPacket.Player = myPlayer.unit;

                myPlayer.Session.Send(enterPacket);

                S_SpawnPlayer spawnPacket = new S_SpawnPlayer();

                foreach (Player player in _playerList)
                {
                    if (myPlayer != player)
                        spawnPacket.ObjectList.Add(player.unit);
                }
                myPlayer.Session.Send(spawnPacket);
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
                despawnPacket.ObjectIdList.Add(objectId);
                foreach (Player p in _playerList)
                {
                    if (p.Id != objectId)
                        p.Session.Send(despawnPacket);
                }
            }

        }

        /// <summary>
        /// 플레이어 움직이는 함수
        /// </summary>
        public void HandleMove(Player player, C_MovePlayer movePacket)
        {
            if (player == null)
                return;

            player.Position = movePacket.Position;
            player.Rotation = movePacket.Rotation;
            player.velocity = movePacket.Velocity;
            player.State = movePacket.StateType;

            // 다른 플레이어한테도 알려준다
            S_MovePlayer reMovePacket = new S_MovePlayer();

            reMovePacket.Time = movePacket.Time;
            reMovePacket.ObjectId = player.Id;
            reMovePacket.Position = player.Position;
            reMovePacket.Rotation = movePacket.Rotation;
            reMovePacket.Velocity = movePacket.Velocity;
            reMovePacket.StateType = movePacket.StateType;

            Broadcast(reMovePacket);
        }

        /// <summary>
        /// 플레이어 점프하는 함수
        /// </summary>
        public void HandleJump(Player player, C_JumpPlayer jumpPacket)
        {
            if (player == null)
                return;

            S_JumpPlayer reJumpPacket = new S_JumpPlayer();

            reJumpPacket.ObjectId = player.Id;
            reJumpPacket.Position = jumpPacket.Position;
            reJumpPacket.AddForce = jumpPacket.AddForce;

            Broadcast(reJumpPacket);
        }

        /// <summary>
        /// 플레이어 공격하는 함수
        /// </summary>
        public void HandleAttack(Player player, C_AttackPlayer attackPacket)
        {
            if (player == null)
                return;

            if (player.CharacterClass == null)
                return;

            if (!player.CharacterClass.IsCanAttack())
                return;

            if (player.CharacterClass.IsUseProjectile())
            {
                S_SpawnProjectile projectilePacket = new S_SpawnProjectile();
                Projectile projectile = ObjectManager.Instance.Add<Projectile>();

                projectile.Position = attackPacket.AttackPos;
                projectile.velocity = player.Transform.forward.normalized;

                _projectileDic.Add(projectile.Id, projectile);

                projectilePacket.Name = "";
                projectilePacket.ObjectId = projectile.Id;
                projectilePacket.Position = attackPacket.AttackPos;
                
                Broadcast(projectilePacket);

                PushAfter(100, SendProjectileMoveAction, projectile);
            }

            S_AttackPlayer reAttackPacket = new S_AttackPlayer();
            reAttackPacket.ObjectId = player.Id;

            PushAfter(player.CharacterClass.AttackDelay(), player.CharacterClass.ResetCanAttack);

            player.CharacterClass.Attack();

            Broadcast(reAttackPacket);
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

        //public void HandleSkill(Player player, C_Skill skillPacket)
        //{
        //    if (player == null)
        //        return;

        //    ObjectInfo info = player.Info;
        //    if (info.PosInfo.State != CreatureState.Idle)
        //        return;

        //    // TODO : 스킬 사용 가능 여부 체크

        //    info.PosInfo.State = CreatureState.Skill;

        //    S_Skill skill = new S_Skill() { Info = new SkillInfo() };
        //    skill.ObjectId = info.ObjectId;
        //    skill.Info.SkillId = skillPacket.Info.SkillId;
        //    Broadcast(skill);

        //    Data.Skill skillData = null;
        //    if (DataManager.SkillDict.TryGetValue(skillPacket.Info.SkillId, out skillData) == false)
        //        return;

        //    // 데미지 판정
        //    switch (skillData.skillType)
        //    {
        //        case SkillType.SkillAuto:
        //            // 주먹질
        //            Vector2Int skillPos = player.GetFrontCellPos(info.PosInfo.MoveDir);
        //            GameObject target = Map.Find(skillPos);
        //            if (target != null)
        //            {
        //                Console.WriteLine("Hit GameObject !");
        //            }
        //            break;
        //        case SkillType.SkillProjectile:
        //            // 화살
        //            Arrow arrow = ObjectManager.Instance.Add<Arrow>();
        //            if (arrow == null)
        //                return;

        //            arrow.Owner = player;
        //            arrow.Data = skillData;
        //            arrow.PosInfo.State = CreatureState.Moving;
        //            arrow.PosInfo.MoveDir = player.PosInfo.MoveDir;
        //            arrow.PosInfo.PosX = player.PosInfo.PosX;
        //            arrow.PosInfo.PosY = player.PosInfo.PosY;
        //            arrow.Speed = skillData.projectile.speed;
        //            Push(EnterGame, arrow);
        //            break;
        //        default:
        //            break;
        //    }

        //}

        //public Player FindPlayer(Func<GameObject, bool> condition)
        //{
        //    foreach (Player player in _playerDic.Values)
        //    {
        //        if (condition.Invoke(player))
        //            return player;
        //    }

        //    return null;
        //}

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
                    //_enemyDic.Remove(id);
                    break;
                case ObjectTag.TagProjectile:
                    if (_projectileDic.TryGetValue(id, out var projectile))
                    {
                        _projectileDic.Remove(id);
                        projectile = null;
                    }
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
        /// 클라에 주기적으로 보내주는 함수
        /// </summary>
        public void SendProjectileMoveAction(Projectile projectile)
        {
            if (!_projectileDic.ContainsKey(projectile.Id))
            {
                Console.WriteLine("return");
                return;
            }
            Console.WriteLine("Send Client");

            S_MoveProjectile s_Move = new S_MoveProjectile();

            s_Move.ObjectId = projectile.Id;
            s_Move.Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            s_Move.Velocity = projectile.velocity;
            s_Move.Position = projectile.Position;

            Push(Broadcast, s_Move);

            PushAfter(100, SendProjectileMoveAction, projectile);
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
