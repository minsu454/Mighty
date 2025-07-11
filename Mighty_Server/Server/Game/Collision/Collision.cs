using Google.Protobuf.UnityProtocol;
using Server.Game.Object;
using System;

public static class Collision
{
    public static bool ISObbCollision(P_GameObject obj1, P_GameObject obj2)
    {
        P_Vector3 center = obj1.Transform.Position - obj2.Transform.Position;

        P_Vector3[] axisArr =
        [
            obj1.Transform.right,
            obj1.Transform.forward,
            obj2.Transform.right,
            obj2.Transform.forward,
        ];

        for (int i = 0; i < axisArr.Length; i++)
        {
            float temp = Math.Abs(P_Vector3.Dot(axisArr[i], obj1.Transform.forward * obj1.Transform.Scale.Z * 0.5f))
                + Math.Abs(P_Vector3.Dot(axisArr[i], obj1.Transform.right * obj1.Transform.Scale.X * 0.5f))
                + Math.Abs(P_Vector3.Dot(axisArr[i], obj2.Transform.forward * obj2.Transform.Scale.Z * 0.5f))
                + Math.Abs(P_Vector3.Dot(axisArr[i], obj2.Transform.right * obj2.Transform.Scale.X * 0.5f));

            float distance = Math.Abs(P_Vector3.Dot(center, axisArr[i]));

            if (distance > temp)
            {
                return false;
            }
        }

        return true;
    }
}