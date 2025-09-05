using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Core;

using Random = UnityEngine.Random;

namespace Core
{
    /// <summary>
    /// 우주선 수리 미니게임을 관리합니다.
    /// 게임 시작부터 종료, 점수 집계 및 UI 갱신 로직을 포함합니다.
    /// </summary>
    [DisallowMultipleComponent]
    public class RepairManager : MonoBehaviour
    {
        #region Fields


        [Tooltip("게임 시작 전 준비 문구")]
        [SerializeField] private TMP_Text ReadyTxt;

        [Header("UI Panels")]
        [Tooltip("게임 시작 전 표시할 패널")]
        [SerializeField] private GameObject gameStartPanel;
        [Tooltip("준비 상태를 표시할 텍스트(TMP)")]
        [SerializeField] private TMP_Text readyText;

        [Header("Gameplay Settings")]
        [Tooltip("스폰할 대상 프리팹")]
        [SerializeField] private GameObject target;
        [Tooltip("미니게임 제한 시간(초)")]
        [SerializeField] private float gameTime;
        [Tooltip("스폰할 타겟 수")]
        [SerializeField] private int numOfTarget;

        [Header("Particles")]
        [Tooltip("폭발 이펙트 프리팹")]
        [SerializeField] private GameObject explodeParticle;
        [Tooltip("힐 이펙트 프리팹")]
        [SerializeField] private GameObject healParticle;
        [Tooltip("폭발 이펙트 색상 그라디언트 배열")]
        [SerializeField] private Gradient[] explodeGradient;

        [Header("Time UI")]
        [Tooltip("남은 시간 비주얼 이미지")]
        [SerializeField] private Image leftTime;
        [Tooltip("남은 시간 텍스트(TMP)")]
        [SerializeField] private TMP_Text leftTimeTxt;

        [Header("Health UI")]
        [Tooltip("남은 체력 바 이미지")]
        [SerializeField] private Image leftHealthImg;
        [Tooltip("플로팅 텍스트 생성 위치")]
        [SerializeField] private Transform spawnFloatingPos;

        [Header("Heal Settings")]
        [Tooltip("힐 시 회복량")]
        [SerializeField] private int healAmount;
        [Tooltip("최대 체력 증가량")]
        [SerializeField] private int maxHealAmount;

        [Header("Result UI")]
        [Tooltip("미니게임 종료 후 결과 패널")]
        [SerializeField] private GameObject resultPanel;
        [Tooltip("적 명중 수 텍스트(TMP)")]
        [SerializeField] private TMP_Text resultHitTxt;
        [Tooltip("수리 완료 수 텍스트(TMP)")]
        [SerializeField] private TMP_Text resultRepairTxt;
        [Tooltip("힐량 텍스트(TMP)")]
        [SerializeField] private TMP_Text resultHealTxt;
        [Tooltip("최대 체력 증가량 텍스트(TMP)")]
        [SerializeField] private TMP_Text resultMaxTxt;

        private int resultHit;
        private int resultRepair;
        private int resultHeal;
        private int resultMax;

        /// <summary>
        /// 미니게임 종료 시 외부에 알릴 이벤트입니다.
        /// </summary>
        public static event Action OnGameFinished;

        /// <summary>
        /// 체력 증가 트리거용 이벤트
        /// </summary>
        private UnityEvent increaseHealth;

        /// <summary>
        /// 미니게임 진행 중 여부 플래그
        /// </summary>
        private bool isGaming;

        /// <summary>
        /// 남은 스폰할 타겟 개수
        /// </summary>
        private int leftNum;

        /// <summary>
        /// 남은 게임 시간(초)
        /// </summary>
        private float nowGameTime;

        #endregion

        #region Unity Callbacks


