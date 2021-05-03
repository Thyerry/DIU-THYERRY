using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets;

public class GalsiaEnemy : Enemy, IAnimatorController
{
    public float speed;
    public float chaseDistance;
    public float stopDistance;
    private float playerDistance;
    private bool isMoving;
    private bool playerJump;

    private void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        AnimatorControllerInit();
        playerDistance = Vector2.Distance(transform.position, player.transform.position);
        playerJump = player.GetComponent<Animator>().GetBool("Jumping");

        if (playerDistance < chaseDistance && playerDistance > stopDistance)
        {
            isMoving = true;
            //ChasePlayer();
        }

        OrientationControl();
        LayerControl();
        AnimatorControllerUpdate();
    }

    private void LayerControl()
    {
        spriteRenderer.sortingOrder = -(int)(transform.position.y * 100);
    }

    private void OrientationControl()
    {
        if (transform.position.x < player.transform.position.x)
            spriteRenderer.flipX = false;

        else
            spriteRenderer.flipX = true;
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
