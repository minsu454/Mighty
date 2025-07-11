using Google.Protobuf.Protocol;
using Google.Protobuf.UnityProtocol;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.Object
{
    public class Projectile : GameObject
    {
        float speed = 15f;

        public Projectile()
        {
            Tag = ObjectTag.TagProjectile;
        }

        public P_Vector3 velocity = P_Vector3.zero;

        public override void Update()
        {
            P_Vector3 result = new P_Vector3(velocity * speed * 0.1f);

            result.X = (float)Math.Round(result.X, 3);
            result.Y = (float)Math.Round(result.Y, 3);
            result.Z = (float)Math.Round(result.Z, 3);

            Position += result;

            Console.WriteLine(Position);
        }
    }
}
