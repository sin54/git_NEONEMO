using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateSetter : MonoBehaviour
{
    [Header("상태별 색상")]
    [SerializeField] private Color buffColor = Color.green;
    [SerializeField] private Color debuffColor = Color.red;

    [Header("UI 공통 프리팹")]
    [SerializeField] private GameObject statePrefab;


    [SerializeField] private SO_EnemyState enemyS;

    [Header("상태 UI 부모")]
    [SerializeField] private Transform content;
    private normalizer_enemyUI normalizer;
    [SerializeField]private BaseEnemy BE;
    private Dictionary<string, GameObject> stateUIs = new Dictionary<string, GameObject>();

    // 상태값
    private float stunTime;
    private float speedScale = 1f;
    private float attackScale = 1f;
    private float defenceScale = 1f;
    [SerializeField]private int fireStack = 0;
    private float shield = 0;
    //private float naturalHeal = 0;

    // 이전값
    private float prevStunTime = -1f;
    private float prevSpeedScale = 1f;
    private float prevAttackScale = 1f;
    private float prevDefenceScale = 1f;
    private int prevFireStack = -1;
    private float prevShield = -1;
    //private float prevNaturalHeal = -1;

    private Coroutine stateCoroutine;

    private void Awake()
    {
        normalizer=GetComponentInChildren<normalizer_enemyUI>();  
    }
    private void OnEnable()
    {
        // 상태 갱신 루프 시작
        if (stateCoroutine == null)
            stateCoroutine = StartCoroutine(StateUpdateLoop());
    }

    private void OnDisable()
    {
        if (stateCoroutine != null)
        {
            StopCoroutine(stateCoroutine);
            stateCoroutine = null;
        }

        // UI 초기화
        foreach (var ui in stateUIs.Values)
        {
            ui.SetActive(false);
        }

        prevStunTime = -1f;
        prevSpeedScale = 1f;
        prevAttackScale = 1f;
        prevDefenceScale = 1f;
        prevFireStack = -1;
        prevShield = -1;

        fireStack = 0;
        attackScale = 1;
        defenceScale = 1;
        speedScale = 1;
        shield = 0;
        stunTime = -0.01f;
    }

    private IEnumerator StateUpdateLoop()
    {
        while (true)
        {
            UpdateStun();
            UpdateSlow();
            UpdateFire();
            UpdateAttackDown();
            UpdateDefenceDown();
            UpdateShield();
            yield return new WaitForSeconds(0.1f);
        }
    }

    #region 상태 갱신

    void UpdateStun()
    {
        if (stunTime > 0f)
        {
            stunTime -= 0.1f;
            if (Mathf.Abs(stunTime - prevStunTime) > 0.05f)
            {
                string text = stunTime.ToString("F1") + "s";
                ShowOrUpdateUI("Stun", text, enemyS.stunSprite,Color.white);
                prevStunTime = stunTime;
            }
        }
        else
        {
            HideUI("Stun");
            prevStunTime = -1f;
        }
    }

    void UpdateSlow()
    {
        if (!Mathf.Approximately(speedScale, 1f))
        {
            if (!Mathf.Approximately(prevSpeedScale, speedScale))
            {
                int percent = Mathf.RoundToInt(Mathf.Abs(1f - speedScale) * 100f);
                string sign = speedScale > 1f ? "+" : "-";
                Color color = speedScale > 1f ? buffColor : debuffColor;
                ShowOrUpdateUI("Slow", $"{sign}{percent}%", enemyS.slowSprite,color);
                prevSpeedScale = speedScale;
            }
        }
        else
        {
            HideUI("Slow");
            prevSpeedScale = 1f;
        }
    }

    void UpdateFire()
    {
        if (fireStack > 0)
        {
            if (prevFireStack != fireStack)
            {
                ShowOrUpdateUI("Fire", $"x{fireStack}", enemyS.fireSprite,Color.white);
                prevFireStack = fireStack;
            }
        }
        else
        {
            HideUI("Fire");
            prevFireStack = -1;
        }
    }

    void UpdateAttackDown()
    {
        if (!Mathf.Approximately(attackScale, 1f))
        {
            if (!Mathf.Approximately(prevAttackScale, attackScale))
            {
                int percent = Mathf.RoundToInt(Mathf.Abs(1f - attackScale) * 100f);
                string sign = attackScale > 1f ? "+" : "-";
                Color color = attackScale > 1f ? buffColor : debuffColor;
                ShowOrUpdateUI("AttackDown", $"{sign}{percent}%", enemyS.attackSprite,color);
                prevAttackScale = attackScale;
            }
        }
        else
        {
            HideUI("AttackDown");
            prevAttackScale = 1f;
        }
    }

    void UpdateDefenceDown()
    {
        if (!Mathf.Approximately(defenceScale, 1f))
        {
            if (!Mathf.Approximately(prevDefenceScale, defenceScale))
            {
                int percent = Mathf.RoundToInt(Mathf.Abs(1f - defenceScale) * 100f);
                string sign = defenceScale > 1f ? "+" : "-";
                Color color = defenceScale > 1f ? buffColor : debuffColor;
                ShowOrUpdateUI("DefenceDown", $"{sign}{percent}%", enemyS.defenceSprite,color);
                prevDefenceScale = defenceScale;
            }
        }
        else
        {
            HideUI("DefenceDown");
            prevDefenceScale = 1f;
        }
    }

    void UpdateShield()
    {
        if (shield > 0)
        {
            if (prevShield!=shield)
            {
                ShowOrUpdateUI("Shield", $"x{Mathf.RoundToInt(shield)}", enemyS.shieldSprite, Color.white);
                prevShield = shield;
            }
        }
        else
        {
            HideUI("Shield");
            prevShield = -1;
        }
    }
    #endregion

    #region UI 관리

    void ShowOrUpdateUI(string key, string text,Sprite icon,Color textColor)
    {
        if (!stateUIs.TryGetValue(key, out GameObject instance))
        {
            instance = Instantiate(statePrefab, content);
            stateUIs[key] = instance;
        }

        instance.SetActive(true);

        var panel = instance.GetComponent<EnemyStatePrefab>();
        panel?.SetPanel(text, icon,textColor);
        normalizer.Normalize();
    }

    void HideUI(string key)
    {
        if (stateUIs.TryGetValue(key, out GameObject instance))
        {
            instance.SetActive(false);
            normalizer.Normalize();
        }
    }

    #endregion

    #region 외부 제어 API

    public void SetFireStack(int amount){
        if (!gameObject.activeInHierarchy) return;
        Execution();
        fireStack = amount;
    }
    public void AddFireStack(int amount){
        if (!gameObject.activeInHierarchy) return;
        Execution();
        fireStack += amount;
    }
    public void MulFireStack(int multiplier){
        if (!gameObject.activeInHierarchy) return;
        Execution();
        fireStack *= multiplier;
    }
    public void SetAttackScale(float amount)
    {
        if (!gameObject.activeInHierarchy) return;
        Execution();
        attackScale = amount;
    }

    public void MulAttackScale(float multiplier)
    {
        if (!gameObject.activeInHierarchy) return;
        Execution();
        attackScale *= multiplier;
    }

    public void SetDefenceScale(float amount)
    {
        if (!gameObject.activeInHierarchy) return;
        defenceScale = amount;
        Execution();
    }

    public void MulDefenceScale(float multiplier)
    {
        if (!gameObject.activeInHierarchy) return;
        defenceScale *= multiplier;
        Execution();
    }

    public void SetSpeedScale(float amount)
    {
        if (!gameObject.activeInHierarchy) return;

        speedScale = amount;
        Execution();
    }

    public void MulSpeedScale(float multiplier)
    {
        if (!gameObject.activeInHierarchy) return;

        speedScale *= multiplier;
        Execution();
    }

    public void AddStunTime(float amount)
    {
        if (!gameObject.activeInHierarchy) return;
        stunTime += amount;
        Execution();
    }

    public void AddShield(float amount)
    {
        if (!gameObject.activeInHierarchy) return;
        shield += amount;
        Execution();
    }

    public void ReduceShield(float amount)
    {
        if (!gameObject.activeInHierarchy) return;
        shield -= amount;
        Execution();
    }

    public void SetShield(float amount)
    {
        if (!gameObject.activeInHierarchy) return;
        shield = amount;
        Execution();
    }


    public float GetStunTime()
    {
        return stunTime;
    }
    public int GetFireStack()
    {
        return fireStack;
    }
    public float GetAttackScale()
    {
        return attackScale;
    }
    public float GetSpeedScale()
    {
        return speedScale;
    }
    public float GetDefenceScale()
    {
        return defenceScale;
    }
    public float GetShield()
    {
        return shield;
    }

    #endregion

    private void Execution()
    {
        BE.canExecute();
    }
}
