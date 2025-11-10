using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace YFarm.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        [Header("物品信息界面")]
        public ItemTooltip itemTooltip;
        [Header("拖拽图片")]
        public Image dragImage;
        [Header("背包界面")]
        [SerializeField] private GameObject bagPanel;
        private bool bagOpened;
        public SlotUI[] playerSlots;

        void OnEnable()
        {
            EventHandler.UpdateInventoryUI += OnUpdateInventoryUI;
        }

        void OnDisable()
        {
            EventHandler.UpdateInventoryUI -= OnUpdateInventoryUI;
        }

        void Awake()
        {
            //初始化背包中格子Index
            for (int i = 0; i < playerSlots.Length; i++)
            {
                playerSlots[i].slotIndex = i;
            }
            bagOpened = bagPanel.activeInHierarchy;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                OpenBagUI();
            }
        }

        private void OnUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)
        {
            switch (location)
            {
                case (InventoryLocation.Bag):

                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].itemAmount > 0)
                        {
                            //背包中仅存储ID和Amount,需要通过InventoryManager拿到物品详细信息调用SlotUI的UpdateUI方法
                            var item = InventoryManager.Instance.GetItemDetails(list[i].itemID);
                            playerSlots[i].UpdateSlot(item, list[i].itemAmount);
                        }
                        else
                        {
                            playerSlots[i].UpdateEmptySlot();
                        }

                    }
                    break;
            }
        }

        public void OpenBagUI()
        {
            bagOpened = !bagOpened;
            bagPanel.SetActive(bagOpened);
        }

        public void UpdateHighLight(int index)
        {
            foreach (var slot in playerSlots)
            {
                if (slot.slotIndex == index && slot.isSelected == true)
                {
                    slot.slotHighLight.gameObject.SetActive(true);
                }
                else
                {
                    slot.isSelected = false;
                    slot.slotHighLight.gameObject.SetActive(false);
                }
            }
        }
    }
}
