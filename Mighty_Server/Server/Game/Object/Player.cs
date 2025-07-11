using Google.Protobuf.UnityProtocol;
using Google.Protobuf.GameProtocol;
using Google.Protobuf.Protocol;
using Server.Game.CharacterClass;

namespace Server.Game.Object
{
    public class Player : Unit
    {
        public ClientSession Session { get; set; }
        public ICharacterClass CharacterClass { get; set; }

        public Player()
        {
            Tag = ObjectTag.TagPlayer;

            State = UnitStateType.StateIdle;
        }
    }
}