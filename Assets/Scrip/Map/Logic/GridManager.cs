using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;


public class GridManager : Singleton<GridManager>
{
    public List<MapData_SO> mapDataList = new List<MapData_SO>();

    public RuleTile digTile;
    public RuleTile waterTile;
    private Tilemap digTilemap;
    private Tilemap waterTilemap;
    //场景名字+坐标 -> 瓦片信息
    private Dictionary<string,TileDetails> tileDetailsDict = new Dictionary<string, TileDetails>();
    private Grid currentGrid;

    void Start()
    {
        foreach(var mapData in mapDataList)
        {
            InitTileDetailsDict(mapData);
        }
    }

    void OnEnable()
    {
        EventHandler.ExcuteActionAfterAnimation += OnExcuteActionAfterAnimation;
        EventHandler.AfterSceneUnLoadEvent += OnAfterSceneUnLoadEvent;
    }

    void OnDisable()
    {
        EventHandler.ExcuteActionAfterAnimation -= OnExcuteActionAfterAnimation;
        EventHandler.AfterSceneUnLoadEvent -= OnAfterSceneUnLoadEvent;
    }

    private void OnAfterSceneUnLoadEvent()
    {
        currentGrid = FindObjectOfType<Grid>();
        digTilemap = GameObject.FindWithTag("Dig").GetComponent<Tilemap>();
        waterTilemap = GameObject.FindWithTag("Water").GetComponent<Tilemap>();
    }

    private void InitTileDetailsDict(MapData_SO mapData)
    {
        foreach(TileProperty tileProperty in mapData.tileProperties)
        {
            TileDetails tileDetails = new TileDetails
            {
              gridX = tileProperty.tileCordinate.x,
              gridY = tileProperty.tileCordinate.y  
            };

            string key = tileDetails.gridX + "x" + tileDetails.gridY + "y" + mapData.sceneName;
            if(GetTileDetails(key) != null)
            {
                tileDetails = GetTileDetails(key);
            }

            switch(tileProperty.gridType)
            {
                case GridType.Diggble:
                    tileDetails.canDig = tileProperty.boolTypeValue;
                    break;
                case GridType.DropItem:
                    tileDetails.canDropItem = tileProperty.boolTypeValue;
                    break;
                case GridType.PlaceFurniture:
                    tileDetails.canPlaceFurniture = tileProperty.boolTypeValue;
                    break;
                case GridType.NPCObstacle:
                    tileDetails.isNPCObstacle = tileProperty.boolTypeValue;
                    break;
            }

            if(GetTileDetails(key) != null)
                tileDetailsDict[key] = tileDetails;
            else
                tileDetailsDict.Add(key,tileDetails);

        }
    }

    private TileDetails GetTileDetails(string key)
    {
        if(tileDetailsDict.ContainsKey(key))
        {
            return tileDetailsDict[key];
        }
        return null;
    }

    /// <summary>
    /// 根据鼠标网格坐标返回网格信息
    /// </summary>
    /// <param name="mouseGridPos"></param>
    /// <returns></returns>
    public TileDetails GetTileDetailsOnMousePosition(Vector3Int mouseGridPos)
    {
        string key = mouseGridPos.x + "x" + mouseGridPos.y + "y" + SceneManager.GetActiveScene().name;

        return GetTileDetails(key);
    }

    private void OnExcuteActionAfterAnimation(Vector3 mouseWorldPos, ItemDetails item)
    {
        var mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);
        var currentTiles = GetTileDetailsOnMousePosition(mouseGridPos);

        if(currentTiles != null)
        {
            switch(item.itemType)
            {
                case ItemType.Commodity:
                    EventHandler.CallDropItemEvent(item.itemID,mouseWorldPos);
                    break;
                case ItemType.HoeTool:
                    SetDigGround(currentTiles);
                    currentTiles.daySinceDug = 0;
                    currentTiles.canDig = false;
                    currentTiles.canDropItem = false;
                    //音效
                    break;
                case ItemType.WaterTool:
                    SetWaterGround(currentTiles);
                    currentTiles.daySInceWatered = 0;
                    break;
            }
        }
    }

    /// <summary>
    /// 显示挖坑瓦片
    /// </summary>
    /// <param name="tile"></param>
    private void SetDigGround(TileDetails tile)
    {
        Vector3Int pos = new Vector3Int(tile.gridX,tile.gridY,0);
        if(digTilemap != null)
            digTilemap.SetTile(pos,digTile);
        
    }

    private void SetWaterGround(TileDetails tile)
    {
        Vector3Int pos = new Vector3Int(tile.gridX,tile.gridY,0);
        if(waterTilemap != null)
            waterTilemap.SetTile(pos,waterTile);
    }
    
}

