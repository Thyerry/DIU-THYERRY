using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using System;

public class ScoreController : MonoBehaviour
{
    [SerializeField]
    Movement playerScript;
    [SerializeField]
    List<Image> ScoreDigits;
    [SerializeField]
    Sprite[] numbers;

    void Update()
    {
        int[] scoreArray = playerScript.score.ToString().Reverse().Select(n => Int32.Parse(n.ToString())).ToArray();

        for (int i = 0; i < scoreArray.Length; i++)
        {
            ScoreDigits[i].sprite = numbers[scoreArray[i]];
        }
    }
}
