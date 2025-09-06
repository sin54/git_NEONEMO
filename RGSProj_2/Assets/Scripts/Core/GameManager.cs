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
    /// 게임 전반의 상태와 주요 매니저 컴포넌트를 싱글톤으로 관리합니다.
    /// 애플리케이션 실행 시 단 한 번만 생성되며, 씬 전환 시 파괴되지 않습니다.
    /// </summary>
    [DefaultExecutionOrder(-100)]
    public class GameManager : MonoBehaviour
    {
        #region Singleton

        /// <summary>
        /// GameManager 싱글톤 인스턴스입니다.
        /// </summary>
        public static GameManager Instance { get; private set; }

        #endregion

        #region Variables


        /// <summary>풀링 매니저 참조</summary>
        public PoolManager poolManager;

        /// <summary>레벨 관리 매니저 참조</summary>
        public LevelManager levelManager;

        /// <summary>스캐너(주변 오브젝트 탐지) 참조</summary>
        public Scanner scanner;

        /// <summary>플레이어 스크립트 참조</summary>
        public Player player;

        /// <summary>플레이어 Rigidbody2D 참조</summary>
        public Rigidbody2D playerRB;

        /// <summary>카메라 매니저 참조</summary>
        public CamManager CM;

        /// <summary>화면 스캔 매니저 참조</summary>
        public ScreenScan screenScan;

        /// <summary>플레이어 타입 매니저 참조</summary>
        public PlayerTypeManager playerTypeManager;

        /// <summary>게임 씬 UI 매니저 참조</summary>
        public UIManager_GameScene UIM;

        /// <summary>스탯 관리 매니저 참조</summary>
        public StatsManager SM;

        /// <summary>스포너(적 생성기) 참조</summary>
        public Spawner Spawner;

        [Header("UI References")]
        [SerializeField] private Camera mainCam;
        [SerializeField] private TMP_Text levelLeftTxt;
        [SerializeField] private TMP_Text levelRightTxt;
        [SerializeField] private TMP_Text timerTxt;
        [SerializeField] private Image gameOverPanel;
        [SerializeField] private Button[] buttons;

        [Header("Game Settings")]
        [Tooltip("맵 경계 반경")]
        public float borderRadius;
        [Tooltip("테두리 세그먼트 개수")]
        [SerializeField] private int segmentNumber;
        public float gameTime { get; private set; }
        public int killedEnemy;
        [Tooltip("최대 레벨")]
        [SerializeField] private int maxLevel;
        [Tooltip("스캔 범위 크기")]
        public Vector2 scanRange;
        public float enemyFireTick;
        [Tooltip("플로팅 텍스트 컬러 배열")]
        public Color[] floatingTxtColors = new Color[7];

        [Header("Day/Night")]
        private bool isTokenUse;
        [SerializeField] private GameObject dayPanel;
        [SerializeField] private GameObject nightPanel;
        [SerializeField] private GameObject[] turnOffInNight;
        /// <summary>현재 낮인지 여부</summary>
        public bool isDay { get; private set; }

        [Header("Reinforce Settings")]
        private int[] reinforceData = new int[8];
        private readonly List<ConditionEntry> executeConditions = new();

        /// <summary>
        /// 강화 조건 엔트리: 태그와 판정 함수를 보관합니다.
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
        /// 싱글톤 인스턴스 초기화 및 중복 제거, 씬 전환 시 파괴 방지 처리.
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
        /// 게임 시작 시 초기 변수 세팅 및 UI, 충돌체(Collider) 생성.
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
        /// 매 프레임 게임 시간을 갱신하고 타이머 UI를 업데이트합니다.
        /// </summary>
        private void Update()
        {
            gameTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(gameTime / 60f);
            int seconds = Mathf.FloorToInt(gameTime % 60f);
            timerTxt.text = $"{minutes:00}:{seconds:00}";
        }

        /// <summary>
        /// 에디터 모드에서 스캔 범위를 시각화하는 Gizmo를 그립니다.
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(player.transform.position, scanRange);
        }

        #endregion

        #region NightManager


        /// <summary>
        /// 레벨업 처리와 UI 레벨 텍스트 갱신을 수행합니다.
        /// </summary>
        public void LevelUp()
        {
            levelLeftTxt.text = player.nowLevel.ToString();
            levelRightTxt.text = (player.nowLevel + 1).ToString();
            levelManager.LevelUp();
        }

        /// <summary>
        /// 경계 Collider를 원형으로 생성하여 맵 외곽을 설정합니다.
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
        /// 게임오버 시 페이드인을 진행하고 GameOver 씬으로 전환합니다.
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
        /// 공격 타입 및 속성, 크리티컬을 반영한 최종 데미지를 계산합니다.
        /// </summary>
        /// <param name="dmg">기본 데미지</param>
        /// <param name="attackType">공격 타입 플래그</param>
        /// <param name="attackAttr">공격 속성 플래그</param>
        /// <param name="isCrit">크리티컬 여부 출력</param>
        /// <returns>최종 보정된 데미지</returns>
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

        /// <summary>
        /// AttackInfo 기반으로 치명타, 타입·속성 배율을 적용하여 최종 공격 정보를 계산합니다.
        /// </summary>
        /// <param name="attackInfo">원본 공격 정보 (damage, knockbackPower)</param>
        /// <param name="attackType">공격 타입 플래그</param>
        /// <param name="attackAttr">공격 속성 플래그</param>
        /// <param name="isCrit">치명타 여부 출력 파라미터</param>
        /// <returns>보정된 최종 AttackInfo</returns>
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

        /// <summary>
        /// 단일 데미지 값으로 적에게 공격을 가합니다.
        /// </summary>
        /// <param name="enemy">공격 대상 적</param>
        /// <param name="damage">데미지 값</param>
        /// <param name="attackType">공격 타입</param>
        /// <param name="attackAttr">공격 속성</param>
        /// <returns>공격 성공 여부</returns>
        public bool AtkEnemy(BaseEnemy BE, float dmg,AttackType AT,AttackAttr AA)
        {
            bool isC = false;
            float finalDmg = CalculateFinalDmg(dmg, AT,AA, out isC);
            return BE.HasAttacked(finalDmg, AT, isC);
        }

        /// <summary>
        /// AttackInfo와 넉백 방향을 포함하여 적에게 공격을 가합니다.
        /// </summary>
        /// <param name="enemy">공격 대상 적</param>
        /// <param name="attackInfo">기본 공격 정보 (damage, knockbackPower)</param>
        /// <param name="attackType">공격 타입</param>
        /// <param name="attackAttr">공격 속성</param>
        /// <param name="knockbackDir">넉백 방향</param>
        /// <returns>공격 성공 여부</returns>
        public bool AtkEnemy(BaseEnemy BE, AttackInfo AIm, AttackType AT,AttackAttr AA,Vector2 KnockbackDir) {
            bool isC = false;
            AttackInfo attackInfo=CalculateFinalDmg(AIm, AT, AA,out isC);
            return BE.HasAttacked(attackInfo, KnockbackDir, AT, isC);
        }

        /// <summary>
        /// 적 처형(Execute) 조건을 추가합니다.
        /// </summary>
        /// <param name="tag">조건 식별용 태그</param>
        /// <param name="condition">조건 함수</param>
        public void AddExeCondition(string tag, Func<BaseEnemy, bool> condition)
        {
            executeConditions.Add(new ConditionEntry(tag, condition));
        }

        /// <summary>
        /// 특정 태그로 등록된 처형 조건을 모두 제거합니다.
        /// </summary>
        /// <param name="tag">제거할 조건 태그</param>
        public void RemoveExeCondition(string tag)
        {
            executeConditions.RemoveAll(c => c.Tag == tag);
        }

        /// <summary>
        /// 현재 등록된 모든 처형 조건을 만족하는지 검사합니다.
        /// </summary>
        /// <param name="enemy">검사 대상 적</param>
        /// <returns>모든 조건이 참이면 true, 아니면 false</returns>
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

        /// <summary>
        /// 주간 모드로 전환합니다.
        /// 버튼 상태를 업데이트하고, 낮 UI를 활성화합니다.
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

            // 플레이어 위치 및 속도 리셋
            player.transform.parent.position = new Vector2(0, 0);
            playerRB.linearVelocity = new Vector2(0, 0);

            // 모든 풀 오브젝트 비활성화
            poolManager.DeActiveAllChild();

        }

        /// <summary>
        /// 야간 웨이브를 시작합니다.
        /// 스포너에 EndDay 호출, 밤 UI를 활성화합니다.
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
        /// 수리 미니게임을 시작합니다.
        /// 미니게임 화면을 로드하고, 카메라를 전환합니다.
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
        /// 수리 미니게임을 종료합니다.
        /// 카메라 복원 및 씬 언로드 후 버튼 상태 업데이트.
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
        /// 강화 미니게임을 시작합니다.
        /// 미니게임 화면을 로드하고, 카메라를 전환합니다.
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
        /// 강화 미니게임을 종료합니다.
        /// 새 강화 데이터 저장, 카메라 복원 및 씬 언로드 후 버튼 상태 업데이트.
        /// </summary>
        /// <param name="data">새롭게 적용된 강화 데이터</param>
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
        /// 수집 미니게임을 시작합니다.
        /// 미니게임 화면을 로드하고, 카메라를 전환합니다.
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
        /// 수집 미니게임을 종료합니다.
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
        /// UI 버튼 상호작용 가능 여부를 업데이트합니다.
        /// </summary>
        /// <param name="active">버튼 활성화 여부</param>
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


