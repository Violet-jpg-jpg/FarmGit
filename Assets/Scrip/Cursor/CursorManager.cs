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

    void OnEnable()
    {
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
    }

    void OnDisable()
    {
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
    }

    

    private void Start()
    {
        cursorCanvas = GameObject.FindGameObjectWithTag("CursorCanvas").GetComponent<RectTransform>();
        cursorImage = cursorCanvas.GetChild(0).GetComponent<Image>();
        Cursor.visible = false;
        SetCursorImage(normal);
        currentSprite = normal;
    }

    void Update()
    {
        if(cursorCanvas == null) return;

        cursorImage.transform.position = Input.mousePosition;
        if(!IsInteractWithUI())
            SetCursorImage(currentSprite);
        else
            SetCursorImage(normal);
    }

    private void SetCursorImage(Sprite sprite)
    {
        cursorImage.sprite = sprite;
        cursorImage.color = new Color(1,1,1,1);
    }

    private void OnItemSelectedEvent(ItemDetails details, bool isSelected)
    {
        if(!isSelected)
            currentSprite = normal;
        else
        {
            currentSprite = details.itemType switch
            {
                ItemType.Seed => seed,
                ItemType.ChopTool => tool,
                ItemType.Commodity => item,
                _ => normal 
            };
        }
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
