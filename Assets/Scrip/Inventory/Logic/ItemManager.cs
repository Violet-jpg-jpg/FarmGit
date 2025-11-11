using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YFarm.Inventory;

public class ItemManager : MonoBehaviour
{
    public Item itemPrefab;
    private Transform itemParent;

    void OnEnable()
    {
        EventHandler.InstantiateItemInScene += OnInstantiateItemInScene;
        EventHandler.AfterSceneUnLoadEvent += OnAfterSceneLoadEvnet;
    }

    void OnDisable()
    {
        EventHandler.InstantiateItemInScene -= OnInstantiateItemInScene;
        EventHandler.AfterSceneUnLoadEvent -= OnAfterSceneLoadEvnet;
    }
    
    private void OnAfterSceneLoadEvnet()
    {
        itemParent = GameObject.FindWithTag("ItemParent").transform;
    }
    

    private void OnInstantiateItemInScene(int ID,Vector3 pos)
    {
        var item = Instantiate(itemPrefab, pos, Quaternion.identity, itemParent);
        item.itemID = ID;
    }
}
