using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using Random = UnityEngine.Random;
public class RepairManager : MonoBehaviour
{
    [SerializeField] private GameObject gameStartPanel;
    [SerializeField] private TMP_Text ReadyTxt;

    [SerializeField] private GameObject target;

    [SerializeField] private float gameTime;
    [SerializeField] private int numOfTarget;

    [SerializeField] private GameObject explodeParticle;
    [SerializeField] private GameObject healParticle;

    [SerializeField] private Image leftTime;
    [SerializeField] private TMP_Text leftTimeTxt;

    [SerializeField] private int healAmount;
    [SerializeField] private int maxHealAmount;
    [SerializeField] private Image leftHealthImg;
    [SerializeField] private Transform spawnFloatingPos;

    [SerializeField] private Gradient[] explodeGradient;

    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TMP_Text resultHitTxt;
    [SerializeField] private TMP_Text resultRepairTxt;
    [SerializeField] private TMP_Text resultHealTxt;
    [SerializeField] private TMP_Text resultMaxTxt;
    private int resultHit;
    private int resultRepair;
    private int resultHeal;
    private int resultMax;

    private UnityEvent increaseHealth;
    public static event Action OnGameFinished;
    private bool isGaming;
    private int leftNum;

    private float nowGameTime;
    public void RepairStart(float currentHP)
    {
        UpdateHealth();
        resultHit = 0;
        resultRepair=0;
        resultHeal = 0;
        resultMax = 0;
        gameStartPanel.SetActive(true);
        resultPanel.SetActive(false);
        StartCoroutine(GameStart());
    }

    IEnumerator GameStart()
    {
        ReadyTxt.text = "우주선을 수리하세요!";
        yield return new WaitForSeconds(1f);
        for(int i = 3; i > 0; i--)
        {
            ReadyTxt.text=i.ToString();
            yield return new WaitForSeconds(1f);
        }
        ReadyTxt.text = "시작!";
        yield return new WaitForSeconds(1f);
        isGaming = true;
        gameStartPanel.SetActive(false);
        for(int i = 0; i < numOfTarget; i++)
        {
            SpawnEnemy();
        }
        nowGameTime = gameTime;
    }
    private void Update()
    {
        if (isGaming)
        {
            nowGameTime -= Time.deltaTime;
            leftTime.fillAmount = nowGameTime / gameTime;
            leftTimeTxt.text = nowGameTime.ToString("F2");
            if (nowGameTime <= 0)
            {
                isGaming = false;
                leftTimeTxt.text = "00:00";
                StartCoroutine(FinishGame());
            }
        }
    }

    private IEnumerator FinishGame()
    {
        OnGameFinished?.Invoke();
        yield return new WaitForSeconds(1.5f);
        resultHitTxt.text = resultHit.ToString();
        resultRepairTxt.text = resultRepair.ToString(); 
        resultMaxTxt.text = "+"+resultMax.ToString();
        resultHealTxt.text = "+"+resultHeal.ToString();
        resultHitTxt.gameObject.SetActive(false);
        resultRepairTxt.gameObject.SetActive(false);
        resultMaxTxt.gameObject.SetActive(false);
        resultHealTxt.gameObject.SetActive(false);
        resultPanel.SetActive(true);
        yield return WaitForMouseClick();
        resultHitTxt.gameObject.SetActive(true);
        yield return WaitForMouseClick();
        resultRepairTxt.gameObject.SetActive(true);
        yield return WaitForMouseClick();
        resultHealTxt.gameObject.SetActive(true);
        yield return WaitForMouseClick();
        resultMaxTxt.gameObject.SetActive(true);
        yield return WaitForMouseClick();
        GameManager.instance.EndRepair();
    }
    IEnumerator WaitForMouseClick()
    {
        // 마우스 누르고 있는 중이면 기다린다 (버튼이 떨어질 때까지)
        yield return new WaitUntil(() => !Input.GetMouseButton(0));

        // 다시 눌릴 때까지 기다림 (정확한 "클릭" 감지)
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
    }
    public void SpawnEnemy()
    {
        Vector2 randPoint = new Vector2(Random.Range(2f, 6f), Random.Range(1f, 3.5f));
        Vector2 startPoint = new Vector2(Random.Range(0.01f, 0.2f), Random.Range(0.01f, 0.2f));
        if (Random.value < 0.5f)
        {
            randPoint.x *= -1;
            startPoint.x *= -1;
        }
        if (Random.value < 0.5f)
        {
            randPoint.y *= -1;
            startPoint.y *= -1;
        }
        GameObject GO=Instantiate(target,Vector2.zero,Quaternion.identity);
        GO.GetComponent<LineARInitalizer>().InitObj(randPoint, startPoint, Random.Range(1, 4));

    }

    public void SpawnParticles(Vector2 pos)
    {
        resultRepair++;
        Instantiate(healParticle,pos, Quaternion.identity);
    }
    public void SpawnExplodeParticles(int hp,Vector2 pos)
    {
        resultHit++;
        ParticleSystem PS=explodeParticle.GetComponent<ParticleSystem>();
        var PSGrad = PS.colorOverLifetime;
        PSGrad.color = explodeGradient[hp];
        Instantiate(explodeParticle, pos, Quaternion.identity);
    }
    public void HealPlayer()
    {
        GameObject floatingTxt = GameManager.instance.poolManager.Get(3);
        floatingTxt.transform.position = spawnFloatingPos.position;
        floatingTxt.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-10, 10));
        TMP_Text floatingTxt_TMP = floatingTxt.GetComponentInChildren<TextMeshPro>();

        if (GameManager.instance.player.playerCurrentHealth >= GameManager.instance.player.playerMaxHealth)
        {
            GameManager.instance.player.IncreaseMaxHealth(maxHealAmount);
            floatingTxt_TMP.text = "+" + maxHealAmount.ToString();
            floatingTxt_TMP.fontSize = 6.5f;
            floatingTxt_TMP.color = Color.red;
            resultMax += maxHealAmount;
        }
        else
        {
            GameManager.instance.player.IncreaseHealth(healAmount);
            floatingTxt_TMP.text = "+" + healAmount.ToString();
            floatingTxt_TMP.fontSize = 6.5f;
            floatingTxt_TMP.color = Color.green;
            resultHeal += healAmount;
        }

        UpdateHealth();
    }
    public void UpdateHealth()
    {
        leftHealthImg.fillAmount=GameManager.instance.player.playerCurrentHealth/GameManager.instance.player.playerMaxHealth;
    }
}
