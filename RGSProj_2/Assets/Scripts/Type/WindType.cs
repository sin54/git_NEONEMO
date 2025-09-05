using UnityEngine;
using Core;

public class WindType : BaseType
{
    public float[] increasingSpeed;
    public float[] knockbackMul;

    public override void Upgrade()
    {
        base.Upgrade();
        GameManager.Instance.SM.RemoveModifiersByTag("WindType");
        GameManager.Instance.SM.AddModifier("PlayerSpeed", additive: increasingSpeed[typePassiveLevel], duration: 0f,tag:"WindType");
        GameManager.Instance.SM.AddModifier("KnockBackMul", additive: knockbackMul[typePassiveLevel], duration: 0f, tag: "WindType");
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameManager.Instance.levelManager.AddWindSkill();
    }
}
