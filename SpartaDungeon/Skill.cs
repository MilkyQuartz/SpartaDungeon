using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    internal class Skill
    {
        public string Name { get; }
        public string Desc { get; }
        public int Mp { get; }
        public float Damage { get; }

        Random random = new Random();

        public Skill(string name, string desc, int mp, float damage = 0.0f)
        {
            Name = name;
            Desc = desc;
            Mp = mp;
            Damage = damage;
        }


        public void RolltheDice(out int dice1, out int dice2)
        {
            dice1 = random.Next(10, 70) / 10;
            dice2 = random.Next(10, 70) / 10;
            Console.WriteLine($"빨간 주사위 : {dice1}, 파란 주사위 : {dice2}");
        }
        public int UseDiceSkill(int dice1, int dice2, int attackDamage)
        {
            int chance = random.Next(dice1, 11);
            int damage = 0;

            if (chance > 5)
            {
                Console.WriteLine("스킬 발동에 성공했습니다.");
                Console.WriteLine("\"운이 좋군!\"");
                damage = (attackDamage / 2 * dice2);
            }
            else
            {
                Console.WriteLine("스킬 발동에 실패했습니다.");
                Console.WriteLine("\"이럴..수가?!\"");
            }

            return damage;
        }


        public void UseCardSkill(out int cardColor)
        {
            List<Card> card = new List<Card>();

            card.Add(new Card("빨간색 카드", CardType.RED));
            card.Add(new Card("파란색 카드", CardType.BLUE));
            card.Add(new Card("황금색 카드", CardType.GOLD));


            int rand = random.Next(0, 3);
            card[rand].DrawCard();
            cardColor = (int)card[rand].Type;
        }

        public void CardCheck()
        {

        }
    }

    public enum CardType
    {
        RED,
        BLUE,
        GOLD
    }

    public class Card
    {
        public string Name { get; }

        public CardType Type { get; }

        public Card(string name, CardType type)
        {
            Name = name;
            Type = type;
        }


        public void DrawCard()
        {
            int damage = 0;

            switch (Type)
            {
                case CardType.RED:
                    RedCard();
                    break;
                case CardType.BLUE:
                    BlueCard();
                    break;
                case CardType.GOLD:
                    GoldCard();
                    break;
            }

        }

        public void RedCard()
        {
            int damage = 0;

            Console.WriteLine($"{Name}를 뽑았습니다");
            Console.WriteLine($"\"핏빛 빨강\"");

        }

        public void BlueCard()
        {

            Console.WriteLine($"{Name}를 뽑았습니다");
            Console.WriteLine($"\"파랑이 좋겠어\"");

        }

        public void GoldCard()
        {
            Console.WriteLine($"{Name}를 뽑았습니다");
            Console.WriteLine($"\"반짝이는 황금색\"");
        }
    }
}