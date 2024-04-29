using System;

namespace TextGameProject
{
    internal class Program
    {
        static string playerFilePath = "player_info.txt";
        static string inventoryFilePath = "inventory.txt";
        static string shopItemsFilePath = "shop_items.txt";

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"
     _______  _______  __   __  _______  _______  _______  _______  ______    _______  __  
    |       ||   _   ||  |_|  ||       ||       ||       ||   _   ||    _ |  |       ||  | 
    |    ___||  |_|  ||       ||    ___||  _____||_     _||  |_|  ||   | ||  |_     _||  | 
    |   | __ |       ||       ||   |___ | |_____   |   |  |       ||   |_||_   |   |  |  | 
    |   ||  ||       ||       ||    ___||_____  |  |   |  |       ||    __  |  |   |  |__| 
    |   |_| ||   _   || ||_|| ||   |___  _____| |  |   |  |   _   ||   |  | |  |   |   __  
    |_______||__| |__||_|   |_||_______||_______|  |___|  |__| |__||___|  |_|  |___|  |__| 
                                               
");
            Console.ResetColor();

            Console.WriteLine("\t\t\t\t\t1. 새로하기");
            Console.WriteLine("\t\t\t\t\t2. 불러오기");
            Console.Write(">> ");
            int input = int.Parse(Console.ReadLine());

            FileManager playerFileManager = new FileManager(playerFilePath, inventoryFilePath, shopItemsFilePath);

            if (input == 1)
            {
                if (File.Exists(playerFilePath))
                {
                    File.WriteAllText(playerFilePath, string.Empty);
                }

                if (File.Exists(inventoryFilePath))
                {
                    File.WriteAllText(inventoryFilePath, string.Empty);
                }
                Console.WriteLine("\t\t\t  플레이 시 사용하실 이름을 정해주세요. 용사님! ");
                Player player = new Player(playerFileManager); 
                string playerName;
                while (true)
                {
                    Console.Write(">> ");
                    player.playerName = Console.ReadLine();
                    if (player.playerName.Length != 3)
                    {
                        Console.WriteLine("\t\t\t    3글자 이름으로 부탁드릴게요. 용사님! ");
                    }
                    else break;
                }

                player.job = "전사";
                player.level = 1;
                player.attack = 10;
                player.defense = 5;
                player.health = 100;
                player.gold = 1500;
                player.clearCount = 0;
                playerFileManager.SavePlayerData(player);

                EnterLobby(player, playerFileManager);
            }
            else if (input == 2)
            {
                if (File.Exists(playerFilePath))
                {
                    Player player = playerFileManager.ReadPlayer(playerFilePath, playerFileManager);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\t\t\t\t[저장된 플레이어 정보를 불러옵니다.]");
                    Console.ResetColor();

                    if (File.Exists(inventoryFilePath))
                    {
                        MyItemInfo[] inventoryItemsArray = playerFileManager.LoadInventoryItems();
                        player.inventoryItems = new List<MyItemInfo>(inventoryItemsArray);
                    }

                    EnterLobby(player, playerFileManager);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\t\t\t\t\t[저장된 플레이어 정보가 없습니다.]");
                    Console.ResetColor();
                    return;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t\t\t\t\t[올바른 옵션을 선택하세요.]");
                Console.ResetColor();
                return;
            }
        }

        public static void EnterLobby(Player player, FileManager fileManager)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n\t\t\t    스파르타 마을에 오신 {player.playerName}님 환영합니다.\n\t\t       이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
            Console.ResetColor();
            Console.WriteLine("\t\t\t\t\t1. 상태보기");
            Console.WriteLine("\t\t\t\t\t2. 인벤토리");
            Console.WriteLine("\t\t\t\t\t3. 상    점");
            Console.WriteLine("\t\t\t\t\t4. 던    전");
            Console.WriteLine("\t\t\t\t\t5. 수정주점");
            Console.WriteLine("\t\t\t\t\t6. 게임종료");
            Console.Write(">> ");
            bool inputCheck = false;

            while (!inputCheck)
            {
                int input = int.Parse(Console.ReadLine());

                switch (input)
                {
                    case 1:
                        SystemPanel.DisplayStatus(player, fileManager);
                        inputCheck = true;
                        break;
                    case 2:
                        VisitInventory.DisplayInventory(player, fileManager);
                        inputCheck = true;
                        break;
                    case 3:
                        VisitShop.DisplayShopItems(player, fileManager);
                        inputCheck = true;
                        break;
                    case 4:
                        if(player.health < 30)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\t\t\t[체력이 30% 미만입니다. 휴식을 취하고 오세요.]");
                            Console.ResetColor();
                            Bar.DisplayHealth(player, fileManager);
                            inputCheck = true;
                        }
                        else
                        {
                            Adventure.DisplayLevel(player, fileManager);
                            inputCheck = true;
                        }
                        break;
                    case 5:
                        Bar.DisplayHealth(player, fileManager);
                        inputCheck = true;
                        break;
                    case 6:
                        GameOver();
                        inputCheck = true;
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\t\t\t\t\t[올바른 옵션을 선택하세요.]");
                        Console.ResetColor();
                        break;
                }
            }
        }

        private static void GameOver()
        {
            Console.WriteLine("\n\t\t\t\t     다음시간에 만나요.");
        }
    }
}