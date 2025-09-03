using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoomEffect : BaseExplosion2
{
    protected override void AttackEnemy(BaseEnemy BE)
    {
        base.AttackEnemy(BE);
        GameManager.instance.AtkEnemy(BE, damageInfo, AttackType.PhysicAttack,AttackAttr.Normal,BE.gameObject.transform.position - transform.position);
    }
}
