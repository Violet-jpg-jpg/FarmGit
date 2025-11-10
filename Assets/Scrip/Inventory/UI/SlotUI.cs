using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

namespace YFarm.Inventory
{
    public class SlotUI : MonoBehaviour,IPointerClickHandler,IBeginDragHandler,IDragHandler,IEndDragHandler
    {
        [Header("组件获取")]
        [SerializeField] private Image slotImage;
        [SerializeField] private TextMeshProUGUI amountText;
        public Image slotHighLight;
        [SerializeField] private Button button;
        private InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();
        public SlotType slotType;
        public bool isSelected;

        //物品信息
        public ItemDetails itemDetails;//存储当前格子物品信息
        public int itemAmount;//存储当前格子物品数量
        public int slotIndex;//当前格子下标

        private void Start()
        {
            isSelected = false;
            if (itemDetails.itemID == 0)
                UpdateEmptySlot();
        }


        /// <summary>
        /// 更新格子信息
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        public void UpdateSlot(ItemDetails item, int amount)
        {
            itemDetails = item;
            slotImage.enabled = true;
            slotImage.sprite = item.itemIcon;
            itemAmount = amount;
            amountText.text = amount.ToString();
            button.interactable = true;
        }

        /// <summary>
        /// 更新空格子
        /// </summary>
        public void UpdateEmptySlot()
        {
            if (isSelected)
                isSelected = false;
            slotImage.enabled = false;
            amountText.text = "";
            button.interactable = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (itemAmount == 0) return;
            isSelected = !isSelected;
            inventoryUI.UpdateHighLight(slotIndex);

            if(slotType == SlotType.Bag)
            {
                EventHandler.CallItemSelectedEvent(itemDetails, isSelected);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(itemAmount != 0)
            {
                inventoryUI.dragImage.enabled = true;
                inventoryUI.dragImage.sprite = slotImage.sprite;
                //防止因为拖拽图片和Slot图片尺寸不同被异常拉伸
                inventoryUI.dragImage.SetNativeSize();

                isSelected = true;
                inventoryUI.UpdateHighLight(slotIndex);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            inventoryUI.dragImage.transform.position = Input.mousePosition;
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            inventoryUI.dragImage.enabled = false;

            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                if (eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>() == null)
                    return;
                var targetItem = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>();
                int targetIndex = targetItem.slotIndex;

                if (slotType == SlotType.Bag && targetItem.slotType == SlotType.Bag)
                {
                    InventoryManager.Instance.SwapItem(slotIndex, targetIndex);
                }
                inventoryUI.UpdateHighLight(-1);
            }
            //else
            //{
                //var pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
                //EventHandler.CallInstantiateItemInScene(itemDetails.itemID, pos);
            //}
        }

    }
}
