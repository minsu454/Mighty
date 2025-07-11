using Google.Protobuf.GameProtocol;
using Google.Protobuf.UnityProtocol;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Room
{
    public class R_GameManager
    {
        private object _lock = new object();
        private Dictionary<int, R_Game> _roomDic = new Dictionary<int, R_Game>();
        private int _counter = 0;

        public void Add(out R_Game r_Game, int maxPlayerCount)
        {
            R_Game gameRoom = new R_Game(maxPlayerCount);

            lock (_lock)
            {
                gameRoom.RoomId = GenerateId(RoomType.RField);
                _roomDic.Add(gameRoom.RoomId, gameRoom);
                _counter++;

                r_Game = gameRoom;
            }
        }

        public bool ReMove(int roomId)
        {
            lock (_lock)
            {
                return _roomDic.Remove(roomId);
            }
        }

        public R_Game Find(int roomId)
        {
            lock (_lock)
            {
                R_Game room = null;
                _roomDic.TryGetValue(roomId, out room);

                return room;
            }
        }

        private int GenerateId(RoomType type)
        {
            lock (_lock)
            {
                return ((int)type << 24) | (_counter++);
            }
        }

        public static RoomType GetObjectTypeById(int id)
        {
            int type = (id >> 24) & 0x7F;
            return (RoomType)type;
        }
    }
}
