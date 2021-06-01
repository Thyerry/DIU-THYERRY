using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

public abstract class Character : MonoBehaviour
{
    [SerializeField]
    protected float horizontal;
    [SerializeField]
    protected float vertical;
    [SerializeField]
    protected Transform leftPunch;
    [SerializeField]
    protected Transform rightPunch;
    [SerializeField]
    protected float punchRadius = 0.1f;
    [SerializeField]
    public int MaxHealth;
    [SerializeField]
    protected int MaxHitStun;
    [SerializeField]
    protected int layerRange = 8;
    [SerializeField]
    protected float jumpForce = 210;
    [SerializeField]
    protected States state;
    [SerializeField]
    AudioSource AudioGetHit;
    [SerializeField]
    AudioSource AudioDeath;
    [SerializeField]
    protected AudioSource AudioJumpAttack;

    protected static Vector2 LimitY = new Vector2(0.039f, -0.841f);
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected new Rigidbody2D rigidbody2D;
    public int currentLife;
    protected int currentHitStun;


    protected virtual void OnDrawGizmosSelected()
    {
        if (leftPunch == null && rightPunch == null)
            return;

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(leftPunch.position, punchRadius);
        Gizmos.DrawWireSphere(rightPunch.position, punchRadius);
    }

    private void Start()
    {
        currentLife = MaxHealth;
        InvokeRepeating("ReduceHitStun", 0, 0.5f);
    }

    protected virtual void GetHit(HitParams hp)
    {
        Debug.Log($"{name}  {currentLife} {currentHitStun}");
        if (state != States.down)
        {
            if (hp.special
                || (hp.SpriteLayer + layerRange >= spriteRenderer.sortingOrder
                    && hp.SpriteLayer - layerRange <= spriteRenderer.sortingOrder))
            {
                currentLife -= hp.Damage;
                currentHitStun += hp.hitStun;
                AnimationHandler(hp);
            }
        }

    }

    private void AnimationHandler(HitParams hp)
    {
        if (currentHitStun >= MaxHitStun)
        {
            hp.heavyHit = true;
            currentHitStun = 0;
        }

        var hitDirection = transform.position.x > hp.AtkPositionX;
        AudioGetHit.Play();
        if (currentLife <= 0)
        {
            AudioDeath.Play();
            animator.SetBool("Death", true);
            DownAnimation(hitDirection);
        }
        else
        {
            if (hp.heavyHit || state == States.jumping)
                DownAnimation(hitDirection);
            else
            {
                state = States.hit;
                animator.SetTrigger("GetHit");
                spriteRenderer.flipX = hitDirection;
            }
        }
    }

    void ReduceHitStun()
    {
        if (currentHitStun > 0)
            currentHitStun -= 1;
    }
    abstract protected void DownAnimation(bool fallSide);
}
