using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Laser : MonoBehaviour
{
    private float damage;
    private float dur;
    private Skill_Laser SL;
    private Animator myAnim;
    [SerializeField] private Animator sourceAnim;

    private HashSet<BaseEnemy> damagedEnemies = new HashSet<BaseEnemy>();

    private void Awake()
    {
        myAnim = GetComponent<Animator>();
    }
    public void DisableLaser()
    {
        damagedEnemies.Clear();
        transform.parent.gameObject.SetActive(false);
    }
    public void SetLaser(float dmg,float duration,Skill_Laser sl)
    {
        if (sl.reinforcedNum == 3)
        {
            transform.position = GameManager.instance.player.transform.position;
        }

        damage = dmg;
        dur= duration;
        myAnim.SetFloat("Multiplier", duration);
        sourceAnim.SetFloat("Multiplier", duration);
        SL = sl;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            BaseEnemy enemy = collision.GetComponent<BaseEnemy>();
            if (enemy != null && !damagedEnemies.Contains(enemy))
            {
                // 데미지 1번만 적용
                GameManager.instance.AtkEnemy(enemy, damage, AttackType.MagicAttack, AttackAttr.Light);
                damagedEnemies.Add(enemy);

                if (SL.reinforcedNum == 2)
                {
                    if (GameManager.instance.playerTypeManager.NowType.Contains(1))
                    {
                        enemy.eSS.AddFireStack(5);
                    }
                }
            }
        }
    }
    private void Update()
    {
        if (SL.reinforcedNum == 3)
        {
            transform.position = GameManager.instance.player.transform.position;
        }
    }
    private void FixedUpdate()
    {

        if (SL.reinforcedNum == 3)
        {
            float zRotation = transform.eulerAngles.z + SL.angularVelocity * Time.fixedDeltaTime;
            transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
        }
    }
}
