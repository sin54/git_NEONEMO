using UnityEngine;

public class LightType : BaseType
{
    public float[] healAmountPerLevel;

    public override void Upgrade()
    {
        base.Upgrade();
        GameManager.instance.SM.RemoveModifiersByTag("LightType");
        GameManager.instance.SM.AddModifier("NaturalHeal", additive: healAmountPerLevel[typePassiveLevel],tag:"LightType");
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameManager.instance.levelManager.AddLightSkill();
    }
}
