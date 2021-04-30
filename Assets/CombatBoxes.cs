using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatBoxes : MonoBehaviour
{
    [SerializeField]
    protected GameObject parent;

    [SerializeField]
    protected SpriteRenderer parentSpriteRenderer;
    protected Transform parentTransform;

    // Update is called once per frame

    private void Awake()
    {
        parentSpriteRenderer = parent.GetComponent<SpriteRenderer>();
        parentTransform = parent.GetComponent<Transform>();
    }
    void Update()
    {
        if (parentSpriteRenderer.flipX)
            transform.position = new Vector3(parentTransform.position.x - 0.3f, transform.position.y, transform.position.z); 
        else
            transform.position = new Vector3(parentTransform.position.x + 0.3f, transform.position.y, transform.position.z);
    }
}
