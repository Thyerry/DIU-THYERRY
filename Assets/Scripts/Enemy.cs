using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;
using System.Linq;

public class Enemy : Character
{
    [SerializeField]
    float searchRange;
    
    [SerializeField]
    float stoppingDistance = 0.4f;
    [SerializeField]
    protected GameObject player;

    Vector3 target;
    Vector2 vel;
    bool isMoving;
    private bool isJumping;
    float axisY;
    private bool destroy;
    

    void Awake()
    {
        
        target = GetComponent<Transform>().position;
        player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("SetTarget", 2, 2);
        var punchCooldown = Random.Range(1.5f, 2.5f);
        InvokeRepeating("SendPunch", 0, punchCooldown);        

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    protected override void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRange);
        Gizmos.DrawWireSphere(target, 0.1f);
        Gizmos.DrawWireCube(target, new Vector3(0.5f, 0.1f, 0));

        base.OnDrawGizmosSelected();
    }
    void SetTarget()
    {
        if (state != States.patrol)
            return;

        target = new Vector2(transform.position.x + Random.Range(-searchRange, searchRange)
                                , Random.Range(LimitY.y, LimitY.x));

    }
    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            state = States.patrol;
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (state != States.down
            && state != States.hit 
            && state != States.attaking)
            DoAction();

        else if (transform.position.y <= axisY)
        {
            if (isJumping)
                transform.position = new Vector3(transform.position.x, axisY, 0.0f);
            else
                axisY = transform.position.y;

            isJumping = false;
            rigidbody2D.gravityScale = 0.0f;
            rigidbody2D.Sleep();
        }

    }
    private void DoAction()
    {
        try
        {

            AnimatorControllerInit();
            if (state == States.pursuit)
            {
                if (!player.GetComponent<Animator>().GetBool("Jumping"))
                    target = player.transform.position;
                else
                    target = new Vector3(player.transform.position.x, player.GetComponent<Movement>().axisY, 0f);

                if (Vector3.Distance(target, transform.position) > searchRange * 1.2f)
                {
                    target = transform.position;
                    state = States.patrol;
                    return;
                }
            }

            var radiusPatrol = Physics2D.CircleCastAll(transform.position, searchRange, Vector2.up)
                                        .LastOrDefault(p => p.collider.CompareTag("Player"));

            if (state == States.patrol && radiusPatrol.collider != null)
            {
                state = States.pursuit;
            }

            vel = target - transform.position;
            if (state != States.attaking)
                OrientationControl();

            if (vel.magnitude < stoppingDistance)
            {
                int enemyOrder = spriteRenderer.sortingOrder;
                int playerOrder = player.GetComponent<SpriteRenderer>().sortingOrder;

                if (enemyOrder + layerRange >= playerOrder
                    && enemyOrder - layerRange <= playerOrder)
                    vel = Vector2.zero;

                else
                    vel = new Vector2(0, vel.y);
            }

            vel.Normalize();

            if (state != States.attaking)
                rigidbody2D.velocity = new Vector2(vel.x * horizontal, vel.y * vertical);

            if (rigidbody2D.velocity != Vector2.zero)
            {
                isMoving = true;
            }

            LayerControl();
            AnimatorControllerUpdate();
        }
        catch (System.Exception)
        {
            state = States.patrol;
        }
    }
    
    protected IEnumerator Attack()
    {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        state = States.attaking;
        yield return new WaitForSeconds(0.1f);

        Vector2 punchPosition = spriteRenderer.flipX ? leftPunch.position : rightPunch.position;

        var hit = Physics2D.CircleCast(punchPosition, punchRadius, Vector2.up);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            var hitParans = new HitParams(spriteRenderer.sortingOrder, 4, transform.position.x, 20, false, false);
            hit.collider.SendMessage("GetHit", hitParans);
        }
    }
    void SendPunch()
    {
        var stateinfo = animator.GetCurrentAnimatorStateInfo(0);
        if (state == States.patrol || vel.magnitude != 0)
            return;
        animator.SetTrigger("CanAttack");
    }
    private void LayerControl()
    {
        spriteRenderer.sortingOrder = -(int)(transform.position.y * 100);
    }

    private void OrientationControl()
    {
        spriteRenderer.flipX = vel.x < 0;
    }

    public void AnimatorControllerInit()
    {
        isMoving = false;
    }
    public void AnimatorControllerUpdate()
    {
        animator.SetBool("Movement", isMoving);
    }

    protected override void DownAnimation(bool fallside)
    {

        animator.SetTrigger("Down");
        isJumping = true;
        axisY = transform.position.y;
        rigidbody2D.gravityScale = 1.0f;
        var xforce = spriteRenderer.flipX ? 150f : -150f;
        rigidbody2D.WakeUp();
        rigidbody2D.AddForce(new Vector2(xforce, jumpForce / 3));
        state = States.down;
    }

    #region Animation Events
    void DeadForGood()
    {
        Destroy(gameObject);
        player.SendMessage("ScoreUp");
    }
    void BackToPatrol()
    {
        state = States.patrol;
    }
    
    #endregion
}
