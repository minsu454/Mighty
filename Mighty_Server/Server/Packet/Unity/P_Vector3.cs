using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Google.Protobuf.UnityProtocol
{
    public partial class P_Vector3
    {

        public P_Vector3(float x, float y)
        {
            this.X = x;
            this.Y = y;
            this.Z = 0;
        }

        public P_Vector3(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        private static readonly P_Vector3 zeroVector = new P_Vector3(0, 0, 0);

        private static readonly P_Vector3 oneVector = new P_Vector3(1, 1, 1);

        private static readonly P_Vector3 upVector = new P_Vector3(0, 1, 0);

        private static readonly P_Vector3 downVector = new P_Vector3(0, -1, 0);

        private static readonly P_Vector3 leftVector = new P_Vector3(-1, 0, 0);

        private static readonly P_Vector3 rightVector = new P_Vector3(1, 0, 0);

        private static readonly P_Vector3 forwardVector = new P_Vector3(0, 0, 1);

        private static readonly P_Vector3 backVector = new P_Vector3(0, 0, -1);

        public static P_Vector3 zero { get { return zeroVector; } }
        public static P_Vector3 one { get { return oneVector; } }
        public static P_Vector3 up { get { return upVector; } }
        public static P_Vector3 down { get { return downVector; } }
        public static P_Vector3 left { get { return leftVector; } }
        public static P_Vector3 right { get { return rightVector; } }
        public static P_Vector3 forward { get { return forwardVector; } }
        public static P_Vector3 back { get { return backVector; } }

        public static float Magnitude(P_Vector3 vector)
        {
            return (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z);
        }

        public P_Vector3 normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return Normalize(this);
            }
        }

        public static P_Vector3 Normalize(P_Vector3 value)
        {
            float num = Magnitude(value);
            if (num > 1E-05f)
            {
                P_Vector3 vector = new P_Vector3();

                vector.X = (float)Math.Round((value.X / num), 2);
                vector.Y = (float)Math.Round((value.Y / num), 2);
                vector.Z = (float)Math.Round((value.Z / num), 2);

                return vector;
            }

            return zero;
        }

        public static float Dot(P_Vector3 lhs, P_Vector3 rhs)
        {
            return lhs.X * rhs.X + lhs.Y * rhs.Y + lhs.Z * rhs.Z;
        }

        public static P_Vector3 Cross(P_Vector3 lhs, P_Vector3 rhs)
        {
            return new P_Vector3(lhs.Y * rhs.Z - lhs.Z * rhs.Y, lhs.Z * rhs.X - lhs.X * rhs.Z, lhs.X * rhs.Y - lhs.Y * rhs.X);
        }

        public static P_Vector3 operator +(P_Vector3 a, P_Vector3 b)
        {
            return new P_Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static P_Vector3 operator -(P_Vector3 a, P_Vector3 b)
        {
            return new P_Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static P_Vector3 operator -(P_Vector3 a)
        {
            return new P_Vector3(0f - a.X, 0f - a.Y, 0f - a.Z);
        }

        public static P_Vector3 operator *(P_Vector3 a, float b)
        {
            return new P_Vector3(a.X * b, a.Y * b, a.Z * b);
        }

        public static P_Vector3 operator *(float b, P_Vector3 a)
        {
            return new P_Vector3(a.X * b, a.Y * b, a.Z * b);
        }

        public static P_Vector3 operator *(P_Vector3 a, P_Vector3 b)
        {
            return new P_Vector3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        }

        public static P_Vector3 operator /(P_Vector3 a, float b)
        {
            return new P_Vector3(a.X / b, a.Y / b, a.Z / b);
        }

        public static P_Vector3 operator *(P_Quaternion rotation, P_Vector3 point)
        {
            float num = rotation.X * 2f;
            float num2 = rotation.Y * 2f;
            float num3 = rotation.Z * 2f;
            float num4 = rotation.X * num;
            float num5 = rotation.Y * num2;
            float num6 = rotation.Z * num3;
            float num7 = rotation.X * num2;
            float num8 = rotation.X * num3;
            float num9 = rotation.Y * num3;
            float num10 = rotation.W * num;
            float num11 = rotation.W * num2;
            float num12 = rotation.W * num3;
            P_Vector3 result = new P_Vector3();
            result.X = (float)Math.Round((1f - (num5 + num6)) * point.X + (num7 - num12) * point.Y + (num8 + num11) * point.Z, 2);
            result.Y = (float)Math.Round((num7 + num12) * point.X + (1f - (num4 + num6)) * point.Y + (num9 - num10) * point.Z, 2);
            result.Z = (float)Math.Round((num8 - num11) * point.X + (num9 + num10) * point.Y + (1f - (num4 + num5)) * point.Z, 2);
            return result;
        }
    }
}
