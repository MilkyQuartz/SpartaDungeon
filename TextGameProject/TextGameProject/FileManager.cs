
using System;
using System.Collections.Generic;
using System.IO;
using static TextGameProject.ItemInfo;

namespace TextGameProject
{
    public class FileManager
    {
        internal string playerFilePath;
        internal string inventoryFilePath;
        internal string shopItemFilePath;

        public FileManager(string playerFilePath, string inventoryFilePath, string shopItemFilePath)
        {
            this.playerFilePath = playerFilePath;
            this.inventoryFilePath = inventoryFilePath;
            this.shopItemFilePath = shopItemFilePath;
        }

        public void SavePlayerData(Player player)
        {
            using (StreamWriter writer = new StreamWriter(playerFilePath))
            {
                writer.WriteLine($"이름: {player.playerName}");
                writer.WriteLine($"직업: {player.job}");
                writer.WriteLine($"레벨: {player.level}");
                writer.WriteLine($"공격력: {player.attack}");
                writer.WriteLine($"방어력: {player.defense}");
                writer.WriteLine($"체력: {player.health}");
                writer.WriteLine($"Gold: {player.gold}G");
                writer.WriteLine($"던전 클리어 횟수: {player.clearCount}");
            } 
        }

        public Player ReadPlayer(string playerFilePath, FileManager fileManager)
        {
            Player player = new Player(this);

            if (File.Exists(playerFilePath))
            {
                using (StreamReader reader = new StreamReader(playerFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(':');
                        if (parts.Length == 2)
                        {
                            string key = parts[0].Trim();
                            string value = parts[1].Trim();

                            switch (key)
                            {
                                case "이름":
                                    player.playerName = value;
                                    break;
                                case "직업":
                                    player.job = value;
                                    break;
                                case "레벨":
                                    player.level = int.Parse(value); 
                                    break;
                                case "공격력":
                                    player.attack = int.Parse(value);
                                    break;
                                case "방어력":
                                    player.defense = int.Parse(value);
                                    break;
                                case "체력":
                                    player.health = int.Parse(value);
                                    break;
                                case "Gold":
                                    player.gold = int.Parse(value.Replace("G", ""));
                                    break;
                                case "던전 클리어 횟수":
                                    player.clearCount = int.Parse(value);
                                    break;
                            }
                        }
                    }
                }
            }

            return player;
        }

        public MyItemInfo[] LoadInventoryItems()
        {
            List<MyItemInfo> myItems = new List<MyItemInfo>();

            if (File.Exists(inventoryFilePath))
            {
                using (StreamReader reader = new StreamReader(inventoryFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split('|');
                        if (parts.Length == 7)
                        {
                            MyItemInfo item = new MyItemInfo
                            {
                                itemName = parts[0],
                                ability = new Ability(
                                    int.Parse(parts[1].Split('+')[1]),
                                    int.Parse(parts[2].Split('+')[1]),
                                    int.Parse(parts[3].Split('+')[1])  
                                ),
                                information = parts[4],
                                gold = int.Parse(parts[5].Split('G')[0]),
                                isEquipped = bool.Parse(parts[6])
                            };

                            myItems.Add(item);
                        }
                    }
                }
            }

            return myItems.ToArray();
        }

        public void SaveInventoryItems(MyItemInfo[] items, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var item in items)
                {
                    string abilityString = FormatAbility(item.ability);
                    writer.WriteLine($"{item.itemName}|{abilityString}|{item.information}|{item.gold}G|{item.isEquipped}");
                }
            }
        }

        //

        public void SaveShopItems(List<ShopItemInfo> shopItems)
        {
            using (StreamWriter writer = new StreamWriter(shopItemFilePath))
            {
                foreach (var item in shopItems)
                {
                    string abilityString = FormatAbility(item.ability);
                    writer.WriteLine($"{item.itemName}|{abilityString}|{item.information}|{item.gold}G|{item.isPurchased}");
                }
            }
        }

        public void UpdateShop(string itemName, bool isPurchased)
        {
            List<ShopItemInfo> shopItems = LoadShopItems();
            foreach (var item in shopItems)
            {
                if (item.itemName.Equals(itemName, StringComparison.OrdinalIgnoreCase))
                {
                    item.isPurchased = isPurchased;
                    break;
                }
            }

            SaveShopItems(shopItems);
        }


        private string FormatAbility(Ability ability)
        {
            return $"체력 +{ability.health}|공격력 +{ability.attack}|방어력 +{ability.defense}";
        }

        public List<ShopItemInfo> LoadShopItems()
        {
            List<ShopItemInfo> shopItems = new List<ShopItemInfo>();
            if (File.Exists(shopItemFilePath))
            {
                using (StreamReader reader = new StreamReader(shopItemFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        //Console.WriteLine(line); 

                        string[] parts = line.Split('|');

                        if (parts.Length == 7)
                        {
                            ShopItemInfo item = new ShopItemInfo
                            {
                                itemName = parts[0],
                                ability = ParseAbility(parts[1], parts[2], parts[3]),
                                information = parts[4],
                                gold = int.Parse(parts[5].Replace("G", "")),
                                isPurchased = bool.Parse(parts[6])
                            };

                            shopItems.Add(item);
                        }
                        else
                        {
                            Console.WriteLine("상점 아이템 정보가 올바르지 않습니다.");
                        }
                    }
                }
            }

            return shopItems;
        }
        private Ability ParseAbility(params string[] abilityParts)
        {
            int health = 0, attack = 0, defense = 0;

            foreach (var part in abilityParts)
            {
                string trimmedPart = part.Trim();
                if (trimmedPart.StartsWith("체력"))
                {
                    health += GetAbilityValue(trimmedPart);
                }
                else if (trimmedPart.StartsWith("공격력"))
                {
                    attack += GetAbilityValue(trimmedPart);
                }
                else if (trimmedPart.StartsWith("방어력"))
                {
                    defense += GetAbilityValue(trimmedPart);
                }
            }

            return new Ability(health, attack, defense);
        }

        private int GetAbilityValue(string abilityPart)
        {
            string[] parts = abilityPart.Split('+');
            if (parts.Length >= 2)
            {
                int.TryParse(parts[1].Trim(), out int value);
                return value;
            }
            return 0;
        }
    }
}
