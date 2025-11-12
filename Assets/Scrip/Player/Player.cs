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

    void OnEnable()
    {
        EventHandler.BeforeSceneUnLoadEvent += OnBeforeSceneUnLoad;
        EventHandler.AfterSceneUnLoadEvent += OnAfterSceneUnLoad;
        EventHandler.MoveToPosition += OnMoveToPosition;
    }

    void OnDisable()
    {
        EventHandler.BeforeSceneUnLoadEvent -= OnBeforeSceneUnLoad;
        EventHandler.AfterSceneUnLoadEvent -= OnAfterSceneUnLoad;
        EventHandler.MoveToPosition -= OnMoveToPosition;
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anims = GetComponentsInChildren<Animator>();
    }

    private void Update()
    {
        if(!inputDisable)
            PlayerInput();
        SwitchAnimation();
    }

    private void FixedUpdate()
    {
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
            if(isMoving)
            {
                anim.SetFloat("InputX", xInput);
                anim.SetFloat("InputY", yInput);
            }
        }
    }
}
