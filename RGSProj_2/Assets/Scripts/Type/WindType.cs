using UnityEngine;

public class WindType : BaseType
{
    public float[] increasingSpeed;
    public float[] knockbackMul;

    public override void Upgrade()
    {
        base.Upgrade();
        GameManager.instance.SM.RemoveModifiersByTag("WindType");
        GameManager.instance.SM.AddModifier("PlayerSpeed", additive: increasingSpeed[typePassiveLevel], duration: 0f,tag:"WindType");
        GameManager.instance.SM.AddModifier("KnockBackMul", additive: knockbackMul[typePassiveLevel], duration: 0f, tag: "WindType");
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameManager.instance.levelManager.AddWindSkill();
    }
}
