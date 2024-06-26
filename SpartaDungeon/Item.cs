using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using SpartaDungeon;
using static SpartaDungeon.Casino_Blackjack;

namespace SpartaDungeon
{

    public enum ItemType
    {
        WEAPON,
        ARMOR,
        USABLE,
        HEAL = 20,
        PERCENTHEAL, // Hp 회복
        CASTERA, //  Hp 회복
        MPHEAL, // Mp 회복
        SOLO, // Hp는 깎이고 Mp는 증가하는 타입
        ATTACK = 30,
        ONLYSELL = 40
    }

    internal class Item
    {
        public string Name { get; }
        public string Desc { get; }
        public ItemType Type { get; }

        public int Atk { get; }
        public int Def { get; }
        public int Hp { get; }
        public int Mp { get; }

        public int Price { get; }
        public string PlayerName { get; set; }
        public float Value { get; set; }
        public int Qty { get; set; } //Quantity

        public bool IsEquipped { get; private set; }
        public bool IsPurchased { get; private set; }

        public Item(string name, string desc, ItemType type, int atk, int def, int hp, int mp, int price, float value, int qty, string playerName = null, bool isEquipped = false, bool isPurchased = false)
        {
            Name = name;
            Desc = desc;
            Type = type;
            Atk = atk;
            Def = def;
            Hp = hp;
            Mp = mp;
            Price = price;
            Value = value;
            Qty = qty;
            PlayerName = playerName;
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

        public void PrintStoreItemDescription(InventoryManager inventoryManager, bool withNumber = false, int idx = 0, string playerName = "")
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

            if (inventoryManager.GetInventory(playerName).Any(item => item.Name == this.Name && item.IsPurchased)) // LINQ의 Any메서드
            {
                Console.WriteLine("구매완료");
            }
            else
            {
                ConsoleUtility.PrintTextHighlights("", Price.ToString(), " G");
            }
        }

        public void PrintUsableItemDescription(bool withNumber = false, int idx = 0)
        {
            Console.Write("- ");
            if (withNumber)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write("{0} ", idx);
                Console.ResetColor();
            }
            Console.Write(ConsoleUtility.PadRightForMixedText(Name, 12));
            Console.Write(" | ");
            // 설명 출력
            Console.Write(ConsoleUtility.PadRightForMixedText(Desc, 52));
            Console.Write(" | ");

            ConsoleUtility.PrintTextHighlightsNoLF("", Price.ToString(), " G");

            Console.Write(" | ");
            // 수량 출력
            ConsoleUtility.PrintTextHighlights("보유수 ", Qty.ToString(), "");
        }


        internal void ToggleEquipStatus()
        {
            IsEquipped = !IsEquipped;
        }

        // 구매
        public void Purchase(string playerName, InventoryManager inventoryManager)
        {
            IsPurchased = true;
            PlayerName = playerName;
            inventoryManager.AddItem(playerName, this);
        }

        // 판매
        public void Refund(string playerName, InventoryManager inventoryManager)
        {
            IsPurchased = false;
            PlayerName = null;
            inventoryManager.RemoveItem(playerName, this); 
        }

        // 호출예시 Item.SearchIndexInInventoryAtName(targetList, searchList, keyInput)
        public static int SearchIndexInInventoryAtName(List<Item> targetList, List<Item> searchList, int keyInput)
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

   
    //internal class UsableItem : Item
    //{
    //    public float Value { get; set; }
    //    public int Qty { get; set; } //Quantity


    //    public UsableItem(string name, string desc, ItemType type, int atk, int def, int hp, int mp, int price, float value, int qty, string playerName = null, bool isEquipped = false, bool isPurchased = false) : base(name, desc, type, atk, def, hp, mp, price, playerName = null, isEquipped = false, isPurchased = false)
    //    {
    //        Value = value;
    //        Qty = qty;
    //    }

        
    //    public void PrintUsableItemDescription(bool withNumber = false, int idx = 0)
    //    {
    //        Console.Write("- ");
    //        if (withNumber)
    //        {
    //            Console.ForegroundColor = ConsoleColor.DarkMagenta;
    //            Console.Write("{0} ", idx);
    //            Console.ResetColor();
    //        }
    //        Console.Write(ConsoleUtility.PadRightForMixedText(Name, 12));
    //        Console.Write(" | ");
    //        // 설명 출력
    //        Console.Write(ConsoleUtility.PadRightForMixedText(Desc, 52));
    //        Console.Write(" | ");

    //        ConsoleUtility.PrintTextHighlightsNoLF("", Price.ToString(), " G");

    //        Console.Write(" | ");
    //        // 수량 출력
    //        ConsoleUtility.PrintTextHighlights("보유수 ", Qty.ToString(), "");
    //    }
    //}

    
    internal class InventoryManager
    {
        




        private Dictionary<string, List<Item>> inventory;
        public InventoryManager()
        {
            inventory = LoadInventory();

        }

        // player의 inventory에 변화를 준 코드 뒤엔 바로 GetInventory()를 호출해서 inventory를 갱신하세요.
        // 갱신없이 player의 inventory에 접근하면 변화가 있기 전의 inventory를 가져옵니다.
        public List<Item> GetInventory(string playerName)
        {
            if (inventory.ContainsKey(playerName))
                return inventory[playerName];
            else
                return new List<Item>();
        }
                


        private Dictionary<string, List<Item>> LoadInventory()
        {
            Dictionary<string, List<Item>> playerInventory = new Dictionary<string, List<Item>>();

            if (File.Exists("Inventory.json"))
            {
                // 파일 크기 확인
                long fileSize = new FileInfo("Inventory.json").Length;

                if (fileSize > 0)
                {
                    try
                    {
                        string json = File.ReadAllText("Inventory.json");
                        playerInventory = JsonSerializer.Deserialize<Dictionary<string, List<Item>>>(json);
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Error loading inventory: {ex.Message}");
                    }
                }
            }

            return playerInventory;
        }

        public void SaveInventory()
        {
            string jsonInventory = JsonSerializer.Serialize(inventory);
            File.WriteAllText("Inventory.json", jsonInventory);
        }

        public void AddItem(string playerName, Item item)
        {
            if (!inventory.ContainsKey(playerName))
            {
                inventory[playerName] = new List<Item>();
            }

            inventory[playerName].Add(item);
            SaveInventory();
        }

        public void RemoveItem(string playerName, Item item)
        {
            if (inventory.ContainsKey(playerName))
            {
                inventory[playerName].Remove(item);
                SaveInventory();
            }
        }
    }
}