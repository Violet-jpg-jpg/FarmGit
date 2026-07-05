using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator[] anims;
    public float speed;

    private float xInput;
    private float yInput;
    private bool isMoving;
    private Vector2 movementInput;

    private bool inputDisable = false;

    //动画用数据
    private float mouseX;
    private float mouseY;
    private bool useTool;

    void OnEnable()
    {
        EventHandler.BeforeSceneUnLoadEvent += OnBeforeSceneUnLoad;
        EventHandler.AfterSceneUnLoadEvent += OnAfterSceneUnLoad;
        EventHandler.MoveToPosition += OnMoveToPosition;
        EventHandler.MouseClickEvent += OnMouseClickEvent;
    }

    void OnDisable()
    {
        EventHandler.BeforeSceneUnLoadEvent -= OnBeforeSceneUnLoad;
        EventHandler.AfterSceneUnLoadEvent -= OnAfterSceneUnLoad;
        EventHandler.MoveToPosition -= OnMoveToPosition;
        EventHandler.MouseClickEvent -= OnMouseClickEvent;
        
    }

    private void OnMouseClickEvent(Vector3 mouseWorldPos, ItemDetails item)
    {
        //TODO:更换角色动画
        if(item.itemType != ItemType.Seed && item.itemType != ItemType.Commodity && item.itemType != ItemType.Furniture)
        {
            mouseX = mouseWorldPos.x - transform.position.x;
            mouseY = mouseWorldPos.y - transform.position.y;

            //处理斜方向
            if(Mathf.Abs(mouseX) > Mathf.Abs(mouseY))
                mouseY = 0;
            else
                mouseX = 0;
            
            StartCoroutine(UseToolRoutine(mouseWorldPos,item));
        }
        else
            EventHandler.CallExcuteActionAfterANimation(mouseWorldPos,item);
        
    }

    private IEnumerator UseToolRoutine(Vector3 mouseWorldPos,ItemDetails itemDetails)
    {
        useTool = true;
        inputDisable = true;
        yield return null;
        foreach(var anim in anims)
        {
            anim.SetTrigger("useTool");
            //更改角色方向
            anim.SetFloat("InputX",mouseX);
            anim.SetFloat("InputY",mouseY);
        }
        yield return new WaitForSeconds(0.45f);
        EventHandler.CallExcuteActionAfterANimation(mouseWorldPos,itemDetails);
        yield return new WaitForSeconds(0.25f);
        useTool = false;
        inputDisable = false;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anims = GetComponentsInChildren<Animator>();
    }

    private void Update()
    {
        if (!inputDisable)
            PlayerInput();
        else
            isMoving = false;
        SwitchAnimation();
    }

    private void FixedUpdate()
    {
        if (!inputDisable)
            Movement();
    }

    private void OnBeforeSceneUnLoad()
    {
        inputDisable = true;
    }

    private void OnAfterSceneUnLoad()
    {
        inputDisable = false;
    }
    
    private void OnMoveToPosition(Vector3 targetPosition)
    {
        transform.position = targetPosition;
    }

    private void PlayerInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");



        movementInput = new Vector2(xInput, yInput).normalized;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementInput.x *= .5f;
            movementInput.y *= .5f;
            xInput *= .5f;
            yInput *= .5f;
        }

        isMoving = movementInput != Vector2.zero;
    }

    private void Movement()
    {
        rb.MovePosition(rb.position + movementInput * speed * Time.deltaTime);
    }
    
    private void SwitchAnimation()
    {
        foreach(var anim in anims)
        {
            anim.SetBool("isMoving", isMoving);
            anim.SetFloat("mouseX",mouseX);
            anim.SetFloat("mouseY",mouseY);
            if(isMoving)
            {
                anim.SetFloat("InputX", xInput);
                anim.SetFloat("InputY", yInput);
            }
        }
    }
}
