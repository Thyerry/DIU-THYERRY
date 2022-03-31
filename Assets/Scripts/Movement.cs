using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : Character, IAnimatorController
{
    [SerializeField]
    Transform activePunch;
    [SerializeField]
    GameObject Special;
    [SerializeField]
    GameObject GameOver;
    [SerializeField]
    MainTheme mainTheme;
    public bool specialActive;
    public int score = 0;
    public float axisY;
    private float xspeed = 1f;
    private float yspeed = .8f;

    

    #region Animator Controllers
    private bool walking;
    private bool isJumping;

    #endregion

    private AnimatorClipInfo[] clipInfos;
    private GameObject spawner;
    public bool specialToken;
    private int specialPoints;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigidbody2D.Sleep();

        Special.transform.position = new Vector3(-11f, -0.4f);
        specialActive = false;

        spawner = GameObject.Find("PlayerSpawner");
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorControllerInit();
        CheckAxis();
        if (axisY >= this.transform.position.y)
            OnLanding();

        if (state != States.hit && state != States.down)
        {
            AttackControl();

            if (!clipInfos[0].clip.name.Contains("atk"))
            {
                MovementControl();
                OrientationControl();
            }
        }

        SpecialCast();
        LimitsControl();
        LayerControl();
        AnimatorControllerUpdate();
    }

    private void AttackControl()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (clipInfos[0].clip.name)
            {
                case "blaze_atk_01":
                    animator.ResetTrigger("Attacking");
                    animator.SetBool("ComboHit2", true);
                    break;

                case "blaze_atk_02":
                    animator.SetBool("ComboHit2", false);
                    animator.SetBool("ComboHit3", true);
                    break;

                case "blaze_atk_03":
                    animator.SetBool("ComboHit3", false);
                    animator.SetBool("ComboHit4", true);
                    break;

                default:
                    animator.SetTrigger("Attacking");
                    break;
            }

            if (state != States.jumping)
                state = States.attaking;
        }
    }

    private void SpecialCast()
    {
        if (Input.GetKeyDown(KeyCode.E) 
            && !specialActive)
        {
            if (!specialToken && currentLife > 20)
            {
                currentLife -= 20;
                Instantiate(Special);
                specialActive = true;
            }
            else if (specialToken)
            {
                specialToken = false;
                specialPoints = 0;
                Instantiate(Special);
                specialActive = true;
            }
        }
    }
    public void SpecialInatcvate()
    {
        specialActive = false;
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

        activePunch.position = spriteRenderer.flipX ? leftPunch.position : rightPunch.position;
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
            if (state != States.down)
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

    protected void Attack(HitParams hp)
    {
       /// HitParams hp = new HitParams(sl, dmg, at, hh);
        Vector2 punchPosition = activePunch.position;

        var hitList = Physics2D.CircleCastAll(punchPosition, punchRadius, Vector2.up);

        if(animator.GetBool("Jumping"))
        {
            AudioJumpAttack.Play();
            hp.heavyHit = true;
        }

        foreach (var hit in hitList)
        {
            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                hit.collider.SendMessage("GetHit", hp);
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
        mainTheme.SendMessage("StopPlaying");
        GameOver.SendMessage("ShowGameOverScreen");
        //spawner.SendMessage("Spawn");
        //SceneManager.LoadScene("SampleScene");
    }

    void GetHitState()
    {
        state = States.stand;
    }

    void ToStand()
    {
        state = States.stand;
    }

    void SetBoolToFalse(string theBool)
    {
        animator.SetBool(theBool, false);
        if (theBool == "ComboHit4")
            resetAttacking();
    }
    void resetAttacking()
    {
        animator.ResetTrigger("Attacking");
    }
    void ScoreUp()
    {
        score++;
        if(!specialToken)
        {
            specialPoints++;
            if (specialPoints >= 20)
                specialToken = true;
        }
    }
    public void StartAttack(string attackName)
    {

        HitParams hp = new HitParams(spriteRenderer.sortingOrder, 5, transform.position.x, 1 ,false, false);

        switch (attackName)
        {
            case "blaze_atk_04":
                hp.heavyHit = true;
                break;

            default: 
                break;
        }

        Attack(hp);
    }
    #endregion
}
