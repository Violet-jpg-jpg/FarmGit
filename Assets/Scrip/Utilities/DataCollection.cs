using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class ItemDetails
{
    public int itemID;
    public string itemName;
    public ItemType itemType;

    public Sprite itemIcon;
    public Sprite itemInWorldSprite;

    [TextArea]
    public string itemDescription;
    public int itemUseRadius;//��Ʒʹ�÷�Χ(��λΪ����)

    public bool canPickUp;
    public bool canDropped;
    public bool canCarried;

    public int itemPrice;//����۸�
    [Range(0,1)]
    public float sellPercentage;//��������
}

[Serializable]
public struct InventoryItem
{
    //背包中只存储道具ID和当前背包中该道具数量信息
    public int itemID;
    public int itemAmount;
}

[Serializable]
public class AnimationType
{
    //存储身体部位动画转换
    public PartType partType;
    public PartName partName;
    public AnimatorOverrideController overrideController;
}