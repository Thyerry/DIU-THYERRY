using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUpController : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    [SerializeField]
    int recoveryAmount;
    [SerializeField]
    AudioSource AudioLifeUp;

    SpriteRenderer playerSp;
    SpriteRenderer spriteRenderer;
    new Rigidbody2D rigidbody2D;
    int maxHealth;
    int currentHealth;

    bool landed;
    float axisY;
    Vector2 LimitY = new Vector2(-0.45f, 0.06f);
    Vector2 LimitX = new Vector2(-6.8f, -1.0f);
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerSp = player.GetComponent<SpriteRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        maxHealth = player.GetComponent<Movement>().MaxHealth;

        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.gravityScale = 0.5f;
        rigidbody2D.WakeUp();

        axisY = Random.Range(LimitY.x, LimitY.y);
        transform.position = new Vector3(Random.Range(LimitX.x, LimitX.y), 1.25f, 0);
    }

    void Update()
    {
        if (axisY >= transform.position.y)
        {
            Onlanding();
        }
        LayerControl();
        try
        {
            currentHealth = player.GetComponent<Movement>().currentLife;
            var getLife = Physics2D.BoxCast(transform.position, new Vector2(0.1f, 0.1f), 0f, Vector2.up);

            if (getLife.collider != null
                && getLife.collider.CompareTag("Player")
                && spriteRenderer.sortingOrder + 3 >= playerSp.sortingOrder
                && spriteRenderer.sortingOrder - 3 <= playerSp.sortingOrder
                && currentHealth != maxHealth)
            {
                if (recoveryAmount + currentHealth >= maxHealth)
                    currentHealth = maxHealth;
                else
                    currentHealth += recoveryAmount;

                AudioLifeUp.Play();
                player.GetComponent<Movement>().currentLife = currentHealth;
                Destroy(gameObject);
            }
        }
        catch (MissingReferenceException)
        {
            Debug.Log("Coisinha Morreu");
        }

    }

    private void LayerControl()
    {
        spriteRenderer.sortingOrder = -(int)(transform.position.y * 100);
    }

    void Onlanding()
    {
        rigidbody2D.gravityScale = 0f;
        rigidbody2D.Sleep();
    }
}