        private void Update()
        {
            if (!isGaming)
                return;

            // 제한 시간을 감소시키고 UI 업데이트
            nowGameTime -= Time.deltaTime;
            leftTime.fillAmount = nowGameTime / gameTime;
            leftTimeTxt.text = nowGameTime.ToString("F2");

            if (nowGameTime <= 0f)
            {
                isGaming = false;
                leftTimeTxt.text = "00:00";
                StartCoroutine(FinishGame());
            }

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 미니게임을 준비하고 시작합니다.
        /// </summary>
        /// <param name="currentHP">플레이어 현재 체력 (초기 체력 표시용)</param>
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

        #endregion

        #region Coroutines


        /// <summary>
        /// 게임 시작 전 준비 단계(카운트다운)를 처리합니다.
        /// </summary>
        IEnumerator GameStart()
        {
            readyText.text = "우주선을 수리하세요!";
            yield return new WaitForSeconds(1f);

            for (int i = 3; i > 0; i--)
            {
                readyText.text = i.ToString();
                yield return new WaitForSeconds(1f);
            }

            readyText.text = "시작!";
            yield return new WaitForSeconds(1f);

            isGaming = true;
            gameStartPanel.SetActive(false);

            // 타겟 스폰
            nowGameTime = gameTime;
            for (int i = 0; i < numOfTarget; i++)
                SpawnEnemy();

        }

        /// <summary>
        /// 게임 종료 처리 및 결과 UI 순차 표시를 수행합니다.
        /// </summary>
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
            GameManager.Instance.EndRepair();
        }

        /// <summary>
        /// 마우스 클릭을 대기합니다.
        /// </summary>
        IEnumerator WaitForMouseClick()
        {
            // 마우스 누르고 있는 중이면 기다린다 (버튼이 떨어질 때까지)
            yield return new WaitUntil(() => !Input.GetMouseButton(0));

            // 다시 눌릴 때까지 기다림 (정확한 "클릭" 감지)
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 타겟 오브젝트를 임의 위치에 생성합니다.
        /// </summary>
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

        /// <summary>
        /// 힐 이펙트를 스폰하고 회복량 집계를 증가시킵니다.
        /// </summary>
        /// <param name="pos">스폰 위치</param>
        public void SpawnParticles(Vector2 pos)
        {
            resultRepair++;
            Instantiate(healParticle,pos, Quaternion.identity);
        }

        /// <summary>
        /// 폭발 이펙트를 스폰하고 명중 수를 증가시킵니다.
        /// </summary>
        /// <param name="hp">현재 남은 HP 단계 인덱스</param>
        /// <param name="pos">스폰 위치</param>
        public void SpawnExplodeParticles(int hp,Vector2 pos)
        {
            resultHit++;
            ParticleSystem PS=explodeParticle.GetComponent<ParticleSystem>();
            var PSGrad = PS.colorOverLifetime;
            PSGrad.color = explodeGradient[hp];
            Instantiate(explodeParticle, pos, Quaternion.identity);
        }

        /// <summary>
        /// 플레이어를 회복하거나 최대 체력을 증가시키고,
        /// 플로팅 텍스트를 표시합니다.
        /// </summary>
        public void HealPlayer()
        {
            GameObject floatingTxt = GameManager.Instance.poolManager.Get(3);
            floatingTxt.transform.position = spawnFloatingPos.position;
            floatingTxt.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-10, 10));
            TMP_Text floatingTxt_TMP = floatingTxt.GetComponentInChildren<TextMeshPro>();

            if (GameManager.Instance.player.playerCurrentHealth >= GameManager.Instance.player.playerMaxHealth)
            {
                GameManager.Instance.player.IncreaseMaxHealth(maxHealAmount);
                floatingTxt_TMP.text = "+" + maxHealAmount.ToString();
                floatingTxt_TMP.fontSize = 6.5f;
                floatingTxt_TMP.color = Color.red;
                resultMax += maxHealAmount;
            }
            else
            {
                GameManager.Instance.player.IncreaseHealth(healAmount);
                floatingTxt_TMP.text = "+" + healAmount.ToString();
                floatingTxt_TMP.fontSize = 6.5f;
                floatingTxt_TMP.color = Color.green;
                resultHeal += healAmount;
            }

            UpdateHealth();
        }

        /// <summary>
        /// 체력 UI fillAmount를 현재 체력 비율로 갱신합니다.
        /// </summary>
        public void UpdateHealth()
        {
            leftHealthImg.fillAmount=GameManager.Instance.player.playerCurrentHealth/GameManager.Instance.player.playerMaxHealth;
        }

        #endregion
    }
}