using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParams
{
    public int SpriteLayer { get; set; }
    public int Damage { get; set; }
    public float AtkPositionX { get; set; }
    public int hitStun { get; set; }
    public bool heavyHit { get; set; }
    public bool special { get; set; }

    public HitParams(int sl, int dmg, float at, int hs, bool hh, bool sp)
    {
        SpriteLayer = sl;
        Damage = dmg;
        AtkPositionX = at;
        hitStun = hs;
        heavyHit = hh;
        special = sp;
    }
}
