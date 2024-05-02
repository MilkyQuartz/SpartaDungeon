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
        public int Damage { get; }

        Random random = new Random();

        public Skill(string name, string desc, int damage)
        {
            Name = name;
            Desc = desc;
            Damage = damage;
        }

        public void RolltheDice(out int dice1, out int dice2)
        {
            dice1 = random.Next(10, 70) / 10;
            dice2 = random.Next(10, 70) / 10;
            Console.WriteLine($"주사위 1 : {dice1}, 주사위 2 : {dice2}");
        }
        public void UseDiceSkill(int dice1, int dice2)
        {
            int chance = random.Next(dice1, 11);

            if (chance > 5)
            {
                Console.WriteLine($"{Name} 스킬사용 성공");
                int damage = Damage / 2 * dice2;
                Console.WriteLine("데미지 = (공격력 / 2) X 주사위 눈");
                Console.WriteLine($"데미지 : {damage}");
            }
            else
            {
                Console.WriteLine($"{Name} 스킬사용 실패");
            }
        }

        public void UseCardSkill()
        {
            List<Card> card = new List<Card>();
            card.Add(new Card("빨간 카드", "빨간색 카드", CardType.RED));
            card.Add(new Card("파란 카드", "파란색 카드", CardType.BLUE));
            card.Add(new Card("황금 카드", "황금색 카드", CardType.GOLD));

            int rand = random.Next(0, 3);
            card[rand].DrawCard();
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
        public string Desc { get; }

        public CardType Type { get; }

        public Card(string name, string desc, CardType type)
        {
            Name = name;
            Desc = desc;
            Type = type;
        }


        public void DrawCard()
        {
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
            Console.WriteLine($"{Name}를 뽑았습니다");
            Console.WriteLine($"\"핏빛 빨강\"");
            Console.WriteLine("2연속베기");
        }

        public void BlueCard()
        {
            Console.WriteLine($"{Name}를 뽑았습니다");
            Console.WriteLine($"\"파랑이 좋겠어\"");
            Console.WriteLine("마나 회복");
        }

        public void GoldCard()
        {
            Console.WriteLine($"{Name}를 뽑았습니다");
            Console.WriteLine($"\"반짝이는 황금색\"");
            Console.WriteLine("강력한 일격,  마나 전부 소진");
        }
    }
}
