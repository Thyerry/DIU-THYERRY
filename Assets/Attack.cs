using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : CombatBoxes
{
    Animator parentAnimator;
    [SerializeField]
    protected float axisY;

    // Start is called before the first frame update
    void Awake()
    {
        parentSpriteRenderer = parent.GetComponent<SpriteRenderer>();
        parentTransform = parent.GetComponent<Transform>();
        parentAnimator = parent.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!parentAnimator.GetBool("Jumping") )
        {
            if(axisY != -100.0f)
            {
                transform.position = new Vector3(parentTransform.position.x, axisY, transform.position.z);
                axisY = -100.0f;
            }
            if (parentSpriteRenderer.flipX)
                transform.position = new Vector3(parentTransform.position.x - 0.3f, transform.position.y, transform.position.z);
            else
                transform.position = new Vector3(parentTransform.position.x + 0.3f, transform.position.y, transform.position.z);
        }
        else
        {
            if(axisY == -100.0f)
            {
                axisY = 0;
            }
            if (parentSpriteRenderer.flipX)
                transform.position = new Vector3(parentTransform.position.x - 0.3f, axisY, transform.position.z);
            else
                transform.position = new Vector3(parentTransform.position.x + 0.3f, axisY, transform.position.z);
        }
    }
}
