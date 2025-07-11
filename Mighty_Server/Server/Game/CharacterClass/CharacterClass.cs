using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.CharacterClass
{
    public class CharacterClass : ICharacterClass
    {
        protected int damage;           //데미지 저장 변수

        protected int curHp;            //현재 hp 저장 변수
        protected int maxHp;            //최대 hp 저장 변수

        protected int attackDelay;      //공격 쿨타임 저장 변수
        protected bool canAttack;       //공격 가능한지 알려주는 변수

        public CharacterClass(int curHp, int maxHp, int damage, int attackDelay)
        {
            this.curHp = curHp;
            this.maxHp = maxHp;
            this.damage = damage;
            this.attackDelay = attackDelay;

            ResetCanAttack();
        }

        /// <summary>
        /// 공격
        /// </summary>
        public virtual void Attack() { canAttack = false; }

        public int AttackDelay() { return attackDelay; }

        /// <summary>
        /// 공격 가능여부를 체크해주는 함수
        /// </summary>
        public virtual bool IsCanAttack() { return canAttack; }

        public virtual bool IsUseProjectile() { return false; }

        public virtual void Reload() {}

        public void ResetCanAttack()
        {
            canAttack = true;
        }
    }
}
