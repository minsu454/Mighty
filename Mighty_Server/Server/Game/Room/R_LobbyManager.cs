using Google.Protobuf.GameProtocol;
using Server.Game.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Room
{
    public class R_LobbyManager
    {
        private object _lock = new object();
        private Dictionary<int, R_Lobby> _roomDic = new Dictionary<int, R_Lobby>();
        private int _counter = 0;


        public R_Lobby Add()
        {
            R_Lobby lobbyRoom = new R_Lobby();

            lock (_lock)
            {
                lobbyRoom.RoomId = _counter;
                _roomDic.Add(_counter, lobbyRoom);
                _counter++;
            }

            return lobbyRoom;
        }

        public bool ReMove(int roomId)
        {
            lock (_lock)
            {
                return _roomDic.Remove(roomId);
            }
        }

        public R_Lobby Find(int roomId)
        {
            lock (_lock)
            {
                R_Lobby room = null;

                _roomDic.TryGetValue(roomId, out room);
                return room;
            }
        }
    }
}
