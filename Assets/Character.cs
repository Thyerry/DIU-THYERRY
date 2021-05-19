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
    protected int characterLife = 10;
    [SerializeField]
    protected int layerRange = 8;
    [SerializeField]
    protected float jumpForce = 210;
    [SerializeField]
    protected States state;

    protected static Vector2 LimitY = new Vector2(0.039f, -0.841f);
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rigidbody2D;


    protected virtual void OnDrawGizmosSelected()
    {
        if (leftPunch == null && rightPunch == null)
            return;

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(leftPunch.position, punchRadius);
        Gizmos.DrawWireSphere(rightPunch.position, punchRadius);
    }

    protected virtual void GetHit(HitParams hp)
    {
        if (hp.SpriteLayer + layerRange >= spriteRenderer.sortingOrder
            && hp.SpriteLayer - layerRange <= spriteRenderer.sortingOrder
            && state != States.down)
        {
            characterLife -= hp.Damage;
            AnimationHandler(hp);
        }
    }

    private void AnimationHandler(HitParams hp)
    {
        var hitDirection = transform.position.x > hp.AtkTransform.position.x;
        if (characterLife <= 0)
        {
            animator.SetBool("Death", true);
            DownAnimation(hitDirection);
        }
        else
        {
            if(hp.heavyHit || state == States.jumping)
                DownAnimation(hitDirection);
            else
            {
                animator.SetTrigger("GetHit");
                spriteRenderer.flipX = hitDirection;
            }
        }
    }

    abstract protected IEnumerator Attack();
    abstract protected void DownAnimation(bool fallSide);
}
