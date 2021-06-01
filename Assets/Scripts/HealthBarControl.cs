using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarControl : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    float health;
    float maxHealth;
    float currentHealth;
    Slider slider;
    void Start()
    {
        maxHealth = (float)player.GetComponent<Character>().MaxHealth;
        slider = GetComponent<Slider>();
    }
    // Update is called once per frame
    void Update()
    {
        try
        {
            currentHealth = (float)player.GetComponent<Character>().currentLife;
            float result = currentHealth / maxHealth;
            slider.value = result;
        }
        catch (MissingReferenceException)
        {
            return;
        }

    }
}
