using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : Character, IAnimatorController
{
    public float axisY;
    private float xspeed = 1f;
    private float yspeed = .8f;

    #region Animator Controllers
    private bool walking;
    private bool isJumping;
    #endregion

    private AnimatorClipInfo[] clipInfos;
    private GameObject spawner;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidbody2D.Sleep();

        
        spawner = GameObject.Find("PlayerSpawner");
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorControllerInit();
        CheckAxis();
        if (axisY >= this.transform.position.y)
            OnLanding();

        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsName("blaze_get_hit") && !animator.GetCurrentAnimatorStateInfo(0).IsName("blaze_down"))
        {
            if (!clipInfos[0].clip.name.Contains("atk"))
                MovementControl();

            AttackControll();
            OrientationControl();
        }
        LimitsControl();
        LayerControl();
        AnimatorControllerUpdate();
    }

    private void AttackControll()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            StartCoroutine(Attack());
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
        if (!isJumping)
            axisY = 9999;
    }

    public void AnimatorControllerUpdate()
    {
        animator.SetBool("Walking", walking);
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
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        animator.SetTrigger("Attacking");
        yield return new WaitForSeconds(0.1f);

        Vector2 punchPosition = spriteRenderer.flipX ? leftPunch.position : rightPunch.position;

        var hitList = Physics2D.CircleCastAll(punchPosition, punchRadius, Vector2.up);

        foreach (var hit in hitList)
        {
            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                var hitParans = new HitParams(spriteRenderer.sortingOrder, 5, transform);
                hit.collider.SendMessage("GetHit", hitParans);
            }
        }
        
    }
    
    protected override void DeathAnimation(bool fallSide)
    {
        isJumping = true;
        if(axisY == 9999)
            axisY = transform.position.y;

        spriteRenderer.flipX = fallSide;
        rigidbody2D.gravityScale = 1.0f;
        var xforce = spriteRenderer.flipX ? 150f : -150f;
        rigidbody2D.WakeUp();
        rigidbody2D.AddForce(new Vector2(xforce, jumpForce / 3));
    }

    void RespawnMessage()
    {
        Destroy(gameObject);
        spawner.SendMessage("Spawn");
        SceneManager.LoadScene("SampleScene");
    }
}
