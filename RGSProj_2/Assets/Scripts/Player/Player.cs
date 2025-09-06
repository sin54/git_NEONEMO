using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Core;

public class Player : MonoBehaviour
{
    private Rigidbody2D RB;
    private SpriteRenderer SR;
    private Skill_Dash SD;

    [SerializeField] private Image hpBar;
    [SerializeField] private Image xpBar;
    [SerializeField] private Image hitImg;
    [SerializeField] private Transform missObjPos;
    private Coroutine fadeCoroutine;

    public Vector2 direction { get; private set; }

    public float playerMaxHealth;
    public Color playerColor;

    public bool canDash;


    public float playerCurrentHealth;


    [SerializeField] private float noMoveRange = 0.5f;

    public float xp;
    public int nowLevel = 1;
    private int baseXP = 10;
    private float growthRate = 1.2f;
    [SerializeField]private int needXP = 0;
    [SerializeField] private LayerMask itemLayer;

    public bool isPlayerDeath;
    public bool noDamage = false;

    public List<int> typeList = new List<int>();
    
    public float playerFinalSpeed { get; private set; }
    private void Awake()
    {
        RB = GetComponentInParent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();

        SD = GetComponent<Skill_Dash>();
    }

    private void Start()
    {
        isPlayerDeath = false;
        canDash = true;
        SetColor(playerColor);
        playerCurrentHealth = playerMaxHealth;
        gameObject.layer = 7;
        needXP= Mathf.FloorToInt(baseXP * Mathf.Pow(growthRate, nowLevel));
    }
    void Update()
    {
        playerFinalSpeed = GameManager.Instance.SM.GetFinalValue("PlayerSpeed");
        PlayerMove();
        GetXPOrb();

    }

    private void FixedUpdate()
    {
        if (!SD.isPlayerDashing)
        {
            
            SetVelocity(playerFinalSpeed, direction);
        }
    }
    private void PlayerMove()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        if ((mousePosition - transform.position).magnitude < noMoveRange)
        {
            direction = Vector2.zero;
        }
        else
        {
            direction = (mousePosition - transform.position).normalized;
        }



        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (!SD.isPlayerDashing)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }

    }

    private void GetXPOrb()
    {
        Collider2D[] RH=Physics2D.OverlapCircleAll(transform.position,GameManager.Instance.SM.GetFinalValue("ItemRange"), itemLayer);
        foreach (Collider2D r in RH) {
            BaseItem BI=r.GetComponent<BaseItem>();
            if (!BI.isCollected)
            {
                BI.Collect();
            }
        }
    }

    public void AddXP(int amount)
    {
        xp +=amount*GameManager.Instance.SM.GetFinalValue("xpMul");
        if (xp >= needXP)
        {
            LevelUp();
        }
        xpBar.fillAmount = (float)xp / (float)needXP;

    }

    private void LevelUp()
    {
        nowLevel++;
        xp -= needXP;
        needXP = Mathf.FloorToInt(baseXP * Mathf.Pow(growthRate, nowLevel));
        Debug.Log("LVUP!");
        GameManager.Instance.LevelUp();
    }


    public void DecreaseHealth(float amount)
    {
        if (noDamage) return;
        if (UtilClass.GetPercent(1-GameManager.Instance.SM.GetFinalValue("dodgeMul")))
        {
            GameObject missObj = GameManager.Instance.poolManager.Get(41);
            missObj.transform.position = new Vector3(missObjPos.position.x+Random.Range(-0.1f,0.1f),missObjPos.position.y,missObjPos.position.z);
            missObj.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-13, 13));
            return;
        }
        float finalDmg = amount * GameManager.Instance.SM.GetFinalValue("defenceRate");
        playerCurrentHealth -= finalDmg;
        hpBar.fillAmount = playerCurrentHealth / playerMaxHealth;
        SpawnFloatingTxt(finalDmg);
        Color c = hitImg.color;
        c.a = Mathf.Clamp01(c.a + 1.5f*finalDmg/playerMaxHealth); // 일정 비율만큼 보이게
        hitImg.color = c;
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeOut());
        if (playerCurrentHealth <= 0)
        {
            Death();
        }
    }
    private void SpawnFloatingTxt(float damage)
    {
        if (damage <= 0) return;
        GameObject floatingTxt = GameManager.Instance.poolManager.Get(3);
        floatingTxt.transform.position = new Vector3(missObjPos.position.x + Random.Range(-0.1f, 0.1f), missObjPos.position.y, missObjPos.position.z);
        floatingTxt.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-13, 13));
        TMP_Text floatingTxt_TMP = floatingTxt.GetComponentInChildren<TextMeshPro>();
        floatingTxt_TMP.color = Color.red;
        floatingTxt_TMP.text = (Mathf.RoundToInt(damage)).ToString();
        floatingTxt_TMP.fontSize = Mathf.Clamp(damage/2, 2f, 7);
    }
    public void Death()
    {
        isPlayerDeath = true;
        StartCoroutine(GameManager.Instance.OnGameOver());
    }
    public void IncreaseHealth(float amount)
    {
        playerCurrentHealth += amount;
        if (playerCurrentHealth > playerMaxHealth) playerCurrentHealth = playerMaxHealth;
        hpBar.fillAmount = playerCurrentHealth / playerMaxHealth;
    }
    public void IncreaseMaxHealth(int amount)
    {
        float prevPercentage=playerCurrentHealth/playerMaxHealth;
        playerMaxHealth += amount;
        playerCurrentHealth = prevPercentage * playerMaxHealth;
        hpBar.fillAmount = playerCurrentHealth / playerMaxHealth;
    }
    public void DecreaseMaxHealth(int amount)
    {
        float prevPercentage = playerCurrentHealth / playerMaxHealth;
        playerMaxHealth -= amount;
        if (playerMaxHealth <= 0) playerMaxHealth = 1;
        playerCurrentHealth=prevPercentage * playerMaxHealth;
        hpBar.fillAmount = playerCurrentHealth / playerMaxHealth;
    }
    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(1f);
        Color c = hitImg.color;

        while (c.a > 0f)
        {
            c.a -= Time.deltaTime * 0.3f; // 1.5초 동안 사라지게
            hitImg.color = c;
            yield return null;
        }

        // 완전히 사라지면 코루틴 정리
        fadeCoroutine = null;
    }
    #region SetFunc
    public void SetVelocity(float speed,Vector2 direction)
    {
        RB.linearVelocity=new Vector2(speed*direction.x,speed*direction.y);
    }
    public void SetVelocityZero()
    {
        RB.linearVelocity=Vector2.zero;
    }
    private void SetColor(Color color)
    {
        SR.material.color = color;
    }
    #endregion

    private IEnumerator Heal()
    {
        IncreaseHealth(GameManager.Instance.SM.GetFinalValue("NaturalHeal"));
        yield return new WaitForSeconds(1f);
    }
}
