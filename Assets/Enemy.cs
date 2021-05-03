using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

public class Enemy : Character
{

    // distancia minima do inimigo do player em x = 0,4
    // distancia minima do inimigo do player em y = 0,1

    enum States { patrol, pursuit }
    [SerializeField]
    States state = States.patrol;

    [SerializeField]
    float searchRange = 1;

    [SerializeField]
    float stoppingDistance = 0.4f;

    protected GameObject player;
    Vector3 target;
    Vector2 vel;
    bool isMoving;


    void Start()
    {
        target = GetComponent<Transform>().position;
        player = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("SetTarget", 2, 2);
        InvokeRepeating("SendPunch", 0, 2);

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    protected override void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRange);
        Gizmos.DrawWireSphere(target, 0.1f);
        Gizmos.DrawWireCube(target, new Vector3(0.5f,0.1f,0));

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
        AnimatorControllerInit();
        if (state == States.pursuit)
        {
            if(!player.GetComponent<Animator>().GetBool("Jumping"))
                target = player.transform.position;
            else
                target = new Vector3(player.transform.position.x, player.GetComponent<Movement>().axisY, 0f);

            if(Vector3.Distance(target, transform.position) > searchRange * 1.2f)
            {
                target = transform.position;
                state = States.patrol;
                return;
            }
        }

        var radiusPatrol = Physics2D.CircleCast(transform.position, searchRange, Vector2.up);

        if (state == States.patrol && radiusPatrol.collider != null && radiusPatrol.collider.CompareTag("Player"))
        {
            state = States.pursuit;
        }     

        var clipinfo = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "galsia_attack";
        vel = target - transform.position;
        if(!clipinfo)
            OrientationControl();
        if(vel.magnitude < stoppingDistance)
            vel = Vector2.zero;
        
        vel.Normalize();

        if (!clipinfo)
            rigidbody2D.velocity = new Vector2(vel.x * horizontal, vel.y * vertical);

        if(rigidbody2D.velocity != Vector2.zero)
        {
            isMoving = true;
        }

        LayerControl();
        AnimatorControllerUpdate();
    }
    protected override IEnumerator Attack()
    {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("galsia_idle"))
        {
            animator.SetTrigger("CanAttack");
            yield return new WaitForSeconds(0.1f);

            Vector2 punchPosition = spriteRenderer.flipX ? leftPunch.position : rightPunch.position;

            var hit = Physics2D.CircleCast(punchPosition, punchRadius, Vector2.up);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                //print(hit.collider.gameObject.GetComponent<SpriteRenderer>().sortingOrder);
                HitParams hitParans = new HitParams()
                {
                    SpriteLayer = spriteRenderer.sortingOrder,
                    Damage = 0
                };
                hit.collider.SendMessage("GetHit", hitParans);
            }
        }
    }
    void SendPunch()
    {
        if (state == States.patrol || vel.magnitude != 0)
            return;
        StartCoroutine(Attack());
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
}
