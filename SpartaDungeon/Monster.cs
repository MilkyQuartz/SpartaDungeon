using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace SpartaDungeon
{
    internal class Monster : ICritical, IDamage
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public float Atk { get; set; }
        public float Def { get; set; }
        public float Hp { get; set; }
        public float MaxHp { get; set; }
        public int Price { get; set; }
        public List<MonsterSkill> MonsterSkills { get; set; }

        public Monster(string name, int level, float atk, float def, float hp, float maxHp, int price)
        {
            Name = name;
            Level = level;
            Atk = atk;
            Def = def;
            Hp = hp;
            MaxHp = maxHp;
            Price = price;
            MonsterSkills = new List<MonsterSkill>();
        }

        public void CheckCritical(ref int attackDamage)
        {
            int rand = new Random().Next(1, 21);
            if (rand >= 18)
            {
                attackDamage = (int)(attackDamage * 1.6);
                Console.WriteLine($"급소에 맞았다!");
            }
        }


        public void TakeDamage(int damage)
        {
            int rand = new Random().Next(1, 11);
            if (rand == 10)
            {
                Console.WriteLine($"Lv.{Level} {Name}을(를) 공격했지만 아무일도 일어나지 않았다!");
                ConsoleUtility.PrintTextHighlights("", "\"요호호호~ 그건 제 잔상입니다만?\"", "");
                Console.WriteLine("");
            }
            else
            {
                Hp -= damage;
                Console.WriteLine($"당신은 {Name}에게 {damage}만큼의 피해를 입혔습니다.");
                Console.WriteLine("");
            }
        }
    }

    public class MonsterSkill
    {
        public string MonsterSkillName { get; set; }
        public float MonsterDamage { get; set; }

        public MonsterSkill(string monsterSkillName, float monsterDamage)
        {
            MonsterSkillName = monsterSkillName;
            MonsterDamage = monsterDamage;
        }
    }

}