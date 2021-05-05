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
        var hitDirection = transform.position.x > hp.AtkTransform.position.x;
        if ((hp.SpriteLayer + layerRange >= spriteRenderer.sortingOrder && hp.SpriteLayer - layerRange <= spriteRenderer.sortingOrder)
            && !animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("down"))
        {
            if (characterLife - hp.Damage <= 0)
            {
                animator.SetTrigger("Death");
                DeathAnimation(hitDirection);
            }
            else
            {
                animator.SetTrigger("GetHit");
                characterLife -= hp.Damage;
                spriteRenderer.flipX = hitDirection;
            }
        }
    }

    abstract protected IEnumerator Attack();
    abstract protected void DeathAnimation(bool fallSide);
}
