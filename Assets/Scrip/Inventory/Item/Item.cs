using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YFarm.Inventory
{
    public class Item : MonoBehaviour
    {
        public int itemID;
        private SpriteRenderer sr;
        public ItemDetails itemDetails;

        private BoxCollider2D cd;

        private void Awake()
        {
            sr = GetComponentInChildren<SpriteRenderer>();
            cd = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            Init(itemID);
        }

        public void Init(int ID)
        {
            itemID = ID;
            itemDetails = InventoryManager.Instance.GetItemDetails(ID);
            if (itemDetails != null)
            {
                sr.sprite = itemDetails.itemInWorldSprite != null ? itemDetails.itemInWorldSprite : itemDetails.itemIcon;

                Vector2 newSize = new Vector2(sr.sprite.bounds.size.x, sr.sprite.bounds.size.y);
                cd.size = newSize;
                cd.offset = new Vector2(0, sr.sprite.bounds.center.y);
            }
        }
    }

}
