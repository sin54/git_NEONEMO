using System.Collections.Generic;
using UnityEngine;

public class LightFieldParticle : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Rage_Light RL;
    public void ApplySlow()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);

        foreach (var hit in hits)
        {
            BaseEnemy enemy = hit.GetComponent<BaseEnemy>();
            if (enemy != null)
            {
                enemy.eSS.MulSpeedScale(RL.reduceSpeedMul);
            }
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.layer == LayerMask.NameToLayer("Player"))
        {
            Player player = other.GetComponentInChildren<Player>();
            player.IncreaseHealth(player.playerMaxHealth * RL.increaseHealthMul);
        }
    }
}
