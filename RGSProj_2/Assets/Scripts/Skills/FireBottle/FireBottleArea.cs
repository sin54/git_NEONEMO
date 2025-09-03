using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FireBottleArea : MonoBehaviour
{
    private float spawnTime = 0f;
    private float damage;
    private float radius;
    private float duration;
    private Skill_FireBottle FB;

    private float damageTimer = 0f;
    public List<BaseEnemy> enemiesInRange = new List<BaseEnemy>();
    private ParticleSystem PS;
    [SerializeField]private ParticleSystem PS2;

    private void Awake()
    {
        PS = GetComponent<ParticleSystem>();
    }
    private void OnEnable()
    {
        spawnTime = Time.time;
        enemiesInRange.Clear();
    }
    void Update()
    {
        if (FB.reinforcedNum == 2)
        {
            transform.localScale = new Vector3(radius, radius, radius);
        }

        if (Time.time > spawnTime + duration)
        {
            gameObject.SetActive(false);
        }

        damageTimer += Time.deltaTime;


        if (damageTimer >= 0.75f)
        {
            foreach (BaseEnemy enemy in enemiesInRange.ToList())
            {
                if (enemy != null)
                {
                    if (enemy.gameObject.activeSelf)
                    {
                        GameManager.instance.AtkEnemy(enemy, damage, AttackType.MagicAttack, AttackAttr.Fire);
                        enemy.eSS.AddFireStack(1);
                    }
                    else
                    {
                        enemiesInRange.Remove(enemy);
                    }

                }
            }

            damageTimer = 0f;
        }
    }
    private void FixedUpdate()
    {
        if (FB.reinforcedNum == 2)
        {
            radius += FB.increasingAmount;
        }
    }
    public void Init(float dur, float dmg, float size, Skill_FireBottle fb)
    {
        PS.Stop();
        duration = dur*GameManager.instance.SM.GetFinalValue("SkillDurationMul");
        damage = dmg;
        radius = size*GameManager.instance.SM.GetFinalValue("AoESize");
        if (fb.reinforcedNum == 3)
        {
            float eqRad = fb.equalRadius * GameManager.instance.SM.GetFinalValue("AoESize");
            transform.localScale = new Vector3(eqRad,eqRad,eqRad);
        }
        else
        {
            transform.localScale = new Vector3(radius, radius, radius);
        }

        FB = fb;
        var pMain = PS.main;
        pMain.duration=duration;
        pMain.startLifetime = duration;
        var pMain2 = PS2.main;
        pMain2.duration = duration;
        pMain2.startLifetime = duration;
        PS.Play();
        PS2.Play();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {

            BaseEnemy enemy = other.GetComponent<BaseEnemy>();
            if (enemy != null && !enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Add(enemy);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            BaseEnemy enemy = other.GetComponent<BaseEnemy>();
            if (enemy != null && enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Remove(enemy);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (FB.reinforcedNum==1)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                collision.gameObject.GetComponent<BaseEnemy>().HasAttacked(new AttackInfo(0, FB.centripetalForce), transform.position - collision.gameObject.transform.position,AttackType.StaticAttack,false);
            }
        }
    }
}
