﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParams : MonoBehaviour
{
    public int SpriteLayer { get; set; }
    public int Damage { get; set; }
    public Transform AtkTransform { get; set; }

    public HitParams(int sl, int dmg, Transform at)
    {
        SpriteLayer = sl;
        Damage = dmg;
        AtkTransform = at;
    }
}
