using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SwicthBound : MonoBehaviour
{

    void OnEnable()
    {
        EventHandler.AfterSceneUnLoadEvent += SwitchConfinerShape;
    }

    void OnDisable()
    {
        EventHandler.AfterSceneUnLoadEvent -= SwitchConfinerShape;
    }
    private void SwitchConfinerShape()
    {
        PolygonCollider2D confinerShape = GameObject.FindGameObjectWithTag("BoundConfiner").GetComponent<PolygonCollider2D>();
        CinemachineConfiner confiner = GetComponent<CinemachineConfiner>();
        confiner.m_BoundingShape2D = confinerShape;
        //切换场景时调用，清理缓存
        confiner.InvalidatePathCache();
    }
}
