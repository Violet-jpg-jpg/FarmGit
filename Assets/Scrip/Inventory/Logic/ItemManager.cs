using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using YFarm.Inventory;

public class ItemManager : MonoBehaviour
{
    public Item itemPrefab;
    private Transform itemParent;

    private Dictionary<string, List<SceneItem>> sceneItemDict = new Dictionary<string, List<SceneItem>>();

    void OnEnable()
    {
        EventHandler.InstantiateItemInScene += OnInstantiateItemInScene;
        EventHandler.BeforeSceneUnLoadEvent += OnBeforeSceneUnLoadEvent;
        EventHandler.AfterSceneUnLoadEvent += OnAfterSceneLoadEvnet;
    }

    void OnDisable()
    {
        EventHandler.InstantiateItemInScene -= OnInstantiateItemInScene;
        EventHandler.BeforeSceneUnLoadEvent -= OnBeforeSceneUnLoadEvent;
        EventHandler.AfterSceneUnLoadEvent -= OnAfterSceneLoadEvnet;
    }

    private void OnBeforeSceneUnLoadEvent()
    {
        GetAllSceneItem();
    }

    private void OnAfterSceneLoadEvnet()
    {
        itemParent = GameObject.FindWithTag("ItemParent").transform;
        RecreateAllItems();
    }


    private void OnInstantiateItemInScene(int ID, Vector3 pos)
    {
        var item = Instantiate(itemPrefab, pos, Quaternion.identity, itemParent);
        item.itemID = ID;
    }

    private void GetAllSceneItem()
    {
        List<SceneItem> currentSceneItem = new List<SceneItem>();
        foreach (var item in FindObjectsOfType<Item>())
        {
            SceneItem sceneItem = new SceneItem
            {
                itemID = item.itemID,
                pos = new SerializableVector3(item.transform.position),
            };
            currentSceneItem.Add(sceneItem);
        }

        if (sceneItemDict.ContainsKey(SceneManager.GetActiveScene().name))
        {
            sceneItemDict[SceneManager.GetActiveScene().name] = currentSceneItem;
        }

        else
        {
            sceneItemDict.Add(SceneManager.GetActiveScene().name, currentSceneItem);
        }
    }
    
    private void RecreateAllItems()
    {
        List<SceneItem> currentSceneItem = new List<SceneItem>();

        if(sceneItemDict.TryGetValue(SceneManager.GetActiveScene().name,out currentSceneItem))
        {
            if(currentSceneItem != null)
            {
                foreach (var item in FindObjectsOfType<Item>())
                {
                    Destroy(item.gameObject);
                }
                
                foreach(var item in currentSceneItem)
                {
                    Item newItem = Instantiate(itemPrefab, item.pos.ToVector3(), Quaternion.identity, itemParent);
                    newItem.Init(item.itemID);
                }
            }
        }
    }
}
