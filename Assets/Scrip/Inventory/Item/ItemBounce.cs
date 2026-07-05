using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBounce : MonoBehaviour
{
    private Transform spriteTrans;
    private BoxCollider2D coll;

    public float gravity = -3.5f;
    private bool isGround;
    private Vector2 dir;
    private Vector3 targetPos;
    private float distance;
    void Awake()
    {
        spriteTrans = transform.GetChild(0);
        coll = GetComponent<BoxCollider2D>();
        coll.enabled = false;
    }

    void Update()
    {
        Bounce();
    }
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="direction">位移方向</param>
    /// <param name="target">位移终点</param>
    public void IniteItemBounce(Vector2 direction,Vector3 target)
    {
        dir = direction;
        targetPos = target;
        coll.enabled = false;
        distance = Vector3.Distance(targetPos,transform.position);
        spriteTrans.position += Vector3.up * 1.5f;
    }

    private void Bounce()
    {
        isGround = spriteTrans.position.y <= transform.position.y;

        if(Vector3.Distance(targetPos,transform.position) > 0.05f)
        {
            transform.position += (Vector3)dir * distance * -gravity * Time.deltaTime;
        }

        if(!isGround)
        {
            spriteTrans.position += Vector3.up * gravity * Time.deltaTime;
        }
        else
        {
            spriteTrans.position = transform.position;
            coll.enabled = true;
        }
    }
}
