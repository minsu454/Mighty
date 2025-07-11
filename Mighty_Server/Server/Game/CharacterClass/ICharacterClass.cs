using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Game.CharacterClass
{
    public interface ICharacterClass
    {
        public void Attack();           //공격함수
        public bool IsCanAttack();      //공격가능 여부 함수
        public void ResetCanAttack();   //공격 가능으로 리셋 함수

        public bool IsUseProjectile();  //투사체를 사용하는지 여부를 반환해주는 함수
        public void Reload();           //재장전 함수

        public int AttackDelay();       //공격 딜레이
    }
}
