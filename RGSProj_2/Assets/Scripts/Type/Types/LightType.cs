using UnityEngine;
using Core;

public class LightType : BaseType
{
    public float[] healAmountPerLevel;

    public override void Upgrade()
    {
        base.Upgrade();
        GameManager.Instance.SM.RemoveModifiersByTag("LightType");
        GameManager.Instance.SM.AddModifier("NaturalHeal", additive: healAmountPerLevel[typePassiveLevel],tag:"LightType");
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameManager.Instance.levelManager.AddLightSkill();
    }
}
