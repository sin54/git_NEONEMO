using UnityEngine;
using Core;

public class FireSpiritExplode : BaseExplosion2
{
    protected override void AttackEnemy(BaseEnemy BE)
    {
        base.AttackEnemy(BE);
        GameManager.Instance.AtkEnemy(BE, damageInfo, AttackType.MagicAttack , AttackAttr.Fire,BE.gameObject.transform.position - transform.position);
        BE.eSS.AddFireStack(2);
    }
}
