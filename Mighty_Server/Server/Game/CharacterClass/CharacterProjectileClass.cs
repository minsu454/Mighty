using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.CharacterClass
{
    public class CharacterProjectileClass : CharacterClass
    {
        protected int curAmmo;             //현재 총알 갯수
        protected int maxAmmo;             //최대 총알 갯수
        protected bool isInfiniteAmmo = false;

        public CharacterProjectileClass(int curHp, int maxHp, int damage, int attackDelay, int curAmmo, int maxAmmo)
            : base(curHp, maxHp, damage, attackDelay)
        {
            this.curAmmo = curAmmo;
            this.maxAmmo = maxAmmo;
        }

        /// <summary>
        /// 공격
        /// </summary>
        public override void Attack()
        {
            base.Attack();

            if (isInfiniteAmmo == false)
            {
                Console.WriteLine($"남은 총알 수 : {curAmmo}");
                curAmmo--;
            }
        }

        /// <summary>
        /// 공격 가능여부를 체크해주는 함수
        /// </summary>
        public override bool IsCanAttack()
        {
            if (!canAttack)
            {
                return false;
            }

            if (curAmmo > 0)
            {
                return true;
            }

            return false;
        }

        public override void Reload()
        {
            curAmmo = maxAmmo;
        }

        public override bool IsUseProjectile()
        {
            return true;
        }
    }
}
