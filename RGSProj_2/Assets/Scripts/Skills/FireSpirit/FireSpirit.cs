using UnityEngine;
using System.Collections;

public class FireSpirit : MonoBehaviour
{
    public Skill_FireSpirit SF;
    private SpinAround SA;
    private float lastAttackTime;

    private void Awake()
    {
        SA=GetComponent<SpinAround>();
    }

    private void Update()
    {
        if (SF.reinforcedNum == 1)
        {
            if (Time.time > lastAttackTime + (SF.skillData.breathCount[SF.itemLevel])*0.15f)
            {
                lastAttackTime = Time.time;
                StartCoroutine(Attack());
            }
        }
        else if (SF.reinforcedNum == 2)
        {
            if (Time.time > lastAttackTime + SF.spearCool* GameManager.instance.SM.GetFinalValue("CoolReduce")* GameManager.instance.SM.GetFinalValue("F_Cool"))
            {
                lastAttackTime = Time.time;
                StartCoroutine(Attack());
            }
        }
        else if (SF.reinforcedNum == 3)
        {
            if (Time.time > lastAttackTime + SF.explodeCool* GameManager.instance.SM.GetFinalValue("CoolReduce") * GameManager.instance.SM.GetFinalValue("F_Cool"))
            {
                lastAttackTime = Time.time;
                StartCoroutine(Attack());
            }
        }
        else
        {
            if (Time.time > lastAttackTime + SF.skillData.breathCool[SF.itemLevel]* GameManager.instance.SM.GetFinalValue("CoolReduce") * GameManager.instance.SM.GetFinalValue("F_Cool"))
            {
                lastAttackTime = Time.time;
                StartCoroutine(Attack());
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            GameObject GO = GameManager.instance.poolManager.Get(42);
            GO.transform.GetChild(0).GetComponent<BaseExplosion2>().SetExplosion(SF.skillData.boomRadByLevel[SF.itemLevel], new AttackInfo(SF.skillData.boomDmgByLevel[SF.itemLevel], 0),transform.position);
        }
    }

    private IEnumerator Attack()
    {
        if (SF.reinforcedNum == 2)
        {
            GameObject GO = GameManager.instance.poolManager.Get(34);

            Vector2 originalVector = new Vector2(transform.position.x - GameManager.instance.player.gameObject.transform.position.x, transform.position.y - GameManager.instance.player.gameObject.transform.position.y);
            GO.transform.position = transform.position + (Vector3)originalVector * 0.5f;
            GO.transform.GetChild(0).GetComponent<SpiritSpearBullet>().SetBullet(SF.skillData.bulletSpeed, 10f, originalVector,SF);
        }
        else if (SF.reinforcedNum == 3)
        {
            for(int i = 0; i < SF.numOfAttack; i++)
            {
                GameObject GO = GameManager.instance.poolManager.Get(22);
                Vector2 originalVector = new Vector2(Mathf.Cos(Mathf.Deg2Rad*(360*i/SF.numOfAttack)),Mathf.Sin(Mathf.Deg2Rad*(360 * i / SF.numOfAttack)));
                GO.transform.position = transform.position;
                GO.transform.GetChild(0).GetComponent<SpiritBreathBullet>().SetBullet(SF.skillData.bulletSpeed, 3.5f, originalVector);
                GO.GetComponent<Rigidbody2D>().gravityScale = 0f;
            }
        }
        else
        {
            for (int i = 0; i < SF.skillData.breathCount[SF.itemLevel]; i++)
            {
                GameObject GO = GameManager.instance.poolManager.Get(22);

                Vector2 originalVector = new Vector2(transform.position.x - GameManager.instance.player.gameObject.transform.position.x, transform.position.y - GameManager.instance.player.gameObject.transform.position.y);
                GO.transform.position = transform.position + (Vector3)originalVector * 0.5f;
                GO.transform.GetChild(0).GetComponent<SpiritBreathBullet>().SetBullet(SF.skillData.bulletSpeed, 3.5f, originalVector);

                yield return new WaitForSeconds(SF.reinforcedNum == 1 ? 0.15f : 0.1f);
            }
        }
    }
}
