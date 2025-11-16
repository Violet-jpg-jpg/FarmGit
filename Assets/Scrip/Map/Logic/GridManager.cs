using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GridManager : Singleton<GridManager>
{
    public List<MapData_SO> mapDataList = new List<MapData_SO>();
    //场景名字+坐标 -> 瓦片信息
    private Dictionary<string,TileDetails> tileDetailsDict = new Dictionary<string, TileDetails>();

    void Start()
    {
        foreach(var mapData in mapDataList)
        {
            InitTileDetailsDict(mapData);
        }
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
    
}

