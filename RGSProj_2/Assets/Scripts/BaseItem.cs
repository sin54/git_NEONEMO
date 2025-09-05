using UnityEngine;
using Core;

public class BaseItem : MonoBehaviour
{
    public bool isCollected;
    private Rigidbody2D RB2D;
    private Vector2 angle;
    private float collectspeed = 5f;
    private float spawnTime;
    private bool isSpawnCollectedBanned;

    protected virtual void Awake()
    {
        RB2D = GetComponent<Rigidbody2D>();
    }

    protected virtual void OnEnable()
    {
        spawnTime=Time.time;
        isSpawnCollectedBanned = true;
        Spawner.OnEndDay += DisableThis;
    }

    protected virtual void OnDisable()
    {
        Spawner.OnEndDay -= DisableThis;
    }
    protected virtual void Update()
    {
        if (Time.time > spawnTime + 0.02f&&isSpawnCollectedBanned)
        {
            isCollected = false;
            isSpawnCollectedBanned = false;
        }
        if (isCollected&& Time.time > spawnTime + 0.02f)
        {
            angle = new Vector2(GameManager.Instance.player.transform.position.x - transform.position.x, GameManager.Instance.player.transform.position.y - transform.position.y).normalized;
        }
    }
    protected virtual void FixedUpdate()
    {
        if (isCollected)
        {
            RB2D.linearVelocity = angle * collectspeed;
        }
        else
        {
            RB2D.linearVelocity = Vector2.zero;
        }
    }
    public virtual void Collect()
    {
        isCollected = true;
    }

    protected virtual void Active()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")|| collision.gameObject.layer == LayerMask.NameToLayer("DashLayer"))
        {
            Active();
            gameObject.SetActive(false);
        }
    }

    private void DisableThis()
    {
        gameObject.SetActive(false);
    }
}
