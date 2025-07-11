using Google.Protobuf.UnityProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Object
{
    public class Wall : GameObject
    {
        public Wall()
        {
            Transform = new P_Transform(P_Vector3.one, P_Quaternion.zero, P_Vector3.one);
        }

        public Wall(P_Transform transform)
        {
            Transform = transform;
        }

        public Wall(P_Vector3 position, P_Quaternion rotation, P_Vector3 scale)
        {
            Transform = new P_Transform(position, rotation, scale);
        }
    }
}
