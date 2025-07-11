using Google.Protobuf.UnityProtocol;
using Server.Game.Room;

namespace Server.Game.Object
{
    public class GameObject
    {
        public R_Game Room { get; set; }

        public P_GameObject gameObject = new P_GameObject();

        public int Id
        {
            get { return gameObject.Id; }
            set { gameObject.Id = value; }
        }

        public ObjectTag Tag
        {
            get { return gameObject.Tag; }
            protected set { gameObject.Tag = value; }
        }

        public string NickName
        {
            get { return gameObject.Name; }
            set { gameObject.Name = value; }
        }

        public P_Transform Transform
        {
            get { return gameObject.Transform; }
            set { gameObject.Transform = value; }
        }

        public P_Vector3 Position
        {
            get { return Transform.Position; }
            set { Transform.Position = value; } 
        }

        public P_Quaternion Rotation
        {
            get { return Transform.Rotation; }
            set { Transform.Rotation = value; }
        }

        public P_Vector3 Scale
        {
            get { return Transform.Scale; }
            set { Transform.Scale = value; }
        }

        public GameObject()
        {
            Transform = new P_Transform(P_Vector3.zero, P_Quaternion.zero, P_Vector3.one);
        }

        public GameObject(P_Transform transform)
        {
            Transform = transform;
        }

        public GameObject(P_Vector3 position, P_Quaternion rotation, P_Vector3 scale)
        {
            Transform = new P_Transform(position, rotation, scale);
        }

        public virtual void Update()
        {
        }
    }
}
