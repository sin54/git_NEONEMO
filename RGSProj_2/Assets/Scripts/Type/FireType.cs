using UnityEngine;

public class FireType : BaseType
{
    public float[] fireTick;

    public override void Upgrade()
    {
        base.Upgrade();
        GameManager.instance.enemyFireTick = fireTick[typePassiveLevel];
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameManager.instance.enemyFireTick = fireTick[0];
        GameManager.instance.levelManager.AddFireSkill();
    }
}
