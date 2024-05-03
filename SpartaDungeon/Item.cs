using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpartaDungeon.Casino_Blackjack;


namespace SpartaDungeon
{   

    public enum ItemType
    {
        WEAPON,
        ARMOR,
        USABLE
    }

    internal class Item
    {
        public string Name { get; }
        public string Desc { get; }

        public ItemType Type { get; }

        public int Atk { get; }
        public int Def { get; }
        public int Hp { get; }

        public int Price { get; }

        public bool IsEquipped { get; private set; }
        public bool IsPurchased { get; private set; }

        //public Item()
        //{ }
        public Item(string name, string desc, ItemType type, int atk, int def, int hp, int price, bool isEquipped = false, bool isPurchased = false)
        {
            Name = name;
            Desc = desc;
            Type = type;
            Atk = atk;
            Def = def;
            Hp = hp;
            Price = price;
            IsEquipped = isEquipped;
            IsPurchased = isPurchased;
        }

        // 아이템 정보를 보여줄때 타입이 비슷한게 2가지있음.
        // 1. 인벤토리에서 그냥 내가 어느 아이템 가지고 있는지 보여줄 때
        // 2. 장착관리에서 내가 어떤 아이템을 낄지 말지 결정할 때
        internal void PrintItemStatDescription(bool withNumber = false, int idx = 0)
        {
            Console.Write("- ");
            if (withNumber)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write($"{idx} ");
                Console.ResetColor();
            }
            if (IsEquipped)
            {
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("E");
                Console.ResetColor();
                Console.Write("]");
                Console.Write(ConsoleUtility.PadRightForMixedText(Name, 9));
            }
            else Console.Write(ConsoleUtility.PadRightForMixedText(Name, 12));

            Console.Write(" | ");

            if (Atk != 0) Console.Write($"공격력 {(Atk >= 0 ? "+" : "")}{Atk} ");
            if (Def != 0) Console.Write($"방어력 {(Atk >= 0 ? "+" : "")}{Def} ");
            if (Hp != 0) Console.Write($"체  력 {(Atk >= 0 ? "+" : "")}{Hp} ");

            Console.Write(" | ");

            Console.WriteLine(Desc);

        }

        public void PrintStoreItemDescription(bool withNumber = false, int idx = 0) 
        {
            Console.Write("- ");
            // 장착관리 전용
            if (withNumber) //인덱스번호 출력유무
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write("{0} ", idx);
                Console.ResetColor();
            }
            Console.Write(ConsoleUtility.PadRightForMixedText(Name, 12));  

            Console.Write(" | ");

            if (Atk != 0) Console.Write($"공격력 {(Atk >= 0 ? "+" : "")}{Atk} ");
            if (Def != 0) Console.Write($"방어력 {(Def >= 0 ? "+" : "")}{Def} ");
            if (Hp != 0) Console.Write($"체  력 {(Hp >= 0 ? "+" : "")}{Hp}");

            Console.Write(" | ");

            Console.Write(ConsoleUtility.PadRightForMixedText(Desc, 18));

            Console.Write(" | ");

            if (IsPurchased)
            {
                Console.WriteLine("구매완료");
            }
            else
            {
                ConsoleUtility.PrintTextHighlights("", Price.ToString(), " G");
            }
        }

        

        internal void ToggleEquipStatus()
        {
            IsEquipped = !IsEquipped;
        }

        internal void Purchase()
        {
            IsPurchased = true;
        }
        internal void Refund()
        {
            IsPurchased = false;
        }
        // 호출예시 Item.SearchIndexInInventoryAtName(targetList, searchList, keyInput)
        public static int SearchIndexInInventoryAtName(List<Item> targetList, List<UsableItem> searchList, int keyInput)
        {
            if (keyInput < 1 || keyInput > searchList.Count)
            {
                // keyInput이 searchList의 인덱스 범위 밖에 있을 경우 처리
                return -1; // 범위를 벗어나면 기본값으로 -1 반환
            }
            for (int i = 0; i < targetList.Count; ++i)
            {
                if (targetList[i].Name == searchList[keyInput - 1].Name)
                {
                    return i;
                }
            }
            return -1;
        }
    }

    internal class UsableItem : Item
    {
        public float Value { get; set; }
        public int Qty { get; set; } //Quantity
        

        public UsableItem(string name, string desc, ItemType type, int atk, int def, int hp, int price, float value,int qty, bool isEquipped = false, bool isPurchased = false) : base(name, desc, type, atk, def, hp, price, isEquipped = false, isPurchased = false)
        {
            Value = value;
            Qty = qty;
        }

        // 호출예시 : 배틀 -> 아이템사용하기 -> 아이템선택 선택한 아이템.UseHealItem(Value);
        void UseItem(Object target, float value)
        {
            

            
        }
        //void Castera()
        //{
        //    player.Hp = (player.Hp + player.Hp * 0.5) >= player.MaxHp ? (player.Hp = player.MaxHp) : (player.Hp + player.Hp * 0.5);
        //}

        //void FixedHeal(float _hp)
        //{
        //    player.Hp = (player.Hp + _hp) >= player.MaxHp ? (player.Hp = player.MaxHp) : (player.Hp + _hp);
        //}

        //void PercentHeal(float _percent)
        //{
        //    player.Hp = (player.Hp + player.MaxHp * _percent) >= player.MaxHp ? (player.Hp = player.MaxHp) : (player.Hp + player.MaxHp * _percent);
        //}
        public void PrintUsableItemDescription(bool withNumber = false, int idx = 0)
        {
            Console.Write("- ");
            // 장착관리 전용
            if (withNumber)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write("{0} ", idx);
                Console.ResetColor();
            }
            Console.Write(ConsoleUtility.PadRightForMixedText(Name, 12));

            Console.Write(" | ");
            // 설명 출력
            Console.Write(ConsoleUtility.PadRightForMixedText(Desc, 20));

            Console.Write(" | ");

            ConsoleUtility.PrintTextHighlightsNoLF("", Price.ToString(), " G");

            Console.Write(" | ");
            // 수량 출력
            ConsoleUtility.PrintTextHighlights("보유수 ", Qty.ToString(), "");
        }

        

    }

}