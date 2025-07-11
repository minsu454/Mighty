using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Room
{
    public interface IRoom
    {
        public int RoomId { get; set; }
        public void Update();
    }
}
