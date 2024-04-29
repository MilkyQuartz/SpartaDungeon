using System;
using System.Collections.Generic;
using System.Numerics;

namespace TextGameProject
{
    public class ItemManager
    {
        private FileManager fileManager;
        private string inventoryFilePath;

        public ItemManager(string inventoryFilePath, FileManager fileManager)
        {
            this.inventoryFilePath = inventoryFilePath;
            this.fileManager = fileManager;
        }

        public void AddItemToInventory(MyItemInfo newItem, Player player)
        {
            MyItemInfo[] inventoryItems = fileManager.LoadInventoryItems();

            Array.Resize(ref inventoryItems, inventoryItems.Length + 1);
            inventoryItems[inventoryItems.Length - 1] = newItem;

            fileManager.SaveInventoryItems(inventoryItems, fileManager.inventoryFilePath);
        }

        public void AddItemToShop(ShopItemInfo newItem)
        {
            List<ShopItemInfo> shopItems = new List<ShopItemInfo>(fileManager.LoadShopItems());

            if (!IsItemPurchased(newItem, shopItems))
            {
                shopItems.Add(newItem);
                fileManager.SaveShopItems(shopItems);
            }
            else
            {
                Console.WriteLine("이미 구매된 아이템입니다.");
            }
        }

        private bool IsItemPurchased(ShopItemInfo newItem, List<ShopItemInfo> shopItems)
        {
            foreach (var item in shopItems)
            {
                if (item.itemName == newItem.itemName && item.isPurchased)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class Ability
    {
        public int health;
        public int attack;
        public int defense;

        public Ability(int health, int attack, int defense)
        {
            this.health = health;
            this.attack = attack;
            this.defense = defense;
        }
    }

    public abstract class ItemInfo
    {
        public string itemName;
        public Ability ability;
        public string information;
        public int gold;
    }

    public class MyItemInfo : ItemInfo
    {
        public bool isEquipped;
    }

    public class ShopItemInfo : ItemInfo
    {
        public bool isPurchased;
    }
}
