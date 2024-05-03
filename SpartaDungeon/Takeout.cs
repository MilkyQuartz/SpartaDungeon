using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpartaDungeon.Casino_Blackjack;

using System.Text.Json;
using System.Xml.Serialization;
using SpartaDungeon;

namespace SpartaDungeon
{

    internal class Takeout 
    {
        Player player;
        List<UsableItem> barInventory;
        List<Item> inventory;


        public Takeout(Player _player, List<UsableItem> _BarInventory, List<Item> _inventory)
        {
            player = _player;
            barInventory = _BarInventory;
            inventory = _inventory;
        }

        public void TakeoutMenu(Action Menu)
        {
            Action prevMenu = Menu;
            bool failBuy = false;
            Console.Clear();

            ConsoleUtility.ShowTitle("■ 테이크아웃 ■");
            if (failBuy)
            { 
                Console.WriteLine("Gold가 부족한데? 다른걸 골라봐");
                failBuy = !failBuy;
            }
            else
            { Console.WriteLine("내 술을 탐험중에도 마시고 싶다고? 기꺼이 챙겨주지. 포장비는 50G다."); }
            Console.WriteLine("");
            Console.WriteLine($"현재 체력:{player.Hp} | 현재 골드: {player.Gold}");
            Console.WriteLine("");
            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < barInventory.Count; i++)
            {
                barInventory[i].PrintUsableItemDescription(true, i + 1);
            }
            Console.WriteLine(" 0. 나가기");
            Console.WriteLine("");

            int keyInput = ConsoleUtility.PromptMenuChoice(0, barInventory.Count);

            switch (keyInput)
            {
                case 0:
                    Menu();
                    break;
                default:
                    if (barInventory[keyInput - 1].Type == ItemType.USABLE && player.Gold >= barInventory[keyInput - 1].Price)
                    {                        
                        // 선택한 아이템이 USABLE인지 체크
                        // USABLE이면 인벤토리의 수량을 증가시키는 로직
                        // 1 : 구매선택한 아이템이 인벤토리에 존재하는 경우, 그 아이템의 수량만 늘린다.
                        // 인벤토리를 탐색해서 구매선택한 아이템의 Name이 있는지 알아낸다.
                        // 있다면 그아이템의 인벤토리 인덱스를 얻는다.
                        // 인벤토리[인덱스].Qty를 ++한다.
                        int index = Item.SearchIndexInInventoryAtName(inventory, barInventory, keyInput);
                        if (index == -1)
                        {
                            inventory.Add(barInventory[keyInput - 1]);
                            UsableItem temp = (UsableItem)inventory[inventory.Count - 1];
                            temp.Qty++;
                        }
                        else
                        {
                            UsableItem temp = (UsableItem)inventory[index];
                            temp.Qty++;
                        }

                        player.Gold -= barInventory[keyInput - 1].Price;
                        string inventoryJson = JsonSerializer.Serialize(inventory);
                        File.WriteAllText("Inventory.json", inventoryJson);
                        
                    }                    
                    // 2 : 돈이 충분해서 살 수 있는 경우 && USABLE이 아님                    
                    if (barInventory[keyInput - 1].Type != ItemType.USABLE && player.Gold >= barInventory[keyInput - 1].Price)
                    {
                        player.Gold -= barInventory[keyInput - 1].Price;
                        barInventory[keyInput - 1].Purchase();
                        inventory.Add(barInventory[keyInput - 1]);
                                                
                        string inventoryJson = JsonSerializer.Serialize(inventory);
                        File.WriteAllText("Inventory.json", inventoryJson);

                        TakeoutMenu(prevMenu);
                    }
                    // 3 : 돈이 모자라는 경우
                    else
                    {
                        failBuy = !failBuy;
                        TakeoutMenu(prevMenu);
                    }
                    break;                    
            } //END switch
            TakeoutMenu(prevMenu);
        } // END TakeoutMenu()

    }
}

