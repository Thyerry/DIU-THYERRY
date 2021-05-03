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
        animator.SetTrigger("GetHit");
        //print("BATEU");
    }

    abstract protected IEnumerator Attack();
}
