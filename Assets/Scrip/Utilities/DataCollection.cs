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
    public int itemUseRadius;

    public bool canPickUp;
    public bool canDropped;
    public bool canCarried;

    public int itemPrice;
    [Range(0,1)]
    public float sellPercentage;
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

[Serializable]
public class SerializableVector3
{
    public float x, y, z;
    public SerializableVector3(Vector3 vector3)
    {
        this.x = vector3.x;
        this.y = vector3.y;
        this.z = vector3.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }

    public Vector2Int ToVector2Int()
    {
        return new Vector2Int((int)x, (int)y);
    }
}

[Serializable]
public class SceneItem
{
    public int itemID;
    public SerializableVector3 pos;
}
