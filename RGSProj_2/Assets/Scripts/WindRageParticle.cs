using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Core;

public class WindRageParticle : MonoBehaviour
{
    [SerializeField] private GameObject WRexplode;
    private Vector2 direction;
    private float speed;
    private float bounceSpeed;
    private float bounceDmg;
    private float dmgPerSec;
    private AttackInfo attractForce;
    private Rigidbody2D rb2D;
    private bool isBounced;
    private Rage_Wind RageWind;
    private float spawnTime;
    private float size;

    private List<BaseEnemy> enemyList=new List<BaseEnemy>();

    // 플레이어와 닿아있는 상태 추적
    private bool isTouchingPlayer = false;

    public Vector2 Init(Rage_Wind RW)
    {
        spawnTime=Time.time;
        RageWind = RW;
        isBounced = false;
        speed = RW.RWSpeed;
        bounceSpeed = RW.RWBounceSpeed;
        bounceDmg = RW.RWBounceDmg;
        dmgPerSec = RW.RWDmgPerSec;
        if (RW.type.typeActiveLevel >= 3)
        {
            size = RW.RWreinforcedSize;
            attractForce = new AttackInfo(0, RW.RWreinforcedForce);
        }
        else
        {
            size = RW.RWSize;
            attractForce = new AttackInfo(0, RW.RWAttractForce);
        }

        transform.localScale = Vector3.one * size;

        direction = GameManager.Instance.player.transform.position- transform.position;
        rb2D.linearVelocity = direction.normalized * speed;
        return direction;
    }

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // 플레이어와 닿아있고, R 키가 눌리면 Bounce 활성화
        if (!isBounced&&isTouchingPlayer && Input.GetKeyDown(KeyCode.R)&&RageWind.type.typeActiveLevel>=4)
        {
            isBounced = true;
            Instantiate(WRexplode, transform.position, Quaternion.identity);
            transform.localScale = Vector3.one * size * 1.5f;
            Vector2 playerDir = GameManager.Instance.player.transform.up;
            rb2D.linearVelocity=playerDir.normalized * bounceSpeed;
        }
        if (Time.time > spawnTime + 6.6f)
        {
            RageWind.RageEnd();
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        StartCoroutine(TickDamageRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator TickDamageRoutine()
    {
        while (true)
        {
            // 1초 기다림
            yield return new WaitForSeconds(0.25f);

            // enemyList에 있는 모든 적에게 1 데미지
            for (int i = 0; i < enemyList.Count; i++)
            {
                BaseEnemy enemy = enemyList[i];
                if (enemy != null&&RageWind.type.typeActiveLevel>1)
                {
                    GameManager.Instance.AtkEnemy(enemy, dmgPerSec, AttackType.MagicAttack,AttackAttr.Wind);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            BaseEnemy BE = collision.GetComponent<BaseEnemy>();
            GameManager.Instance.AtkEnemy(BE, attractForce, AttackType.StaticAttack, AttackAttr.None,transform.position - collision.transform.position);
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isTouchingPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isTouchingPlayer = false;
        }
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            BaseEnemy BE = collision.GetComponent<BaseEnemy>();
            if (enemyList.Contains(BE))
            {
                enemyList.Remove(BE);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            BaseEnemy BE = collision.GetComponent<BaseEnemy>();
            if (!enemyList.Contains(BE))
            {
                enemyList.Add(BE);
            }
            if (isBounced&&RageWind.type.typeActiveLevel>=5)
            {

                GameManager.Instance.AtkEnemy(BE, bounceDmg, AttackType.MagicAttack, AttackAttr.Wind);
            }

        }
    }

}
