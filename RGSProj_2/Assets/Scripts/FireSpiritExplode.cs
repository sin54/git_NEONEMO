using UnityEngine;

public class FireSpiritExplode : BaseExplosion2
{
    protected override void AttackEnemy(BaseEnemy BE)
    {
        base.AttackEnemy(BE);
        GameManager.instance.AtkEnemy(BE, damageInfo, AttackType.MagicAttack , AttackAttr.Fire,BE.gameObject.transform.position - transform.position);
        BE.eSS.AddFireStack(2);
    }
}
