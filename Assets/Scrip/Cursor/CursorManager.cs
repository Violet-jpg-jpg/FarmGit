using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    public Sprite normal,seed,tool,item;

    private Sprite currentSprite;

    private Image cursorImage;
    private RectTransform cursorCanvas;

    //鼠标检测
    private Camera mainCamera;
    private Grid currentGrid;
    private Vector3 mouseWorldPos;
    private Vector3Int mouseGridPos;
    private bool cursorEnable;
    private bool cursorPostionValid;//判断鼠标当前位置是否可用
    private ItemDetails currentItem;//当前选中的物品

    //角色检测
    private Transform PlayerTransform => FindObjectOfType<Player>().transform;

    void OnEnable()
    {
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
        EventHandler.AfterSceneUnLoadEvent += OnAfterSceneUnLoadEvent;
        EventHandler.BeforeSceneUnLoadEvent += OnBeforeSceneUnLoadEvent;
    }

    

    void OnDisable()
    {
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
        EventHandler.AfterSceneUnLoadEvent -= OnAfterSceneUnLoadEvent;
        EventHandler.BeforeSceneUnLoadEvent -= OnBeforeSceneUnLoadEvent;
    }

    

    private void Start()
    {
        cursorCanvas = GameObject.FindGameObjectWithTag("CursorCanvas").GetComponent<RectTransform>();
        cursorImage = cursorCanvas.GetChild(0).GetComponent<Image>();
        Cursor.visible = false;
        SetCursorImage(normal);
        currentSprite = normal;

        mainCamera = Camera.main;
    }

    void Update()
    {
        if(cursorCanvas == null) return;

        cursorImage.transform.position = Input.mousePosition;
        if(!IsInteractWithUI() && cursorEnable)
        {
            SetCursorImage(currentSprite);
            CheckCursorValid();
        }
        else
            SetCursorImage(normal);
    }

    #region 设置鼠标样式
    /// <summary>
    /// 设置鼠标图片
    /// </summary>
    /// <param name="sprite"></param>
    private void SetCursorImage(Sprite sprite)
    {
        cursorImage.sprite = sprite;
        cursorImage.color = new Color(1,1,1,1);
    }

    ///
    private void OnItemSelectedEvent(ItemDetails details, bool isSelected)
    {
        if(!isSelected)
        {
            currentItem = null;
            cursorEnable = false;
            currentSprite = normal;
        }
        else
        {
            currentSprite = details.itemType switch
            {
                ItemType.Seed => seed,
                ItemType.ChopTool => tool,
                ItemType.HoeTool => tool,
                ItemType.CollectTool => tool,
                ItemType.BreakTool => tool,
                ItemType.ReapTool => tool,
                ItemType.WaterTool => tool,
                ItemType.Commodity => item,
                _ => normal 
            };
            currentItem = details;
            cursorEnable = true;
        }
    }

    private void CheckCursorValid()
    {
        mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);

        TileDetails currentTiles = GridManager.Instance.GetTileDetailsOnMousePosition(mouseGridPos);

        var playerGridPos = currentGrid.WorldToCell(PlayerTransform.position);

        //判断是否在使用范围内
        if(Mathf.Abs(mouseGridPos.x - playerGridPos.x) > currentItem.itemUseRadius || Mathf.Abs(mouseGridPos.y - playerGridPos.y) > currentItem.itemUseRadius)
        {
            SetCursorInValid();
            return;
        }

        if(currentTiles != null)
        {
            switch(currentItem.itemType)
            {
                case ItemType.Commodity:
                    if(currentTiles.canDropItem && currentItem.canDropped) SetCursorValid();else SetCursorInValid();
                    break;
            }
        }
        else
        {
            SetCursorInValid();
        }
    }

    /// <summary>
    /// 设置鼠标可用
    /// </summary>
    private void SetCursorValid()
    {
        cursorPostionValid = true;
        cursorImage.color = new Color(1,1,1,1);
    }

    /// <summary>
    /// 设置鼠标不可用
    /// </summary>
    private void SetCursorInValid()
    {
        cursorPostionValid = false;
        cursorImage.color = new Color(1,0,0,0.5f);
    }
    #endregion

    private void OnAfterSceneUnLoadEvent()
    {
        currentGrid = FindObjectOfType<Grid>();
    }

    private void OnBeforeSceneUnLoadEvent()
    {
        cursorEnable = false;
    }


    private bool IsInteractWithUI()
    {
        if(EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        return false;
    }
}
