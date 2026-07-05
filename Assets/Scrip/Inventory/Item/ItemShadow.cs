using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShadow : MonoBehaviour
{
    private SpriteRenderer shadowSprite;
    public SpriteRenderer itemSprite;

    void Awake()
    {
        shadowSprite = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        shadowSprite.sprite = itemSprite.sprite;
        shadowSprite.color = new Color(0,0,0,0.3f);
    }
}
