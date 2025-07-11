using System;
using Google.Protobuf.GameProtocol;

namespace Server.Game.CharacterClass
{
    public static class CharacterClassManager
    {
        public static ICharacterClass Create(JobType type)
        {
            ICharacterClass characterClass = null;
            switch (type)
            {
                case JobType.Pistolmaster:
                    characterClass = new Pistolmaster(10, 10, 1, 500, 30, 30);
                    break;
                case JobType.Machinegunner:
                    characterClass = new Machinegunner(10, 10, 1, 200, 30, 30);
                    break;
                case JobType.Tanker:
                    characterClass = new Tanker(10, 10, 1, 500);
                    break;
                case JobType.Potionist:
                    characterClass = new Pistolmaster(10, 10, 1, 1000, 5, 5);
                    break;
                default:
                    Console.WriteLine("is not Job");
                    break;
            }

            return characterClass;
        }
    }
}
