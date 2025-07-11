using System;
using System.Numerics;

namespace Google.Protobuf.UnityProtocol
{
    public partial class P_Transform
    {
        public P_Transform(P_Vector3 position, P_Quaternion rotation, P_Vector3 scale)
        {
            this.Position = position;
            this.Rotation = rotation;
            this.Scale = scale;
        }

        public P_Vector3 up
        {
            get
            {
                return Rotation * P_Vector3.up;
            }
            set
            {
                Rotation = P_Quaternion.FromToRotation(P_Vector3.up, value);
            }
        }

        public P_Vector3 right
        {
            get
            {
                return Rotation * P_Vector3.right;
            }
            set
            {
                Rotation = P_Quaternion.FromToRotation(P_Vector3.right, value);
            }
        }

        public P_Vector3 forward
        {
            get
            {
                return Rotation * P_Vector3.forward;
            }
            set
            {
                Rotation = P_Quaternion.FromToRotation(P_Vector3.forward, value);
            }
        }
    }
}
