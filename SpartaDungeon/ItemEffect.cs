using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpartaDungeon.Casino_Blackjack;

namespace SpartaDungeon
{
    internal class ItemEffect
    {


        // 호출예시 : 배틀 -> 아이템사용하기 -> 아이템선택 선택한 아이템.UseHealItem(Value);
        void UseItem(Object target, float value)
        {
        }
        public static void Castera(Player target, float _percent)
        {
            Console.WriteLine("\n\t\t\t  \"키야~ 역시 한국인이라면 이 맥주를 마셔줘야지!\"\n");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("HP : ");
            Console.WriteLine(target.Hp.ToString() + " -> " + ((target.Hp + target.Hp * _percent) > target.MaxHp ? (target.Hp = target.MaxHp) : (target.Hp + target.Hp * _percent)).ToString());
            Console.ResetColor();
            target.Hp = (target.Hp + target.Hp * _percent) > target.MaxHp ? (target.Hp = target.MaxHp) : (target.Hp + target.Hp * _percent);
            Thread.Sleep(200);
        }
        public static void FixedHeal(Player target, float _fixedNum)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("HP : ");
            Console.WriteLine(target.Hp.ToString() + " -> " + ((target.Hp + _fixedNum) > target.MaxHp ? (target.Hp = target.MaxHp) : (target.Hp + _fixedNum)).ToString());
            Console.ResetColor();
            target.Hp = (target.Hp + _fixedNum) > target.MaxHp ? (target.Hp = target.MaxHp) : (target.Hp + _fixedNum);
            Thread.Sleep(200);
        }
        public static void PercentHeal(Player target, float _percent)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("HP : ");
            Console.WriteLine(target.Hp.ToString() + " -> " + ((target.Hp + target.MaxHp*_percent) > target.MaxHp ? (target.Hp = target.MaxHp) : (target.Hp + target.MaxHp * _percent)).ToString());
            Console.ResetColor();
            target.Hp = (target.Hp + target.MaxHp * _percent) > target.MaxHp ? (target.Hp = target.MaxHp) : (target.Hp + target.MaxHp * _percent);
            Thread.Sleep(200);
        }


    }
}
