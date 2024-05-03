using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpartaDungeon.Casino_Blackjack;

using System.Text.Json;
using System.Xml.Serialization;

namespace SpartaDungeon
{

    internal class Takeout 
    {
        Player player;
        List<Item> barInventory;
        List<Item> inventory;


        public Takeout(Player _player, List<Item> _BarInventory, List<Item> _inventory)
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

            Console.WriteLine("");
            Console.WriteLine(" 1. [카스테라주] 자신의 보유 체력의 50%를 채워준다.(체력 50일때 +25)     - 가격 : 150G");
            Console.WriteLine(" 2. [복분자주] 정읍의 자랑, 100을 기준으로 체력을 50% 채워준다.          - 가격 : 350G");
            Console.WriteLine(" 3. [조니왔다] 유명 위스키, 100을 기준으로 체력을 100% 채워준다.         - 가격 : 550G");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("");

            int keyInput = ConsoleUtility.PromptMenuChoice(0, barInventory.Count);

            switch (keyInput)
            {
                case 0:
                    Menu();
                    break;
                default:
                    //// 1 : 이미 구매한 경우 여긴상관없음
                    //if (barInventory[keyInput - 1].IsPurchased) // index 맞추기
                    //{
                    //    PurchaseMenu("이미 구매한 아이템입니다.");
                    //}
                    // 2 : 돈이 충분해서 살 수 있는 경우
                    if (player.Gold >= barInventory[keyInput - 1].Price)
                    {
                        player.Gold -= barInventory[keyInput - 1].Price;
                        barInventory[keyInput - 1].Purchase();
                        inventory.Add(barInventory[keyInput - 1]);

                        // 
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
            }
        }
    }
}
