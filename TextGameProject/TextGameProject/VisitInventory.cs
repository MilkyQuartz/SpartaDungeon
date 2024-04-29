using System;

namespace TextGameProject
{
    internal class VisitInventory
    {
        public static void DisplayInventory(Player player, FileManager fileManager)
        {
            Console.WriteLine("  ===========================================================================================");
            Console.WriteLine("  |                                       인벤토리                                          |");
            Console.WriteLine("  ===========================================================================================");
            Console.WriteLine("\t\t\t\t\t[아이템 목록]");

            var items = fileManager.LoadInventoryItems();

            if (items.Length == 0)
            {
                Console.WriteLine("        \t\t\t\t    텅~");
            }
            else
            {
                foreach (var item in items)
                {
                    string equippedStatus = item.isEquipped ? "[E]" : "";
                    Console.WriteLine($"{equippedStatus} {item.itemName}: 체력 +{item.ability.health}, 공격력 +{item.ability.attack}, 방어력 +{item.ability.defense}");
                }
            }

            Console.WriteLine("\n\t\t\t\t\t1. 장착/해제 관리\n\t\t\t\t\t0. 나가기");
            Console.Write(">> ");

            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int input))
                {
                    switch (input)
                    {
                        case 0:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\t\t\t\t    [로비로 이동합니다.]");
                            Console.ResetColor();
                            Program.EnterLobby(player, fileManager);
                            break;

                        case 1:
                            ManageEquipments(player, fileManager);
                            break;

                        default:
                            Console.WriteLine("\t\t\t\t\t[올바른 옵션을 선택하세요.]");
                            Console.Write(">> ");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("숫자를 입력하세요.");
                    Console.Write(">> ");
                }
            }
        }

        private static void ManageEquipments(Player player, FileManager fileManager)
        {
            var items = fileManager.LoadInventoryItems();

            if (items.Length == 0)
            {
                Console.WriteLine("\t\t\t[장착할 수 있는 아이템이 존재하지 않습니다.]");
                Console.WriteLine("\t\t\t\t\t0. 나가기");
            }
            else
            {
                Console.WriteLine("장착 또는 해제할 아이템을 선택하세요:");
                for (int i = 0; i < items.Length; i++)
                {
                    string equippedStatus = items[i].isEquipped ? "[E]" : "";
                    Console.WriteLine($"{i + 1}. {equippedStatus} {items[i].itemName}");
                }

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    if (choice >= 1 && choice <= items.Length)
                    {
                        int index = choice - 1;
                        MyItemInfo selectedItem = items[index];

                        if (selectedItem.isEquipped)
                        {
                            player.UnequipItem(selectedItem.itemName);
                            Console.WriteLine($"{selectedItem.itemName}의 장착을 해제했습니다.");
                        }
                        else
                        {
                            player.EquipItem(selectedItem.itemName);
                            Console.WriteLine($"{selectedItem.itemName}을(를) 장착했습니다.");
                        }

                        selectedItem.isEquipped = !selectedItem.isEquipped;
                        fileManager.SaveInventoryItems(items, fileManager.inventoryFilePath);

                        Console.WriteLine("\n\t\t\t\t\t1. 장착/해제 관리\t\t\t\t\t0. 나가기");
                        Console.Write(">> ");
                    }
                    else
                    {
                        Console.WriteLine("\t\t\t\t\t[올바른 옵션을 선택하세요.]");
                    }
                }
            }
        }
    }
}
