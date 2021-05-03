using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : Character, IAnimatorController
{
    public float axisY;
    private float jumpForce = 210;
    private float xspeed = 1f;
    private float yspeed = .8f;

    #region Animator Controllers
    private bool walking;
    private bool isJumping;
    private bool atkJump;
    private bool atkCombo01;
    #endregion

    private AnimatorClipInfo[] clipInfos;


    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidbody2D.Sleep();
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorControllerInit();
        CheckAxis();
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("blaze_get_hit"))
        {
            if (!clipInfos[0].clip.name.Contains("atk"))
                MovementControl();
            
            AttackControll();
        }
        LimitsControl();
        LayerControl();
        OrientationControl();
        AnimatorControllerUpdate();
    }

    private void AttackControll()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (animator.GetBool("Jumping") || Input.GetKeyDown(KeyCode.Space))
            {
                atkJump = true;
            }

            else
                atkCombo01 = true;
        }
    }

    private void LimitsControl()
    {
        if (!isJumping)
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                axisY = transform.position.y;
                isJumping = true;
                rigidbody2D.gravityScale = 1.5f;
                rigidbody2D.WakeUp();
                rigidbody2D.AddForce(new Vector2(0, jumpForce));
                animator.SetBool("Jumping", isJumping);
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
        if (!isJumping)
            spriteRenderer.sortingOrder = -(int)(transform.position.y * 100) + 1;
        else
            spriteRenderer.sortingOrder = -(int)(axisY * 100) + 1;
    }

    public void AnimatorControllerInit()
    {
        clipInfos = animator.GetCurrentAnimatorClipInfo(0);
        walking = false;
        atkJump = false;
        atkCombo01 = false;
    }

    public void AnimatorControllerUpdate()
    {
        animator.SetBool("Walking", walking);
        animator.SetBool("Atk_Combo_01", atkCombo01);
        animator.SetBool("Atk_Jump", atkJump);
    }

    public void OnLanding()
    {
        if (isJumping)
            transform.position = new Vector3(transform.position.x, axisY, 0.0f);
        else
            axisY = transform.position.y;

        isJumping = false;
        rigidbody2D.gravityScale = 0.0f;
        rigidbody2D.Sleep();

        animator.SetBool("Jumping", isJumping);
    }

    protected override IEnumerator Attack()
    {
        throw new NotImplementedException();
    }
}
