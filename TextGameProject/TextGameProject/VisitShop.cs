using System;
using System.Collections.Generic;

namespace TextGameProject
{
    internal class VisitShop
    {
        public static void DisplayShopItems(Player player, FileManager fileManager)
        {
            var shopItems = fileManager.LoadShopItems();

            Console.WriteLine("  ===========================================================================================");
            Console.WriteLine("  |                                       상    점                                          |");
            Console.WriteLine("  ===========================================================================================");
            Console.WriteLine("* [보유 골드]");
            Console.WriteLine($"* {player.gold} G");
            Console.WriteLine(" [아이템 목록]\n");

            foreach (var item in shopItems)
            {
                string priceInfo = item.isPurchased ? "구매완료" : $"{item.gold} G";
                Console.WriteLine($"* {item.itemName}: 체력 +{item.ability.health}, 공격력 +{item.ability.attack}, 방어력 +{item.ability.defense}, {(item.isPurchased ? "구매완료" : "가격 " + priceInfo)}");
            }

            Console.WriteLine("\t\t\t\t\t1. 아이템 구매");
            Console.WriteLine("\t\t\t\t\t0. 나가기");
            Console.Write(">> ");

            bool shopping = true;

            while (shopping)
            {
                int input = int.Parse(Console.ReadLine());

                if (input == 1)
                {
                    BuyItem(player, fileManager, shopItems);
                }
                else if (input == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\t\t\t\t[로비로 이동합니다.]");
                    Console.ResetColor();
                    Program.EnterLobby(player, fileManager);
                    shopping = false; 
                }
                else
                {
                    Console.WriteLine("\t\t\t\t\t[올바른 옵션을 선택하세요.]");
                }
            }
        }

        private static void BuyItem(Player player, FileManager fileManager, List<ShopItemInfo> shopItems)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n\"아이고 손님. 구매하시렵니까!! 스파르타 마을에서 제일가는......\"");
            Console.WriteLine("(심드렁하던 주인장이 시끄럽게 떠들기시작한다.)\n");
            Console.ResetColor();
            int num = 1;

            foreach (var item in shopItems)
            {
                if (!item.isPurchased)
                {
                    Console.WriteLine($"{num}.{item.itemName} : 가격: {item.gold} G");
                }
                else
                {
                    Console.WriteLine($"{num}.{item.itemName} : 구매완료");
                }
                num++;
            }
            Console.WriteLine("\n\t\t\t\t구매할 아이템 번호를 입력하세요.");
            Console.WriteLine("\t\t\t\t\t0. 나가기");
            Console.Write(">> ");

            int input2 = int.Parse(Console.ReadLine());

            if (input2 > 0 && input2 <= shopItems.Count)
            {
                int index = input2 - 1;
                ShopItemInfo shopItem = shopItems[index];
                Random random = new Random();
                int rad = random.Next(0, 4);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n\"아이고 이 {shopItem.itemName}은 말입죠..! \"");
                switch (rad)
                {
                    case 0:
                        Console.WriteLine($"\"{shopItem.information}..없어서 못사요!\"");
                        break;
                    case 1:
                        Console.WriteLine($"\"{shopItem.information} 진짜 귀한 물건이에요!\"");
                        break;
                    case 2:
                        Console.WriteLine($"\"{shopItem.information}.. 이거 금방금방 나가서 언제 들어올지 몰라요!\"");
                        break;
                    case 3:
                        Console.WriteLine($"\"{shopItem.information}.. 내가 {player.playerName}용사님께만 특별히 싸게해드릴게!\"");
                        break;
                }
                Console.ResetColor();
                Console.WriteLine("\n1. 아이템 구매");
                Console.WriteLine("\n2. 다시선택");
                int input3 = int.Parse(Console.ReadLine());

                if (input3 == 1)
                {
                    if (!shopItem.isPurchased && player.gold >= shopItem.gold)
                    {
                        shopItem.isPurchased = true;
                        fileManager.UpdateShop(shopItem.itemName, true);

                        Console.WriteLine($"{shopItem.itemName}을(를) 구매하셨습니다.");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n\"감사합니다 흑ㅇ..아니.. 손님! 다음에도 또 방문해주세요!\"\n");
                        Console.ResetColor();
                        player.gold -= shopItem.gold;

                        fileManager.SavePlayerData(player);

                        player.AddItemToInventory(shopItem);

                        Console.WriteLine("1. 확인");
                        Console.WriteLine("0. 나가기");
                        Console.Write(">> ");
                        int nextAction = int.Parse(Console.ReadLine());

                        if (nextAction == 1)
                        {
                            DisplayShopItems(player, fileManager);
                        }
                        else if (nextAction == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\t\t\t\t[로비로 이동합니다.]");
                            Console.ResetColor();
                            Program.EnterLobby(player, fileManager);
                        }
                    }
                    else if (shopItem.isPurchased)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n\"이미 구매한 아이템입니다!\"");
                        Console.ResetColor();
                        Console.WriteLine("1. 확인");
                        Console.Write(">> ");
                        int nextAction = int.Parse(Console.ReadLine());

                        if (nextAction == 1)
                        {
                            DisplayShopItems(player, fileManager);
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n\"에이 손님.. 돈도 없으면서.. 나가요 나가!\"");
                        Console.WriteLine("(쫒겨났다.)\n");
                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\t\t\t\t    [로비로 이동합니다.]");
                        Console.ResetColor();
                        Program.EnterLobby(player, fileManager);
                    }
                }
                else if (input3 == 2)
                {
                    DisplayShopItems(player, fileManager);
                }
                else
                {
                    Console.WriteLine("\t\t\t\t\t[올바른 옵션을 선택하세요.]");
                }
            }
            else if (input2 == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t\t\t\t[로비로 이동합니다.]");
                Console.ResetColor();
                Program.EnterLobby(player, fileManager);
            }
        }
    }
}