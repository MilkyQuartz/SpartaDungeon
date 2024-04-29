using System;

namespace TextGameProject
{
    internal class Bar
    {
        public static void DisplayHealth(Player player, FileManager fileManager)
        {
            Console.WriteLine("  ===========================================================================================");
            Console.WriteLine("  |                                       주    점                                          |");
            Console.WriteLine("  ===========================================================================================");
            Console.WriteLine($"\t\t\t      현재 체력: {player.health} | 현재 골드: {player.gold}");
            Console.WriteLine(" 1. [카수테리켈로] 자신의 보유 체력의 50%를 채워준다.(체력 50일때 +25)   - 가격 : 100G");
            Console.WriteLine(" 2. [복분자주] 정읍의 자랑, 100을 기준으로 체력을 50% 채워준다.          - 가격 : 300G");
            Console.WriteLine(" 3. [조니왔다] 유명 위스키, 100을 기준으로 체력을 100% 채워준다.         - 가격 : 500G");
            Console.WriteLine(" 0. 나가기");

            while (true)
            {
                Console.Write(">> ");
                int input = int.Parse(Console.ReadLine());

                switch (input)
                {
                    case 1:
                        if (player.gold < 100)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\t\t\t\t\t[돈이 부족하여 구매할 수 없습니다.]");
                            Console.ResetColor();
                            break;
                        }
                        Console.WriteLine("\t\t\t  \"키야~ 역시 한국인이라면 이 맥주를 마셔줘야지!\"");
                        player.health += player.health / 2;
                        if (player.health > 100)
                        {
                            player.health = 100;
                        }
                        player.gold -= 100;

                        fileManager.SavePlayerData(player);
                        break;
                    case 2:
                        if (player.gold < 300)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\t\t\t\t\t[돈이 부족하여 구매할 수 없습니다.]");
                            Console.ResetColor();
                            break;
                        }
                        Console.WriteLine("\t\t\"아차차~ 이런 술은 난생 처음 마셔봤네! 너무 맛있다! 자주 사먹어야겠는걸?\"");
                        player.health += 50;
                        if (player.health > 100)
                        {
                            player.health = 100;
                        }
                        player.gold -= 300;

                        fileManager.SavePlayerData(player);
                        break;
                    case 3:
                        if (player.gold < 500)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\t\t\t\t\t[돈이 부족하여 구매할 수 없습니다.]");
                            Console.ResetColor();
                            break;
                        }
                        Console.WriteLine("\t\t\t\t\t \"비싸구만.. \"");
                        player.health += 100;
                        if (player.health > 100)
                        {
                            player.health = 100;
                        }
                        player.gold -= 500;

                        fileManager.SavePlayerData(player);
                        break;
                    case 0:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\t\t\t\t    [로비로 이동합니다.]");
                        Console.ResetColor();
                        Program.EnterLobby(player, fileManager);
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\t\t\t\t\t[올바른 옵션을 선택하세요.]");
                        Console.ResetColor();
                        break;
                }
            }
        }
    }
}
