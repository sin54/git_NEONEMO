using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[System.Serializable]
public struct AttackInfo
{
    public float damage;
    public float knockbackPower;
    public AttackInfo(float v1,float v2)
    {
        damage = v1; knockbackPower = v2;
    }
    public AttackInfo(float v1)
    {
        damage = v1;
        knockbackPower = 0f;
    }
}

[Flags]
[System.Serializable]
public enum AttackType
{
    MagicAttack = 1,    // 0001
    PhysicAttack = 2,   // 0010
    StaticAttack = 4    // 0100
}

[System.Flags]
[System.Serializable]
public enum AttackAttr
{
    None = 0,
    Normal = 1 << 0, // 1
    Fire = 1 << 1, // 2
    Light = 1 << 2, // 4
    Wind = 1 << 3, // 8
    Ice = 1 << 4, // 16
    Dark = 1 << 5, // 32
    Mech = 1 << 6  // 64
}