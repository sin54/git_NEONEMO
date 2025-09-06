using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using InventorySystem;
using Core;
using Scenes;
using Item;
using UI;
using Type;
using UI.LevelUp;

namespace Core
{
    /// <summary>
    /// ���� ������ ���¿� �ֿ� �Ŵ��� ������Ʈ�� �̱������� �����մϴ�.
    /// ���ø����̼� ���� �� �� �� ���� �����Ǹ�, �� ��ȯ �� �ı����� �ʽ��ϴ�.
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public class GameManager : MonoBehaviour
    {
        #region Singleton

        /// <summary>
        /// GameManager �̱��� �ν��Ͻ��Դϴ�.
        /// </summary>
        public static GameManager Instance { get; private set; }

        #endregion

        #region Variables


        /// <summary>Ǯ�� �Ŵ��� ����</summary>
        public PoolManager poolManager;

        /// <summary>���� ���� �Ŵ��� ����</summary>
        public LevelManager levelManager;

        /// <summary>��ĳ��(�ֺ� ������Ʈ Ž��) ����</summary>
        public Scanner scanner;

        /// <summary>�÷��̾� ��ũ��Ʈ ����</summary>
        public Player player;

        /// <summary>�÷��̾� Rigidbody2D ����</summary>
        public Rigidbody2D playerRB;

        /// <summary>ī�޶� �Ŵ��� ����</summary>
        public CamManager CM;

        /// <summary>ȭ�� ��ĵ �Ŵ��� ����</summary>
        public ScreenScan screenScan;

        /// <summary>�÷��̾� Ÿ�� �Ŵ��� ����</summary>
        public PlayerTypeManager playerTypeManager;

        /// <summary>���� �� UI �Ŵ��� ����</summary>
        public UIManager_GameScene UIM;

        /// <summary>���� ���� �Ŵ��� ����</summary>
        public StatsManager SM;

        /// <summary>������(�� ������) ����</summary>
        public Spawner Spawner;

        [Header("UI References")]
        [SerializeField] private Camera mainCam;
        [SerializeField] private TMP_Text levelLeftTxt;
        [SerializeField] private TMP_Text levelRightTxt;
        [SerializeField] private TMP_Text timerTxt;
        [SerializeField] private Image gameOverPanel;
        [SerializeField] private Button[] buttons;

        [Header("Game Settings")]
        [Tooltip("�� ��� �ݰ�")]
        public float borderRadius;
        [Tooltip("�׵θ� ���׸�Ʈ ����")]
        [SerializeField] private int segmentNumber;
        public float gameTime { get; private set; }
        public int killedEnemy;
        [Tooltip("�ִ� ����")]
        [SerializeField] private int maxLevel;
        [Tooltip("��ĵ ���� ũ��")]
        public Vector2 scanRange;
        public float enemyFireTick;
        [Tooltip("�÷��� �ؽ�Ʈ �÷� �迭")]
        public Color[] floatingTxtColors = new Color[7];

        [Header("Day/Night")]
        private bool isTokenUse;
        [SerializeField] private GameObject dayPanel;
        [SerializeField] private GameObject nightPanel;
        [SerializeField] private GameObject[] turnOffInNight;
        /// <summary>���� ������ ����</summary>
        public bool isDay { get; private set; }

        [Header("Reinforce Settings")]
        private int[] reinforceData = new int[8];
        private readonly List<ConditionEntry> executeConditions = new();

        /// <summary>
        /// ��ȭ ���� ��Ʈ��: �±׿� ���� �Լ��� �����մϴ�.
        /// </summary>
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

        #endregion

        #region Unity Callbacks

        /// <summary>
        /// �̱��� �ν��Ͻ� �ʱ�ȭ �� �ߺ� ����, �� ��ȯ �� �ı� ���� ó��.
        /// </summary>
        private void Awake()
        {
            Application.targetFrameRate = 120;

            if (Instance == null)
            {
                Instance = this;
                if (transform.parent != null)
                    transform.SetParent(null);

                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// ���� ���� �� �ʱ� ���� ���� �� UI, �浹ü(Collider) ����.
        /// </summary>
        private void Start()
        {
            isDay = false;
            enemyFireTick = 1.25f;
            gameTime = 0f;
            levelLeftTxt.text = player.nowLevel.ToString();
            levelRightTxt.text = (player.nowLevel + 1).ToString();
            timerTxt.text = "00:00";

            CreateCollider();
        }

        /// <summary>
        /// �� ������ ���� �ð��� �����ϰ� Ÿ�̸� UI�� ������Ʈ�մϴ�.
        /// </summary>
        private void Update()
        {
            gameTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(gameTime / 60f);
            int seconds = Mathf.FloorToInt(gameTime % 60f);
            timerTxt.text = $"{minutes:00}:{seconds:00}";
        }

        /// <summary>
        /// ������ ��忡�� ��ĵ ������ �ð�ȭ�ϴ� Gizmo�� �׸��ϴ�.
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(player.transform.position, scanRange);
        }

        #endregion

        #region NightManager


        /// <summary>
        /// ������ ó���� UI ���� �ؽ�Ʈ ������ �����մϴ�.
        /// </summary>
        public void LevelUp()
        {
            levelLeftTxt.text = player.nowLevel.ToString();
            levelRightTxt.text = (player.nowLevel + 1).ToString();
            levelManager.LevelUp();
        }

        /// <summary>
        /// ��� Collider�� �������� �����Ͽ� �� �ܰ��� �����մϴ�.
        /// </summary>
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

        /// <summary>
        /// ���ӿ��� �� ���̵����� �����ϰ� GameOver ������ ��ȯ�մϴ�.
        /// </summary>
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


        /// <summary>
        /// ���� Ÿ�� �� �Ӽ�, ũ��Ƽ���� �ݿ��� ���� �������� ����մϴ�.
        /// </summary>
        /// <param name="dmg">�⺻ ������</param>
        /// <param name="attackType">���� Ÿ�� �÷���</param>
        /// <param name="attackAttr">���� �Ӽ� �÷���</param>
        /// <param name="isCrit">ũ��Ƽ�� ���� ���</param>
        /// <returns>���� ������ ������</returns>
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

            // AttackAttr �� ���� ����
            foreach (AttackAttr attr in System.Enum.GetValues(typeof(AttackAttr)))
            {
                if (attr == AttackAttr.None) continue; // 0�� ����
                if ((attackattr & attr) != 0)
                {
                    // enum �̸��� ���ڿ��� �״�� ���
                    string key = attr.ToString() + "_AtkMul";
                    finalDmg *= SM.GetFinalValue(key);
                }
            }

            return finalDmg;
        }

        /// <summary>
        /// AttackInfo ������� ġ��Ÿ, Ÿ�ԡ��Ӽ� ������ �����Ͽ� ���� ���� ������ ����մϴ�.
        /// </summary>
        /// <param name="attackInfo">���� ���� ���� (damage, knockbackPower)</param>
        /// <param name="attackType">���� Ÿ�� �÷���</param>
        /// <param name="attackAttr">���� �Ӽ� �÷���</param>
        /// <param name="isCrit">ġ��Ÿ ���� ��� �Ķ����</param>
        /// <returns>������ ���� AttackInfo</returns>
        private AttackInfo CalculateFinalDmg(AttackInfo attackInfo, AttackType attacktype, AttackAttr attackattr, out bool isCrit)
        {
            bool isC = UtilClass.GetPercent(SM.GetFinalValue("CriticalPercent"));
            isCrit = isC;

            float baseDmg = attackInfo.damage * SM.GetFinalValue("AtkMul");
            float criticalDmg = isC ? baseDmg * SM.GetFinalValue("CriticalMul") : baseDmg;
            float finalDmg = criticalDmg;

            // StaticAttack�� ġ��Ÿ ���� �� ��, �״�� ����
            if ((attacktype & AttackType.StaticAttack) != 0)
            {
                isCrit = false;
                return attackInfo;
            }

            // Ÿ�Ժ� ����
            if ((attacktype & AttackType.PhysicAttack) != 0)
            {
                finalDmg *= SM.GetFinalValue("P_AtkMul");
            }
            if ((attacktype & AttackType.MagicAttack) != 0)
            {
                finalDmg *= SM.GetFinalValue("M_AtkMul");
            }

            // �Ӽ��� ����
            foreach (AttackAttr attr in System.Enum.GetValues(typeof(AttackAttr)))
            {
                if (attr == AttackAttr.None) continue; // 0�� ����
                if ((attackattr & attr) != 0)
                {
                    string key = attr.ToString() + "_AtkMul";
                    finalDmg *= SM.GetFinalValue(key);
                }
            }

            return new AttackInfo(finalDmg, attackInfo.knockbackPower);
        }

        /// <summary>
        /// ���� ������ ������ ������ ������ ���մϴ�.
        /// </summary>
        /// <param name="enemy">���� ��� ��</param>
        /// <param name="damage">������ ��</param>
        /// <param name="attackType">���� Ÿ��</param>
        /// <param name="attackAttr">���� �Ӽ�</param>
        /// <returns>���� ���� ����</returns>
        public bool AtkEnemy(BaseEnemy BE, float dmg,AttackType AT,AttackAttr AA)
        {
            bool isC = false;
            float finalDmg = CalculateFinalDmg(dmg, AT,AA, out isC);
            return BE.HasAttacked(finalDmg, AT, isC);
        }

        /// <summary>
        /// AttackInfo�� �˹� ������ �����Ͽ� ������ ������ ���մϴ�.
        /// </summary>
        /// <param name="enemy">���� ��� ��</param>
        /// <param name="attackInfo">�⺻ ���� ���� (damage, knockbackPower)</param>
        /// <param name="attackType">���� Ÿ��</param>
        /// <param name="attackAttr">���� �Ӽ�</param>
        /// <param name="knockbackDir">�˹� ����</param>
        /// <returns>���� ���� ����</returns>
        public bool AtkEnemy(BaseEnemy BE, AttackInfo AIm, AttackType AT,AttackAttr AA,Vector2 KnockbackDir) {
            bool isC = false;
            AttackInfo attackInfo=CalculateFinalDmg(AIm, AT, AA,out isC);
            return BE.HasAttacked(attackInfo, KnockbackDir, AT, isC);
        }

        /// <summary>
        /// �� ó��(Execute) ������ �߰��մϴ�.
        /// </summary>
        /// <param name="tag">���� �ĺ��� �±�</param>
        /// <param name="condition">���� �Լ�</param>
        public void AddExeCondition(string tag, Func<BaseEnemy, bool> condition)
        {
            executeConditions.Add(new ConditionEntry(tag, condition));
        }

        /// <summary>
        /// Ư�� �±׷� ��ϵ� ó�� ������ ��� �����մϴ�.
        /// </summary>
        /// <param name="tag">������ ���� �±�</param>
        public void RemoveExeCondition(string tag)
        {
            executeConditions.RemoveAll(c => c.Tag == tag);
        }

        /// <summary>
        /// ���� ��ϵ� ��� ó�� ������ �����ϴ��� �˻��մϴ�.
        /// </summary>
        /// <param name="enemy">�˻� ��� ��</param>
        /// <returns>��� ������ ���̸� true, �ƴϸ� false</returns>
        public bool CanExecute(BaseEnemy enemy)
        {
            if (executeConditions.Count == 0)
                return false; // ������ ������ ó�� �Ұ�

            foreach (var entry in executeConditions)
                if (!entry.Condition(enemy))
                    return false;
            return true;
        }
        #endregion

        #region DayNightCycle

        /// <summary>
        /// �ְ� ���� ��ȯ�մϴ�.
        /// ��ư ���¸� ������Ʈ�ϰ�, �� UI�� Ȱ��ȭ�մϴ�.
        /// </summary>
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

            // �÷��̾� ��ġ �� �ӵ� ����
            player.transform.parent.position = new Vector2(0, 0);
            playerRB.linearVelocity = new Vector2(0, 0);

            // ��� Ǯ ������Ʈ ��Ȱ��ȭ
            poolManager.DeActiveAllChild();

        }

