using UnityEngine;
using Core;

public class EnemyExplodeEffect : MonoBehaviour
{
    public Transform boomParent;
    private float radius;
    private float damage;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.Instance.player.DecreaseHealth(damage);
        }
    }
    public void DisableExplode()
    {
        boomParent.gameObject.SetActive(false);
    }
    public virtual void SetExplosion(float rad, float dmg, Vector3 boomPos)
    {
        radius = rad;
        damage = dmg;
        gameObject.transform.parent.localScale = new Vector3(radius, radius, radius);
        transform.position = boomPos;
    }
}
