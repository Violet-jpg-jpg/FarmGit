using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YFarm.Inventory
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        public ItemDataList_SO itemDataList_SO;//连接数据库
        public InventoryBag_SO playerBag;

        void Start()
        {
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Bag, playerBag.inventoryItems);
        }

        /// <summary>
        /// ͨ查找物品
        /// </summary>
        /// <param name="ID">Item ID</param>
        /// <returns></returns>
        public ItemDetails GetItemDetails(int ID)
        {
            return itemDataList_SO.itemData.Find(i => i.itemID == ID);
        }

        /// <summary>
        /// 添加物品
        /// </summary>
        /// <param name="itemDetails"></param>
        /// <param name="destroy">是否销毁</param>
        public void AddItem(Item item, bool destroy)
        {
            //在背包中查找是否存在该物品
            var index = GetItemIndexInBag(item.itemID);

            //不使用List自带的Add函数进行添加
            //使用Add函数会导致背包容量变大
            AddItemAtIndex(item.itemID, index, 1);

            if (destroy)
            {
                Destroy(item.gameObject);
            }

            //更新背包UI
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Bag, playerBag.inventoryItems);
        }

        /// <summary>
        /// 判断背包是否还有空余
        /// </summary>
        /// <returns></returns>
        public bool CheckBagCapacity()
        {
            for (int i = 0; i < playerBag.inventoryItems.Count; i++)
            {
                if (playerBag.inventoryItems[i].itemID == 0)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 查找背包中是否含有该物品
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>若存在则返回下标，若不存在则返回-1</returns>
        public int GetItemIndexInBag(int ID)
        {
            for (int i = 0; i < playerBag.inventoryItems.Count; i++)
            {
                if (playerBag.inventoryItems[i].itemID == ID)
                    return i;
            }
            return -1;
        }
        /// <summary>
        /// 通过下标添加物品
        /// </summary>
        /// <param name="ID">添加物品的ID</param>
        /// <param name="index">该物品已经存在背包中的下标</param>
        /// <param name="amount">添加数量</param>
        public void AddItemAtIndex(int ID, int index, int amount)
        {
            if (index == -1 && CheckBagCapacity())
            {
                var item = new InventoryItem { itemID = ID, itemAmount = amount };
                for (int i = 0; i < playerBag.inventoryItems.Count; i++)
                {
                    if (playerBag.inventoryItems[i].itemID == 0)
                    {
                        playerBag.inventoryItems[i] = item;
                        break;
                    }
                }
            }
            else
            {
                int currentAmount = amount + playerBag.inventoryItems[index].itemAmount;
                var item = new InventoryItem { itemID = ID, itemAmount = currentAmount };
                playerBag.inventoryItems[index] = item;
            }
        }

        public void SwapItem(int fromIndex,int targetIndex)
        {
            var currentItem = playerBag.inventoryItems[fromIndex];
            var targetItem = playerBag.inventoryItems[targetIndex];

            if (targetItem.itemID != 0)
            {
                playerBag.inventoryItems[fromIndex] = targetItem;
                playerBag.inventoryItems[targetIndex] = currentItem;
            }
            else
            {
                playerBag.inventoryItems[targetIndex] = currentItem;
                playerBag.inventoryItems[fromIndex] = new InventoryItem();
            }

            EventHandler.CallUpdateInventoryUI(InventoryLocation.Bag,playerBag.inventoryItems);
        }
    }
    
    

}
