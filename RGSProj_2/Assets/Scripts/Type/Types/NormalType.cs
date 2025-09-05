using UnityEngine;
using Core;

public class NormalType : BaseType
{
    public float[] increaseXPmul;

    public override void Upgrade()
    {
        base.Upgrade();
        GameManager.Instance.SM.RemoveModifiersByTag("NormalType");
        GameManager.Instance.SM.AddModifier("xpMul", additive: increaseXPmul[typePassiveLevel],tag:"NormalType");
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }
}
