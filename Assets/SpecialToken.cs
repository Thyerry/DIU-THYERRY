using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialToken : MonoBehaviour
{
    Movement playerScript;
    Image specialToken;
    void Awake()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
        specialToken = GetComponent<Image>();
    }
    // Update is called once per frame
    void Update()
    {
        specialToken.color = playerScript.specialToken 
            ? new Color(specialToken.color.r, specialToken.color.g, specialToken.color.b, 1)
            : new Color(specialToken.color.r, specialToken.color.g, specialToken.color.b, 0);
    }
}
