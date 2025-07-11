using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Google.Protobuf.UnityProtocol
{
    public partial class P_Quaternion
    {

        public P_Quaternion(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = 0;
        }

        public P_Quaternion(float x, float y, float z, float w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        private static readonly P_Quaternion zeroQuaternion = new P_Quaternion(0, 0, 0, 0);

        private static readonly P_Quaternion oneQuaternion = new P_Quaternion(1, 1, 1, 1);

        public static P_Quaternion zero { get { return zeroQuaternion; } }
        public static P_Quaternion one { get { return oneQuaternion; } }

        public static P_Quaternion FromToRotation(P_Vector3 fromDirection, P_Vector3 toDirection)
        {
            fromDirection = P_Vector3.Normalize(fromDirection);
            toDirection = P_Vector3.Normalize(toDirection);

            float dot = P_Vector3.Dot(fromDirection, toDirection);
            float angle = MathF.Acos(dot);
            P_Vector3 axis = P_Vector3.Cross(fromDirection, toDirection);

            if (P_Vector3.Magnitude(axis) == 0)
            {
                // from과 to가 반대 방향인 경우
                axis = P_Vector3.Cross(fromDirection, P_Vector3.right);
                if (P_Vector3.Magnitude(axis) == 0)
                {
                    axis = P_Vector3.Cross(fromDirection, P_Vector3.up);
                }
                axis = P_Vector3.Normalize(axis);
                return CreateFromAxisAngle(axis, MathF.PI);
            }
            CreateFromAxisAngle(axis, MathF.PI);
            axis = P_Vector3.Normalize(axis);
            return CreateFromAxisAngle(axis, angle);
        }

        private static P_Quaternion CreateFromAxisAngle(P_Vector3 axis, float angle)
        {
            axis = P_Vector3.Normalize(axis);
            float halfAngle = angle / 2;
            float sinHalfAngle = MathF.Sin(halfAngle);

            float x = axis.X * sinHalfAngle;
            float y = axis.Y * sinHalfAngle;
            float z = axis.Z * sinHalfAngle;
            float w = MathF.Cos(halfAngle);

            return new P_Quaternion(x, y, z, w);
        }
    }
}
