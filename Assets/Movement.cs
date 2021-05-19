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
        Debug.Log(characterLife);
        AnimatorControllerInit();
        CheckAxis();
        if (axisY >= this.transform.position.y)
            OnLanding();

        if (state != States.hit && state != States.down)
        {
            AttackControl();

            if (!animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("atk"))
                MovementControl();

            OrientationControl();
        }
        LimitsControl();
        LayerControl();
        AnimatorControllerUpdate();
    }

    private void AttackControl()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (state != States.jumping)
                state = States.attaking;
            StartCoroutine(Attack());
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
        if (!isJumping)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                state = States.jumping;
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
        {
            if(state != States.down)
                state = States.stand;
            transform.position = new Vector3(transform.position.x, axisY, 0.0f);
        }
        else
            axisY = transform.position.y;
        
        isJumping = false;
        rigidbody2D.gravityScale = 0.0f;
        rigidbody2D.Sleep();

        animator.SetBool("Jumping", isJumping);
    }

    protected override IEnumerator Attack()
    {

        animator.SetTrigger("Attacking");
        yield return new WaitForSeconds(0.1f);

        Vector2 punchPosition = spriteRenderer.flipX ? leftPunch.position : rightPunch.position;

        var hitList = Physics2D.CircleCastAll(punchPosition, punchRadius, Vector2.up);
        var heavyHit = animator.GetBool("Jumping");

        foreach (var hit in hitList)
        {
            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                var hitParans = new HitParams(spriteRenderer.sortingOrder, 5, transform, heavyHit);
                hit.collider.SendMessage("GetHit", hitParans);
            }
        }
    }

    protected override void DownAnimation(bool fallSide)
    {
        isJumping = true;
        if (axisY == 9999)
            axisY = transform.position.y;

        animator.SetTrigger("Down");
        float yforce = state != States.jumping ? jumpForce / 3 : 0;
        state = States.down;

        spriteRenderer.flipX = fallSide;
        rigidbody2D.gravityScale = 1.0f;
        var xforce = spriteRenderer.flipX ? 150f : -150f;
        rigidbody2D.WakeUp();
        rigidbody2D.AddForce(new Vector2(xforce, yforce));
    }

    #region Animation events
    void RespawnMessage()
    {
        Destroy(gameObject);
        spawner.SendMessage("Spawn");
        SceneManager.LoadScene("SampleScene");
    }

    void GetHitState()
    {
        if (state != States.hit)
            state = States.hit;
        else
            state = States.stand;
    }
    
    void ToStand()
    {
        state = States.stand;
    }
    #endregion
}
