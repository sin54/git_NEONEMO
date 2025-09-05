using System.Collections.Generic;
using UnityEngine;
using Core;

public class WindKnockback : MonoBehaviour
{
    private Skill_Knockback SK;
    private List<Collider2D> enemiesInRange = new List<Collider2D>();
    private int fireStacks;
    private void Awake()
    {
        SK=GetComponentInParent<Skill_Knockback>();
    }
    private void OnEnable()
    {
        fireStacks = 0;
        enemiesInRange.Clear();
    }
    public void ActiveFalse()
    {
        if (SK.reinforcedNum == 1)
        {
            for (int i = 0; i < enemiesInRange.Count; i++)
            {
                enemiesInRange[i].GetComponent<BaseEnemy>().eSS.SetFireStack(fireStacks);
            }

        }
        else if (SK.reinforcedNum == 3)
        {
            GameManager.Instance.SM.AddModifier("defenceRate", multiplier: SK.def_rf3,duration:SK.buffTime_rf3);
            GameManager.Instance.SM.AddModifier("AtkMul", multiplier: SK.atk_rf3, duration: SK.buffTime_rf3);
            Invoke("InitEffect", SK.buffTime_rf3);
        }
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (!enemiesInRange.Contains(collision))
            {
                enemiesInRange.Add(collision);
                GameManager.Instance.AtkEnemy(collision.GetComponent<BaseEnemy>(), SK.skillData.attackInfoByLevel[SK.itemLevel].damage,AttackType.PhysicAttack,AttackAttr.Wind);
                if (SK.reinforcedNum == 1)
                {
                    int fs = collision.GetComponent<BaseEnemy>().eSS.GetFireStack();
                    if (fs > fireStacks)
                    {
                        fireStacks = fs;
                    }
                }
            }

        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            GameManager.Instance.AtkEnemy(collision.GetComponent<BaseEnemy>(), new AttackInfo(0, SK.skillData.attackInfoByLevel[SK.itemLevel].knockbackPower),AttackType.StaticAttack, AttackAttr.None,collision.transform.position - transform.position);
        }
    }
}
