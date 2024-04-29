using System;
using System.Collections.Generic;
using System.Numerics;

namespace TextGameProject
{
    public class Player
    {
        public string playerName;
        public string job;
        public int level;
        public int attack;
        public int defense;
        public int health;
        public int gold;
        public int clearCount; 

        public List<MyItemInfo> inventoryItems = new List<MyItemInfo>();

        private FileManager fileManager;

        public Player(FileManager fileManager)
        {
            this.fileManager = fileManager;
        }

        public void LevelUp()
        {
            if(clearCount >= 5)
            {
                Console.WriteLine("\t\t\t\t[LevelUp]");
                this.level++;
                this.attack += 1;
                this.defense += 1;
                this.health += 1;

                this.clearCount = 0;
            }
            fileManager.SavePlayerData(this);
        }

        public void AddItemToInventory(ShopItemInfo shopItem)
        {
            MyItemInfo newItem = new MyItemInfo()
            {
                itemName = shopItem.itemName,
                ability = shopItem.ability,
                information = shopItem.information,
                gold = shopItem.gold,
                isEquipped = false
            };

            inventoryItems.Add(newItem);

            fileManager.SaveInventoryItems(inventoryItems.ToArray(), fileManager.inventoryFilePath);
        }

        public void EquipItem(string itemName)
        {
            MyItemInfo[] inventoryItems = fileManager.LoadInventoryItems();

            foreach (var item in inventoryItems)
            {
                if (item.itemName == itemName)
                {
                    this.attack += item.ability.attack;
                    this.defense += item.ability.defense;
                    this.health += item.ability.health;
                    item.isEquipped = true;

                    break;
                }
            }
            fileManager.SavePlayerData(this);
            fileManager.SaveInventoryItems(inventoryItems, fileManager.inventoryFilePath);
        }

        public void UnequipItem(string itemName)
        {
            MyItemInfo[] inventoryItems = fileManager.LoadInventoryItems();

            foreach (var item in inventoryItems)
            {
                if (item.itemName == itemName)
                {
                    this.attack -= item.ability.attack;
                    this.defense -= item.ability.defense;
                    this.health -= item.ability.health;

                    item.isEquipped = false;

                    break;
                }
            }

            fileManager.SavePlayerData(this);
            fileManager.SaveInventoryItems(inventoryItems, fileManager.inventoryFilePath);
        }
    }
}
