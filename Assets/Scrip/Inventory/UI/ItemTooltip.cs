using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private GameObject bottomObject;

    public void SetupTooltip(ItemDetails item,SlotType slotType)
    {
        nameText.text = item.itemName;
        typeText.text = GetItemType(item.itemType);
        descriptionText.text = item.itemDescription;
        if (item.itemType == ItemType.Seed || item.itemType == ItemType.Furniture || item.itemType == ItemType.Commodity)
        {
            bottomObject.SetActive(true);
            var price = item.itemPrice;

            if (slotType == SlotType.Bag)
            {
                price = (int)(price * item.sellPercentage);
            }

            valueText.text = price.ToString();
        }
        else
            bottomObject.SetActive(false);
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }
    private string GetItemType(ItemType itemType)
    {
        return itemType switch
        {
            ItemType.Seed => "种子",
            ItemType.Furniture => "家具",
            ItemType.Commodity => "商品",
            ItemType.HoeTool => "工具",
            ItemType.BreakTool => "工具",
            ItemType.ChopTool => "工具",
            ItemType.CollectTool => "工具",
            ItemType.WaterTool => "工具",
            ItemType.ReapTool => "工具",
            _ => "无"
        };
    }

}
