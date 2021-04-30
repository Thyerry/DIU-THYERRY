using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour, IAnimatorController
{
    private SpriteRenderer spriteRenderer;
    private Animator movementAnimator;
    private Rigidbody2D rigidbody;

    static Vector2 LimitY = new Vector2(0.039f, -0.841f);

    private float axisY;
    private float jumpForce = 210;
    private float xspeed = 1f;
    private float yspeed = .8f;
    private float horizontal;
    private float vertical;
    #region Animator Controllers
    private bool walking;
    private bool isJumping;
    #endregion

    private AnimatorClipInfo[] clipInfos;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        movementAnimator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.Sleep();
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorControllerInit();
        CheckAxis();

        if (!clipInfos[0].clip.name.Contains("atk"))
            MovementControl();

        LimitsControl();
        LayerControl();
        OrientationControl();
        AnimatorControllerUpdate();
    }

    private void LimitsControl()
    {
        if(!isJumping)
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, LimitY.y, LimitY.x), 0.0f);
    }

    private void CheckAxis()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    private void OrientationControl()
    {
        if (horizontal > 0)
            spriteRenderer.flipX = false;
        else if (horizontal < 0)
            spriteRenderer.flipX = true;
    }

    private void MovementControl()
    {
        if (axisY >= this.transform.position.y)
            OnLanding();

        if (!isJumping)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                axisY = transform.position.y;
                isJumping = true;
                rigidbody.gravityScale = 1.5f;
                rigidbody.WakeUp();
                rigidbody.AddForce(new Vector2(0, jumpForce));
                movementAnimator.SetBool("Jumping", isJumping);
            }

            Vector3 movimento = new Vector3(horizontal * xspeed, vertical * yspeed, 0.0f);
            this.transform.position = transform.position + movimento * Time.deltaTime;

            if (horizontal != 0 || vertical != 0)
                walking = true;
        }
        
        else
        {
            Vector3 movimento = new Vector3(horizontal * xspeed, this.transform.position.y, 0.0f);
            this.transform.position = transform.position + movimento * Time.deltaTime;
        }
    }
    private void LayerControl()
    {
        if(!isJumping)
            spriteRenderer.sortingOrder = -(int)(transform.position.y * 100) + 1;
        else
            spriteRenderer.sortingOrder = -(int)(axisY * 100) + 1;
    }

    public void AnimatorControllerInit()
    {
        clipInfos = movementAnimator.GetCurrentAnimatorClipInfo(0);
        walking = false;
    }

    public void AnimatorControllerUpdate()
    {
        movementAnimator.SetBool("Walking", walking);
    }

    public void OnLanding()
    {
        if (isJumping)
            transform.position = new Vector3(transform.position.x, axisY, 0.0f);
        else
            axisY = transform.position.y;

        isJumping = false;
        rigidbody.gravityScale = 0.0f;
        rigidbody.Sleep();
        
        movementAnimator.SetBool("Jumping", isJumping);
    }
}
