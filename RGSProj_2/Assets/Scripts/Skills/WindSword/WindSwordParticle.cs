using System.Collections.Generic;
using UnityEngine;
using Core;

public class WindSwordParticle : MonoBehaviour
{
    private Skill_WindSword SW;
    private float damage;
    private ParticleSystem PS;
    private void OnEnable()
    {
    }
    public void SetParticle(float dmg, Skill_WindSword sw)
    {
        damage = dmg;
        SW = sw;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        BaseEnemy BE = collision.GetComponent<BaseEnemy>();
        if (BE != null)
        {
            if (SW.reinforcedNum == 1)
            {
                if (BE.eSS.GetSpeedScale() < 1)
                {
                    GameManager.Instance.AtkEnemy(BE,damage*SW.atkMul_rf1,AttackType.PhysicAttack,AttackAttr.Wind);
                }
                else
                {
                    GameManager.Instance.AtkEnemy(BE, damage, AttackType.PhysicAttack,AttackAttr.Wind);
                }
            }
            else if (SW.reinforcedNum == 2)
            {
                GameManager.Instance.AtkEnemy(BE, SW.damage_rf2, AttackType.PhysicAttack, AttackAttr.Wind);
            }
            else if (SW.reinforcedNum == 3)
            {
                bool isFire = BE.eSS.GetFireStack() > 0;
                bool isDead = GameManager.Instance.AtkEnemy(BE, damage, AttackType.PhysicAttack,AttackAttr.Wind);
                if (isFire&&isDead)
                {
                    GameObject GO = GameManager.Instance.poolManager.Get(40);
                    GO.transform.position = transform.position;
                }
            }
            else
            {
                GameManager.Instance.AtkEnemy(BE, damage, AttackType.PhysicAttack, AttackAttr.Wind);
            }

        }
    }
}
