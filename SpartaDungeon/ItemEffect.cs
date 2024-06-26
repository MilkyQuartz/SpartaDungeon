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
        public static void SoloHeal(Player target, float _percent)
        {
            float hpDecreaseAmount = 20 * _percent;
            float mpIncreaseAmount = 50 * _percent;
            // HP 감소
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("HP : ");
            float newHp = target.Hp - hpDecreaseAmount;
            newHp = Math.Max(newHp, 0);
            Console.WriteLine(target.Hp + " -> " + newHp);
            Console.ResetColor();

            // MP 증가
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("MP : ");
            float newMp = target.Mp + mpIncreaseAmount;
            newMp = Math.Min(newMp, target.MaxMp);
            Console.WriteLine(target.Mp + " -> " + newMp);
            Console.ResetColor();

            target.Hp = newHp;
            target.Mp = newMp;
            Thread.Sleep(200);
        }
        public static void MpHeal(Player target, float _percent)
        {
            float mpIncreaseAmount = 100 * _percent;

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("MP : ");
            float newMp = Math.Min(target.Mp + mpIncreaseAmount, target.MaxMp);
            Console.WriteLine(target.Mp + " -> " + newMp);
            Console.ResetColor();

            target.Mp = newMp;
            Thread.Sleep(200);
        }
        public static void FixedAttacktoOne(Monster target, float _fixedNum)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"{target.Name} HP : ");
            Console.WriteLine(target.Hp.ToString() + " -> " + ((target.Hp - _fixedNum) < 0 ? (target.Hp = 0) : (target.Hp - _fixedNum)).ToString());
            Console.ResetColor();
            target.Hp = (target.Hp - _fixedNum) < 0 ? (target.Hp = 0) : (target.Hp - _fixedNum);
            Thread.Sleep(200);
        }

    }
}
