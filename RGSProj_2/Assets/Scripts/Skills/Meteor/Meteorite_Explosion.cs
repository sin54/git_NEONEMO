using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Meteorite_Explosion : BaseExplosion
{
    private Skill_Meteor SM;
    public override void AttackEnemy(BaseEnemy BE)
    {
        if (SM.reinforcedNum == 3)
        {
            GameManager.instance.AtkEnemy(BE, new AttackInfo(BE.maxHealth * SM.damagePercent, damageInfo.knockbackPower), AttackType.PhysicAttack,AttackAttr.Fire ,BE.gameObject.transform.position - transform.position);
        }
        else
        {
            GameManager.instance.AtkEnemy(BE, damageInfo, AttackType.PhysicAttack, AttackAttr.Fire,BE.gameObject.transform.position - transform.position);
        }

        if (SM.reinforcedNum == 2)
        {
            BE.eSS.AddStunTime(SM.stunTime);
        }
    }

    public override void SetExplode(float rad, AttackInfo dmg, float time, BaseSkill BS)
    {
        base.SetExplode(rad, dmg, time, BS);
        SM = (Skill_Meteor)BS;
    }
}
