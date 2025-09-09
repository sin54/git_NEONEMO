using System.Collections.Generic;
using UnityEngine;

public class WindSword_FireExplode : MonoBehaviour
{
    private HashSet<BaseEnemy> enemiesInRange = new HashSet<BaseEnemy>();
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.GetComponent<BaseEnemy>().eSS.AddFireStack(1);
        }
    }
}
