using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Room
{
    public class RoomManagers
    {
        public static R_GameManager R_Game { get; } = new R_GameManager();
        public static R_LobbyManager R_Lobby { get; } = new R_LobbyManager();
    }
}
