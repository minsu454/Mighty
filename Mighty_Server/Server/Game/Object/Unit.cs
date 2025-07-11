using Google.Protobuf.GameProtocol;
using Google.Protobuf.UnityProtocol;
using System;

namespace Server.Game.Object
{
    public class Unit : GameObject
    {
        public P_Unit unit = new P_Unit();
        
        public UnitStateType State
        {
            get { return unit.StateType; }
            set { unit.StateType = value; }
        }

        public JobType Job
        {
            get { return unit.Job; }
            set { unit.Job = value; }
        }

        public Unit() : base()
        {
            unit.GameObject = gameObject;
        }

        public P_Vector3 velocity = P_Vector3.zero;

        public override void Update()
        {
        }
    }
}
