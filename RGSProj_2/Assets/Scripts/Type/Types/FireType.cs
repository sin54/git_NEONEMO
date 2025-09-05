using UnityEngine;
using Core;
public class FireType : BaseType
{
    public float[] fireTick;

    public override void Upgrade()
    {
        base.Upgrade();
        GameManager.Instance.enemyFireTick = fireTick[typePassiveLevel];
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameManager.Instance.enemyFireTick = fireTick[0];
        GameManager.Instance.levelManager.AddFireSkill();
    }
}
