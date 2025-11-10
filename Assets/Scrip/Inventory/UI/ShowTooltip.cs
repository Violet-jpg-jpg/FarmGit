using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace YFarm.Inventory
{
    [RequireComponent(typeof(SlotUI))]
    public class ShowTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private SlotUI slot;
        private InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();


        void Awake()
        {
            slot = GetComponent<SlotUI>();
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (slot.itemAmount != 0)
            {
                inventoryUI.itemTooltip.gameObject.SetActive(true);
                inventoryUI.itemTooltip.SetupTooltip(slot.itemDetails, slot.slotType);

                inventoryUI.itemTooltip.transform.position = transform.position + Vector3.up * 60;
            }
            else
                inventoryUI.itemTooltip.gameObject.SetActive(false);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            inventoryUI.itemTooltip.gameObject.SetActive(false);
        }
    }
}
