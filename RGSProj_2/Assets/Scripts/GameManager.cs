using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using InventorySystem;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PoolManager poolManager;
    public LevelManager levelManager;
    public Scanner scanner;
    public Player player;
    public Rigidbody2D playerRB;
    public CamManager CM;
    public ScreenScan screenScan;
    public PlayerTypeManager playerTypeManager;
    public UIManager_GameScene UIM;
    public StatsManager SM;
    public Spawner Spawner;

    [SerializeField] private Camera mainCam;
    [SerializeField] private TMP_Text levelLeftTxt;
    [SerializeField] private TMP_Text levelRightTxt;
    [SerializeField] private TMP_Text timerTxt;
    [SerializeField] private Image gameOverPanel;

    [SerializeField] private Button[] buttons;

    public float borderRadius;
    [SerializeField] private int segmentNumber;
    public float gameTime { get; private set; }
    public int killedEnemy;
    [SerializeField] private int maxLevel;
    public Vector2 scanRange;

    public float enemyFireTick;

    public Color[] floatingTxtColors = new Color[7];

    //[SerializeField] private bool isDay;
    private bool isTokenUse;
    [SerializeField] private GameObject dayPanel;
    [SerializeField] private GameObject nightPanel;

    [SerializeField] private GameObject[] turnOffInNight;
    
    public bool isDay { get;private set; }

    [Header("Reinforce")]
    private int[] reinforceData = new int[8];
    private readonly List<ConditionEntry> executeConditions = new();
    private class ConditionEntry
    {
        public string Tag;
        public Func<BaseEnemy, bool> Condition;

        public ConditionEntry(string tag, Func<BaseEnemy, bool> condition)
        {
            Tag = tag;
            Condition = condition;
        }
    }

    private void Awake()
    {
        Application.targetFrameRate = 120;
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        isDay = false;
        enemyFireTick = 1.25f;
        gameTime = 0f;
        levelLeftTxt.text = player.nowLevel.ToString();
        levelRightTxt.text = (player.nowLevel+1).ToString();
        timerTxt.text = "00:00";
        CreateCollider();
    }
    private void Update()
    {
        gameTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(gameTime / 60f);
        int seconds = Mathf.FloorToInt(gameTime % 60f);

        timerTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(player.transform.position, scanRange);
    }
    #region NightManager
    public void LevelUp()
    {
        levelLeftTxt.text = player.nowLevel.ToString();
        levelRightTxt.text = (player.nowLevel + 1).ToString();
        levelManager.LevelUp();
    }

    private void CreateCollider()
    {
        EdgeCollider2D edge = gameObject.GetComponent<EdgeCollider2D>();

        int segments = segmentNumber;
        float radius = borderRadius;
        Vector2[] points = new Vector2[segments + 1];

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * Mathf.PI * 2 / segments;
            points[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
        }

        edge.points = points;
    }

    public IEnumerator OnGameOver()
    {
        TimeScaleManager.Instance.TimeStopStackPlus();
        for(int i = 0; i < 100; i++)
        {
            gameOverPanel.color = new Color(0, 0, 0, i / 100.0f);
            yield return new WaitForSecondsRealtime(0.02f);
        }
        TimeScaleManager.Instance.TimeStopStackMinus();
        Loader.LoadWithNoLoadingScene(Loader.Scene.GameOver);
    }
    #endregion
    #region EnemyControl
    private float CalculateFinalDmg(float dmg, AttackType attacktype, AttackAttr attackattr, out bool isCrit)
    {
        bool isC = UtilClass.GetPercent(SM.GetFinalValue("CriticalPercent"));
        isCrit = isC;

        float baseDmg = dmg * SM.GetFinalValue("AtkMul");
        float criticalDmg = isC ? baseDmg * SM.GetFinalValue("CriticalMul") : baseDmg;
        float finalDmg = criticalDmg;

        if ((attacktype & AttackType.StaticAttack) != 0)
        {
            isCrit = false;
            return dmg;
        }
        if ((attacktype & AttackType.PhysicAttack) != 0)
        {
            finalDmg *= SM.GetFinalValue("P_AtkMul");
        }
        if ((attacktype & AttackType.MagicAttack) != 0)
        {
            finalDmg *= SM.GetFinalValue("M_AtkMul");
        }

        // AttackAttr 별 배율 적용
        foreach (AttackAttr attr in System.Enum.GetValues(typeof(AttackAttr)))
        {
            if (attr == AttackAttr.None) continue; // 0은 무시
            if ((attackattr & attr) != 0)
            {
                // enum 이름을 문자열로 그대로 사용
                string key = attr.ToString() + "_AtkMul";
                finalDmg *= SM.GetFinalValue(key);
            }
        }

        return finalDmg;
    }
    private AttackInfo CalculateFinalDmg(AttackInfo attackInfo, AttackType attacktype, AttackAttr attackattr, out bool isCrit)
    {
        bool isC = UtilClass.GetPercent(SM.GetFinalValue("CriticalPercent"));
        isCrit = isC;

        float baseDmg = attackInfo.damage * SM.GetFinalValue("AtkMul");
        float criticalDmg = isC ? baseDmg * SM.GetFinalValue("CriticalMul") : baseDmg;
        float finalDmg = criticalDmg;

        // StaticAttack은 치명타 적용 안 함, 그대로 리턴
        if ((attacktype & AttackType.StaticAttack) != 0)
        {
            isCrit = false;
            return attackInfo;
        }

        // 타입별 배율
        if ((attacktype & AttackType.PhysicAttack) != 0)
        {
            finalDmg *= SM.GetFinalValue("P_AtkMul");
        }
        if ((attacktype & AttackType.MagicAttack) != 0)
        {
            finalDmg *= SM.GetFinalValue("M_AtkMul");
        }

        // 속성별 배율
        foreach (AttackAttr attr in System.Enum.GetValues(typeof(AttackAttr)))
        {
            if (attr == AttackAttr.None) continue; // 0은 무시
            if ((attackattr & attr) != 0)
            {
                string key = attr.ToString() + "_AtkMul";
                finalDmg *= SM.GetFinalValue(key);
            }
        }

        return new AttackInfo(finalDmg, attackInfo.knockbackPower);
    }
    public bool AtkEnemy(BaseEnemy BE, float dmg,AttackType AT,AttackAttr AA)
    {
        bool isC = false;
        float finalDmg = CalculateFinalDmg(dmg, AT,AA, out isC);
        return BE.HasAttacked(finalDmg, AT, isC);
    }
    public bool AtkEnemy(BaseEnemy BE, AttackInfo AIm, AttackType AT,AttackAttr AA,Vector2 KnockbackDir) {
        bool isC = false;
        AttackInfo attackInfo=CalculateFinalDmg(AIm, AT, AA,out isC);
        return BE.HasAttacked(attackInfo, KnockbackDir, AT, isC);
    }
    public void AddExeCondition(string tag, Func<BaseEnemy, bool> condition)
    {
        executeConditions.Add(new ConditionEntry(tag, condition));
    }

    // 특정 태그 조건 전체 제거
    public void RemoveExeCondition(string tag)
    {
        executeConditions.RemoveAll(c => c.Tag == tag);
    }

    // 적이 처형 가능한지 확인
    public bool CanExecute(BaseEnemy enemy)
    {
        if (executeConditions.Count == 0)
            return false; // 조건이 없으면 처형 불가

        foreach (var entry in executeConditions)
            if (!entry.Condition(enemy))
                return false;
        return true;
    }
    #endregion

    #region DayNightCycle
    public void ChangeToDay()
    {
        UpdateButton(true);
        isDay = true;

        isTokenUse = false;
        dayPanel.SetActive(true);
        nightPanel.SetActive(false);
        for (int i = 0; i < turnOffInNight.Length; i++) {
            turnOffInNight[i].SetActive(false);
        }
        player.transform.parent.position = new Vector2(0, 0);
        playerRB.linearVelocity = new Vector2(0, 0);
        poolManager.DeActiveAllChild();

    }
    public void StartNextWave()
    {
        Spawner.EndDay();
        dayPanel.SetActive(false);
        nightPanel.SetActive(true);
        isDay = false;
        for (int i = 0; i < turnOffInNight.Length; i++)
        {
            turnOffInNight[i].SetActive(true);
        }
    }
    #endregion
