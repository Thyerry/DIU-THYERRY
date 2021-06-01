using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ActionCode.ColorPalettes;

public class EnemyColor : MonoBehaviour
{
    [SerializeField]
    ColorPaletteSwapperCycle swapper;
    // Start is called before the first frame update
    void Awake()
    {
        swapper = GetComponent<ColorPaletteSwapperCycle>();
        InvokeRepeating("", 0, 0.0001f);
    }

    void podeFazerSwap()
    {

    }
}