        /// <summary>
        /// �߰� ���̺긦 �����մϴ�.
        /// �����ʿ� EndDay ȣ��, �� UI�� Ȱ��ȭ�մϴ�.
        /// </summary>
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

        /// <summary>
        /// ���� �̴ϰ����� �����մϴ�.
        /// �̴ϰ��� ȭ���� �ε��ϰ�, ī�޶� ��ȯ�մϴ�.
        /// </summary>
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

        /// <summary>
        /// ���� �̴ϰ����� �����մϴ�.
        /// ī�޶� ���� �� �� ��ε� �� ��ư ���� ������Ʈ.
        /// </summary>
        public void EndRepair()
        {
            GameObject.Find("MiniGameCamera").GetComponent<Camera>().enabled = false;
            mainCam.enabled = true;
            SceneManager.UnloadSceneAsync("RepairScene");
            UpdateButton(false);
            dayPanel.SetActive(true);
        }

        /// <summary>
        /// ��ȭ �̴ϰ����� �����մϴ�.
        /// �̴ϰ��� ȭ���� �ε��ϰ�, ī�޶� ��ȯ�մϴ�.
        /// </summary>
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

        /// <summary>
        /// ��ȭ �̴ϰ����� �����մϴ�.
        /// �� ��ȭ ������ ����, ī�޶� ���� �� �� ��ε� �� ��ư ���� ������Ʈ.
        /// </summary>
        /// <param name="data">���Ӱ� ����� ��ȭ ������</param>
        public void EndReinforce(int[] data)
        {
            reinforceData = (int[])data.Clone();
            GameObject.Find("MiniGameCamera").GetComponent<Camera>().enabled = false;
            mainCam.enabled = true;
            SceneManager.UnloadSceneAsync("ReinforceScene");
            UpdateButton(false);
            dayPanel.SetActive(true);
        }

        /// <summary>
        /// ���� �̴ϰ����� �����մϴ�.
        /// �̴ϰ��� ȭ���� �ε��ϰ�, ī�޶� ��ȯ�մϴ�.
        /// </summary>
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

        /// <summary>
        /// ���� �̴ϰ����� �����մϴ�.
        /// </summary>
        public void EndCollect(List<InventoryItem> collectedItems)
        {
            GameObject.Find("MiniGameCamera").GetComponent<Camera>().enabled = false;
            mainCam.enabled = true;

            SceneManager.UnloadSceneAsync("CollectScene");
            UpdateButton(false);
            dayPanel.SetActive(true);
            foreach (InventoryItem item in collectedItems)
            {
                InventoryController.instance.AddPassiveItem(item.GetItemType());
            }
        }

        /// <summary>
        /// UI ��ư ��ȣ�ۿ� ���� ���θ� ������Ʈ�մϴ�.
        /// </summary>
        /// <param name="active">��ư Ȱ��ȭ ����</param>
        private void UpdateButton(bool active)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = active;
            }
        }

        #endregion
    }
}


