using UnityEngine;

public class NormalType : BaseType
{
    public float[] increaseXPmul;

    public override void Upgrade()
    {
        base.Upgrade();
        GameManager.instance.SM.RemoveModifiersByTag("NormalType");
        GameManager.instance.SM.AddModifier("xpMul", additive: increaseXPmul[typePassiveLevel],tag:"NormalType");
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }
}
