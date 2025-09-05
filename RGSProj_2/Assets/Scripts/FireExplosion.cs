using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Core;

public class FireExplosion : BaseExplosion
{
    private int fireS;
    public void SetExplosion(float rad, float dmg,int fireStack,float time)
    {
        radius = rad * GameManager.Instance.SM.GetFinalValue("explosionRad");
        damageInfo = new AttackInfo(dmg);
        gameObject.transform.localScale = new Vector3(radius, radius, radius);
        fireS=fireStack;
        Invoke("BoomAttack", time);
    }

    public override void AttackEnemy(BaseEnemy BE)
    {
        base.AttackEnemy(BE);
        GameManager.Instance.AtkEnemy(BE, damageInfo, AttackType.MagicAttack, AttackAttr.Fire, BE.gameObject.transform.position - transform.position);
        BE.eSS.AddFireStack(fireS);
    }

}
