using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerItemFader : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ItemFader[] itemFaders = collision.GetComponentsInChildren<ItemFader>();

        if (itemFaders != null)
        {
            foreach (var item in itemFaders)
            {
                item.FadeOut();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ItemFader[] itemFaders = collision.GetComponentsInChildren<ItemFader>();

        if (itemFaders != null)
        {
            foreach (var item in itemFaders)
            {
                item.FadeIn();
            }
        }
    }
}