#region DayManager
    public void StartRepair()
    {
        if (isTokenUse) return;
        isTokenUse=true;
        dayPanel.SetActive(false);
        mainCam.enabled = false;

        SceneManager.LoadSceneAsync("RepairScene", LoadSceneMode.Additive).completed += (op) =>
        {
            GameObject miniCam = GameObject.Find("MiniGameCamera");
            RepairManager Rm = GameObject.Find("RepairManager").GetComponent<RepairManager>();
            if (miniCam != null)
            {
                miniCam.GetComponent<Camera>().enabled = true;
            }
            else
            {
                Debug.LogWarning("MiniGameCamera not found!");
            }
            Rm.RepairStart(player.playerCurrentHealth);
        };
    }
    public void EndRepair()
    {
        GameObject.Find("MiniGameCamera").GetComponent<Camera>().enabled = false;
        mainCam.enabled = true;
        SceneManager.UnloadSceneAsync("RepairScene");
        UpdateButton(false);
        dayPanel.SetActive(true);
    }

    public void StartReinforce()
    {
        if (isTokenUse) return;
        isTokenUse = true;
        dayPanel.SetActive(false);
        mainCam.enabled = false;

        SceneManager.LoadSceneAsync("ReinforceScene", LoadSceneMode.Additive).completed += (op) =>
        {
            GameObject miniCam = GameObject.Find("MiniGameCamera");
            ReinforceManager Rm = GameObject.Find("reinforceManager").GetComponent<ReinforceManager>();
            if (miniCam != null)
            {
                miniCam.GetComponent<Camera>().enabled = true;
            }
            else
            {
                Debug.LogWarning("MiniGameCamera not found!");
            }
            Rm.StartReinforce(reinforceData);
        };
    }

    public void EndReinforce(int[] data)
    {
        reinforceData = (int[])data.Clone();
        GameObject.Find("MiniGameCamera").GetComponent<Camera>().enabled = false;
        mainCam.enabled = true;
        SceneManager.UnloadSceneAsync("ReinforceScene");
        UpdateButton(false);
        dayPanel.SetActive(true);
    }

    public void StartCollect()
    {
        if (isTokenUse) return;
        isTokenUse = true;
        dayPanel.SetActive(false);
        mainCam.enabled = false;

        SceneManager.LoadSceneAsync("CollectScene", LoadSceneMode.Additive).completed += (op) =>
        {
            GameObject miniCam = GameObject.Find("MiniGameCamera");
            CollectSceneManager Cm = GameObject.Find("CollectSceneManager").GetComponent<CollectSceneManager>();
            if (miniCam != null)
            {
                miniCam.GetComponent<Camera>().enabled = true;
            }
            else
            {
                Debug.LogWarning("MiniGameCamera not found!");
            }
            
        };
    }

    public void EndCollect()
    {

    }
    private void UpdateButton(bool active)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = active;
        }
    }
    #endregion
}
