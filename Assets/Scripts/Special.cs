using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Special : MonoBehaviour
{
    Movement playerScript;
    public bool isActive;
    Vector2 size;
    SpriteRenderer spriteRenderer;
    readonly float limitToDestroy = 1.8f;
    void Awake()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        isActive = true;
        size = new Vector2(1f, 1.5f);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnDrawGizmosSelected()
    {
        Vector2 size = new Vector2(1f, 1.5f);
        var center = new Vector3(transform.position.x + 1.3f, transform.position.y);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, size);
    }
    void Update()
    {
        Vector3 movimento = new Vector3(10f, 0, 0.0f);
        this.transform.position = transform.position + movimento * Time.deltaTime;
        HitParams hp = new HitParams(spriteRenderer.sortingOrder, 10, transform.position.x, 0, true, true);

        var hitbox = Physics2D.BoxCastAll(new Vector2(transform.position.x + 1.3f, transform.position.y), size, 0, Vector2.up);

        foreach (var hit in hitbox)
        {
            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                hit.collider.SendMessage("GetHit", hp);
            }
        }
        LayerControl();
        if (transform.position.x >= limitToDestroy)
        {
            playerScript.specialActive = false;
            Destroy(gameObject);
        }

    }
    private void LayerControl()
    {
        spriteRenderer.sortingOrder = -(int)(transform.position.y * 100);
    }
}
