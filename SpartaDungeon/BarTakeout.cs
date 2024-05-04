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

    internal class BarTakeout 
    {
        Player player;
        List<UsableItem> barInventory;
        List<Item> playerInventory;
        InventoryManager inventoryManager;

          

        public BarTakeout(Player _player, List<UsableItem> _BarInventory, InventoryManager _inventoryManager)
        {
            player = _player;
            barInventory = _BarInventory;
            inventoryManager = _inventoryManager;
        }

        public void TakeoutMenu(Action Menu)
        {
            playerInventory = inventoryManager.GetInventory(player.Name);
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
            int selectedItem = keyInput - 1;

            switch (keyInput)
            {
                case 0:
                    Menu();
                    break;
                default:
                    if (barInventory[selectedItem].Type == ItemType.USABLE && player.Gold >= barInventory[selectedItem].Price)
                    {                        
                        // 선택한 아이템이 USABLE인지 체크, USABLE이면 인벤토리의 수량을 증가시키는 로직
                        // 1 : 구매선택한 아이템이 인벤토리에 존재하는 경우, 그 아이템의 수량만 늘린다.
                        // 인벤토리를 탐색해서 구매선택한 아이템의 Name이 있는지 알아낸다.
                        // 있다면 그아이템의 인벤토리 인덱스를 얻는다.
                        // 인벤토리[인덱스].Qty를 ++한다.
                        int index = Item.SearchIndexInInventoryAtName(playerInventory, barInventory, keyInput);
                        if (index == -1) // playerInventory에 barInventory[selectedItem]과 같은 아이템이 없을 경우
                        {
                            barInventory[selectedItem].Purchase(player.Name, inventoryManager); // Add할 대상객체에 .Purchase()를 하면 inventoryManager가 player.Name의 인벤토리를 찾아서 Add를 실행한다. 
                            playerInventory = inventoryManager.GetInventory(player.Name); // 가방상태 갱신코드. 이코드를 실행하지 않으면 playerInventory는 함수 맨 위의 처음호출 때의 inventory를 가리키고있고, 바로 윗줄의 Purchase로 인한 변화를 반영하지 못한다.
                            UsableItem temp = (UsableItem)playerInventory[playerInventory.Count -1]; // 방금 Add를 했기때문에 대상아이템이 List의 맨 뒤에 있을 것을 알고있다.                     
                            temp.Qty++;
                        }
                        else
                        {
                            UsableItem temp = (UsableItem)playerInventory[index];
                            temp.Qty++;
                        }
                        player.Gold -= barInventory[selectedItem].Price;              
                    }     
                    // 돈이 모자라는 경우
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

