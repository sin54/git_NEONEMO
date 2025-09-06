using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class SpiritAreaAttack : MonoBehaviour
{
    private float spawnTime = 0f;
    private float damage;
    private float radius;
    private float duration;
    private Skill_FireSpirit SI;
    private List<BaseEnemy> enemiesInRange = new List<BaseEnemy>();

    private void OnEnable()
    {
        spawnTime = Time.time;
    }
    void Update()
    {
        if (Time.time > spawnTime + 0.02f)
        {
            foreach (BaseEnemy enemy in enemiesInRange.ToList())
            {
                if (enemy != null)
                {
                }
            }
        }

        if (Time.time > spawnTime + 0.5f)
        {
            gameObject.SetActive(false);
        }
    }
    public void Init(float dmg, float size, Skill_FireSpirit si)
    {
        damage = dmg;
        radius = size;
        transform.localScale = new Vector3(radius, radius, radius);
        SI = si;

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
