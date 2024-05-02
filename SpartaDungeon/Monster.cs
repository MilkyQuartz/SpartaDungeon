using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SpartaDungeon
{
    internal class Monster
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