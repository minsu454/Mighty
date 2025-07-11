
namespace Server.Game.CharacterClass
{
    public class Machinegunner : CharacterProjectileClass
    {
        public Machinegunner(int curHp, int maxHp, int damage, int attackDelay, int curAmmo, int maxAmmo)
            : base(curHp, maxHp, damage, attackDelay, curAmmo, maxAmmo) {}
    }
}
