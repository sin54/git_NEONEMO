using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Core;

public class StarExplode : BaseExplosion2
{
    protected override void AttackEnemy(BaseEnemy BE)
    {
        base.AttackEnemy(BE);
        GameManager.Instance.AtkEnemy(BE, damageInfo, AttackType.MagicAttack, AttackAttr.Light,BE.gameObject.transform.position - transform.position);
        BE.eSS.MulSpeedScale(0.9f);
    }
}
