using System;

namespace TextGameProject
{
    internal class SystemPanel
    {
        public static void DisplayStatus(Player player, FileManager fileManager)
        {
            Console.WriteLine("  ===========================================================================================");
            Console.WriteLine("  |                                       상태보기                                          |");
            Console.WriteLine("  ===========================================================================================");
            Console.WriteLine($"\t\t\t\t\tLv. {player.level:D2}");
            Console.WriteLine($"\t\t\t\t\t{player.playerName} ( {player.job} )");
            Console.WriteLine($"\t\t\t\t\t공격력: {player.attack}");
            Console.WriteLine($"\t\t\t\t\t방어력: {player.defense}");
            Console.WriteLine($"\t\t\t\t\t체력: {player.health}");
            Console.WriteLine($"\t\t\t\t\tGold: {player.gold}\n");
            Console.WriteLine("\t\t\t\t\t0. 나가기");

            Console.Write(">> ");
            int input = int.Parse(Console.ReadLine());

            if (input == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t\t\t\t    [로비로 이동합니다.]");
                Console.ResetColor();
                Program.EnterLobby(player, fileManager);

            }
        }
    }
}
