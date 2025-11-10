using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ItemFader : MonoBehaviour
{
    private SpriteRenderer sp;

    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
    }

    ///<summary>
    ///Öð½¥»Ö¸´ÑÕÉ«
    ///</summary>
    public void FadeIn()
    {
        Color targetColor = new Color(1, 1, 1, 1);
        sp.DOColor(targetColor, Settings.fadeDuration);
    }

    ///<summary>
    ///Öð½¥±äÍ¸Ã÷
    ///</summary>
    public void FadeOut()
    {
        Color targetColor = new Color(1, 1, 1, Settings.targetAlpha);
        sp.DOColor(targetColor, Settings.fadeDuration);
    }
}
