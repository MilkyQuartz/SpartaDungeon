using System;

namespace TextGameProject
{
    internal class Adventure
    {
        public static void DisplayLevel(Player player, FileManager fileManager)
        {
            bool inputCheck = false;

            while (!inputCheck)
            {
                if (player.health < 30)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\t\t\t[체력이 30% 미만입니다. 휴식을 취하고 오세요.]");
                    Console.ResetColor();
                    Bar.DisplayHealth(player, fileManager);
                    inputCheck = true;
                }
                Console.WriteLine("  ===========================================================================================");
                Console.WriteLine("  |                                       던    전                                          |");
                Console.WriteLine("  ===========================================================================================");
                Console.WriteLine($"\t\t\t\t\t현재 체력: {player.health} | 현재 방어력: {player.defense}");
                Console.WriteLine("\t\t\t\t\t1. 쉬운 던전     | 방어력 5 이상 권장");
                Console.WriteLine("\t\t\t\t\t2. 일반 던전     | 방어력 11 이상 권장");
                Console.WriteLine("\t\t\t\t\t3. 어려운 던전   | 방어력 17 이상 권장");
                Console.WriteLine("\t\t\t\t\t0. 나가기");

                Console.Write(">> ");
                int input = int.Parse(Console.ReadLine());

                switch (input)
                {
                    case 1:
                        Easy(player, fileManager);
                        break;
                    case 2:
                        Medium(player, fileManager);
                        break;
                    case 3:
                        Hard(player, fileManager);
                        break;
                    case 0:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\t\t\t\t    [로비로 이동합니다.]");
                        Console.ResetColor();
                        Program.EnterLobby(player, fileManager);
                        inputCheck = true;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\t\t\t\t\t[올바른 옵션을 선택하세요.]");
                        Console.ResetColor();
                        break;
                }
            }
        }

        private static void Easy(Player player, FileManager fileManager)
        {
            Random rand = new Random();
            int randNum = rand.Next(100);
            int randhealth = rand.Next(20, 36);
            if (player.defense < 5 && randNum < 40)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t\t\t\t[던전 공략에 실패하셨습니다.]");
                Console.ResetColor();
                player.health -= player.health / 2;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t\t\t\t[던전 공략에 성공하셨습니다.]");
                Console.ResetColor();
                player.gold += 1000;
                int randAttack = rand.Next(10, 21);
                int randAttackGold = player.attack * randAttack / 100;
                player.gold += randAttackGold;

                player.health -= randhealth - (player.defense - 5);
                player.clearCount++;
                player.LevelUp();
            }
            fileManager.SavePlayerData(player);
        }

        private static void Medium(Player player, FileManager fileManager)
        {
            Random rand = new Random();
            int randNum = rand.Next(100);
            int randhealth = rand.Next(20, 36);
            if (player.defense < 11 && randNum < 40)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t\t\t\t[던전 공략에 실패하셨습니다.]");
                Console.ResetColor();
                player.health -= player.health / 2;

            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t\t\t\t[던전 공략에 성공하셨습니다.]");
                Console.ResetColor();
                player.gold += 1700;
                int randAttack = rand.Next(15, 26);
                int randAttackGold = player.attack * randAttack / 100;
                player.gold += randAttackGold;
                player.health -= randhealth - (player.defense - 11);
                player.clearCount++;
                player.LevelUp();
            }
            fileManager.SavePlayerData(player);
        }

        private static void Hard(Player player, FileManager fileManager)
        {
            Random rand = new Random();
            int randNum = rand.Next(100);
            int randhealth = rand.Next(25, 36);
            if (player.defense < 17 && randNum < 40)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t\t\t\t[던전 공략에 실패하셨습니다.]");
                Console.ResetColor();
                player.health -= player.health / 2;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t\t\t\t[던전 공략에 성공하셨습니다.]");
                Console.ResetColor();
                player.gold += 2500;
                int randAttack = rand.Next(20, 31);
                int randAttackGold = player.attack * randAttack / 100;
                player.gold += randAttackGold;
                player.health -= randhealth - (player.defense - 17);
                player.clearCount++;
                player.LevelUp();
            }
            fileManager.SavePlayerData(player);
        }
    }
}
