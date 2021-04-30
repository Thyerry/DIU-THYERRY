using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalsiaEnemy : Enemy
{
    public float speed;
    public float chaseDistance;
    public float stopDistance;
    public GameObject target;

    private float targetDistance;
    private bool isMoving;
    private bool targetJump;
    Animator animator;
    SpriteRenderer spriterenderer;
    private void Start()
    {
        animator = GetComponent<Animator>();
        spriterenderer = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        targetDistance = Vector2.Distance(transform.position, target.transform.position);
        targetJump = target.GetComponent<Animator>().GetBool("Jumping");

        if (targetDistance < chaseDistance && targetDistance > stopDistance)
        {
            isMoving = true;
            ChasePlayer();
        }
        else
        {
            isMoving = false;
        }

        OrientationControl();
        LayerControl();
        SetAnimatorVariables();
    }

    private void LayerControl()
    {
        spriterenderer.sortingOrder = -(int)(transform.position.y * 100);
    }

    private void OrientationControl()
    {
        if (transform.position.x < target.transform.position.x)
            spriterenderer.flipX = false;

        else
            spriterenderer.flipX = true;
    }

    private void SetAnimatorVariables()
    {
        animator.SetBool("Movement", isMoving);
    }

    private void ChasePlayer()
    {
        if (!targetJump)
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        else
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x, transform.position.y), speed * Time.deltaTime);
    }
}
