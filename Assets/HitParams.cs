using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParams
{
    public int SpriteLayer { get; set; }
    public int Damage { get; set; }
    public Transform AtkTransform { get; set; }
    public bool heavyHit { get; set; }

    public HitParams(int sl, int dmg, Transform at, bool hh)
    {
        SpriteLayer = sl;
        Damage = dmg;
        AtkTransform = at;
        heavyHit = hh;
    }
}
