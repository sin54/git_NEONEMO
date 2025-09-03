using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireTrail : MonoBehaviour
{
    private float damageTimer = 0f;
    public List<BaseEnemy> enemiesInRange = new List<BaseEnemy>();
    public Animator AC;
    private void Awake()
    {
        AC = GetComponent<Animator>();
    }
    void Update()
    {
        damageTimer += Time.deltaTime;


        if (damageTimer >= 0.3f)
        {
            foreach (BaseEnemy enemy in enemiesInRange.ToList())
            {
                if (enemy != null)
                {
                    if (GameManager.instance.playerTypeManager.BT.typeCode == 1)
                    {
                        enemy.eSS.AddFireStack(1);
                    }

                }
            }

            damageTimer = 0f;
        }
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
}
