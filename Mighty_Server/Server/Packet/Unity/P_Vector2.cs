using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Google.Protobuf.UnityProtocol
{
    public partial class P_Vector2
    {

        public P_Vector2(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        private static readonly P_Vector2 zeroVector = new P_Vector2(0, 0);

        private static readonly P_Vector2 oneVector = new P_Vector2(1, 1);

        private static readonly P_Vector2 upVector = new P_Vector2(0, 1);

        private static readonly P_Vector2 downVector = new P_Vector2(0, -1);

        private static readonly P_Vector2 leftVector = new P_Vector2(-1, 0);

        private static readonly P_Vector2 rightVector = new P_Vector2(1, 0);

        public static P_Vector2 zero { get { return zeroVector; } }
        public static P_Vector2 one { get { return oneVector; } }
        public static P_Vector2 up { get { return upVector; } }
        public static P_Vector2 down { get { return downVector; } }
        public static P_Vector2 left { get { return leftVector; } }
        public static P_Vector2 right { get { return rightVector; } }

        public static float Magnitude(P_Vector2 vector)
        {
            return (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        public P_Vector2 normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return Normalize(this);
            }
        }

        public static P_Vector2 Normalize(P_Vector2 value)
        {
            float num = Magnitude(value);
            if (num > 1E-05f)
            {
                P_Vector2 vector = new P_Vector2();

                vector.X = (float)Math.Round((value.X / num), 2);
                vector.Y = (float)Math.Round((value.Y / num), 2);

                return vector;
            }

            return zero;
        }

        public static P_Vector2 operator +(P_Vector2 a, P_Vector2 b)
        {
            return new P_Vector2(a.X + b.X, a.Y + b.Y);
        }

        public static P_Vector2 operator -(P_Vector2 a, P_Vector2 b)
        {
            return new P_Vector2(a.X - b.X, a.Y - b.Y);
        }

        public static P_Vector2 operator -(P_Vector2 a)
        {
            return new P_Vector2(0f - a.X, 0f - a.Y);
        }

        public static P_Vector2 operator *(P_Vector2 a, float b)
        {
            return new P_Vector2(a.X * b, a.Y * b);
        }

        public static P_Vector2 operator *(float b, P_Vector2 a)
        {
            return new P_Vector2(a.X * b, a.Y * b);
        }

        public static P_Vector2 operator *(P_Vector2 a, P_Vector2 b)
        {
            return new P_Vector2(a.X * b.X, a.Y * b.Y);
        }
    
        public static P_Vector2 operator /(P_Vector2 a, float b)
        {
            return new P_Vector2(a.X / b, a.Y / b);
        }
    }
}
