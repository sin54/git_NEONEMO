using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Unity.Cinemachine;
using TMPro;
using DialogueSystem;
using Core;

namespace Scene
{
    /// <summary>
    /// 컷신 씬의 여러 연출(랜딩 프로토콜, 연결 상태, 경고, 충돌 등)을 관리합니다.
    /// DialogueSystem 호출 및 다음 씬 로딩도 이 클래스에서 처리합니다.
    /// </summary>
    [DisallowMultipleComponent]
    public class CutSceneManager : MonoBehaviour
    {
        #region Fields

        [Header("Landing Protocol")]
        [Tooltip("부스터 이펙트 파티클 시스템")]
        [SerializeField] private ParticleSystem boost;
        [Tooltip("메인 착륙 파티클 시스템")]
        [SerializeField] private ParticleSystem PS;
        [Tooltip("파티클 속성 페이드에 걸리는 시간(초)")]
        [SerializeField] private float fadeDuration = 1f;

        [Header("Connection")]
        [Tooltip("연결 시도 상태를 표시하는 스프라이트 렌더러")]
        [SerializeField] private SpriteRenderer connectSR;

        [Header("Warning")]
        [Tooltip("경고 이미지 UI")]
        [SerializeField] private Image warningImg;
        [Tooltip("경고 시 카메라 흔들림용 Perlin 노이즈 컴포넌트")]
        [SerializeField] private CinemachineBasicMultiChannelPerlin CBMCP;
        [Tooltip("경고 후 행성 및 우주선 오브젝트")]
        [SerializeField] private GameObject planet;
        [SerializeField] private GameObject spaceShip;
        [SerializeField] private GameObject boom;

        [Header("Story Selection")]
        [Tooltip("스토리 진행 여부")]
        private bool isStory = true;
        private bool storyStart;
        [SerializeField] private GameObject isStoryPanel;
        [SerializeField] private TMP_Text YesText;
        [SerializeField] private TMP_Text NoText;
        [Tooltip("대화 시스템 시작용 홀더")]
        [SerializeField] private DialogueHolder DH;

        #endregion

        #region Unity Callbacks

        /// <summary>
        /// 시작 시 스토리 선택 패널을 최신 상태로 초기화합니다.
        /// </summary>
        private void Start()
        {
            isStory = true;
            UpdatePanel();
        }

        /// <summary>
        /// 매 프레임 입력을 체크하여 스토리/빠른 시작 선택
        /// 또는 대화 시작·게임 씬 로딩을 처리합니다.
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)){
                isStory = !isStory;
                UpdatePanel();  
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { 
                isStory=!isStory;
                UpdatePanel();
            }
            if (Input.GetKeyDown(KeyCode.Return)&&!storyStart)
            {
                storyStart = true;
                if (isStory) {
                    isStoryPanel.SetActive(false);
                    DH.StartDialogue();
                }
                else
                {
                    Loader.Load(Loader.Scene.GameScene);
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 착륙 프로토콜을 실행: 파티클 방출량을 증가시키고 페이드 코루틴을 시작합니다.
        /// </summary>
        public void LandingProtocol()
        {
            var PSMain = PS.emission;
            PSMain.rateOverTime=5;
            StartCoroutine(FadeToTarget());
        }

        /// <summary>
        /// 연결 시도 시 연결 아이콘을 노란색으로 활성화합니다.
        /// </summary>
        public void ConnectTry()
        {
            connectSR.gameObject.SetActive(true);
            connectSR.color = Color.yellow;
        }

        /// <summary>
        /// 연결 실패 시 연결 아이콘을 빨간색으로 표시합니다.
        /// </summary>
        public void ConnectFailed()
        {
            connectSR.color = Color.red;
        }

        /// <summary>
        /// 연결 아이콘을 비활성화합니다.
        /// </summary>
        public void ConnectOff()
        {
            connectSR.gameObject.SetActive(false);
        }

        /// <summary>
        /// 무전 경고 시작: 이미지 루프 페이드 및 카메라 흔들림을 활성화합니다.
        /// </summary>
        public void RadioActive()
        {
            warningImg.gameObject.SetActive(true);
            StartCoroutine(FadeWarningLoop());
            CBMCP.FrequencyGain = 1;
            CBMCP.AmplitudeGain = 1;
        }

        /// <summary>
        /// 경고 강도를 높여 카메라 흔들림을 격하게 만듭니다.
        /// </summary>
        public void Error()
        {
            CBMCP.FrequencyGain = 3;
            CBMCP.AmplitudeGain = 3;
        }

        /// <summary>
        /// 충돌 연출 시작: 파티클 일시 정지 후 크래시 시퀀스를 수행합니다.
        /// </summary>
        public void Crash()
        {
            PS.Pause();
            StartCoroutine(PlanetCrashSequence());
        }

        #endregion

        #region Coroutines

        /// <summary>
        /// 경고 이미지를 지속적으로 페이드 인·아웃 루프로 반복합니다.
        /// </summary>
        private IEnumerator FadeWarningLoop()
        {
            float alphaDuration = 1f; // 한 쪽 방향으로 바뀌는 데 걸리는 시간
            Color baseColor = warningImg.color;

            while (true)
            {
                // Fade In: 알파 0 → 1
                float time = 0f;
                while (time < alphaDuration)
                {
                    time += Time.deltaTime;
                    float t = time / alphaDuration;
                    float alpha = Mathf.Lerp(0f, 1f, t);
                    warningImg.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
                    yield return null;
                }

                // Fade Out: 알파 1 → 0
                time = 0f;
                while (time < alphaDuration)
                {
                    time += Time.deltaTime;
                    float t = time / alphaDuration;
                    float alpha = Mathf.Lerp(1f, 0f, t);
                    warningImg.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
                    yield return null;
                }
            }
        }

        /// <summary>
        /// 착륙 파티클의 속도 및 생명 주기 속성을 목표 값으로 페이드합니다.
        /// </summary>
        private IEnumerator FadeToTarget()
        {
            float time = 0f;

            // 시작값
            float startVelocityX = -10f;
            float startLifetime = 1.0f;

            // 목표값
            float targetVelocityX = -5f;
            float targetLifetime = 0.5f;

            // 초기 설정
            var vel = PS.velocityOverLifetime;
            vel.x = new ParticleSystem.MinMaxCurve(startVelocityX);

            var mainBoost = boost.main;
            mainBoost.startLifetime = new ParticleSystem.MinMaxCurve(startLifetime);

            while (time < fadeDuration)
            {
                time += Time.deltaTime;
                float t = time / fadeDuration;

                float currentVelocityX = Mathf.Lerp(startVelocityX, targetVelocityX, t);
                float currentLifetime = Mathf.Lerp(startLifetime, targetLifetime, t);

                var tempVel = PS.velocityOverLifetime;
                tempVel.x = new ParticleSystem.MinMaxCurve(currentVelocityX);

                var tempBoost = boost.main;
                tempBoost.startLifetime = new ParticleSystem.MinMaxCurve(currentLifetime);

                yield return null;
            }

            // 마지막 정리
            var finalVel = PS.velocityOverLifetime;
            finalVel.x = new ParticleSystem.MinMaxCurve(targetVelocityX);

            var finalBoost = boost.main;
            finalBoost.startLifetime = new ParticleSystem.MinMaxCurve(targetLifetime);
        }

        /// <summary>
        /// 행성 충돌 시퀀스를 연출하고 폭발 이펙트를 확장한 뒤 게임 씬으로 전환합니다.
        /// </summary>
        private IEnumerator PlanetCrashSequence()
        {
            // 준비 단계
            planet.SetActive(true);
            spaceShip.SetActive(true);

            // 위치 초기화
            Vector3 planetStartPos = new Vector3(15f, 1f, 0f); // 오른쪽 바깥
            Vector3 planetTargetPos = new Vector3(11.35f, 1f, 0f); // 중앙

            Vector3 shipStartPos = spaceShip.transform.position;
            Vector3 shipTargetPos = new Vector3(6.36f,1,0);
            Quaternion shipStartRot = spaceShip.transform.rotation;
            Quaternion shipTargetRot = Quaternion.Euler(0f, 0f, 200f); // 2바퀴 회전
            float totalRotation = 1440*1.4f; // z축으로 720도 회전

            float duration = 3f; // 전체 연출 시간
            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                float t = time / duration;

                // 행성 이동
                planet.transform.position = Vector3.Lerp(planetStartPos, planetTargetPos, t);

                float currentZRotation = Mathf.Lerp(0f, totalRotation, t);
                spaceShip.transform.rotation = Quaternion.Euler(0f, 0f, currentZRotation);
                spaceShip.transform.position = Vector3.Lerp(shipStartPos, shipTargetPos, t); // 행성을 향해 이동

                yield return null;
            }

            StartCoroutine(ScaleBoomOverTime(boom.transform));
        }

        /// <summary>
        /// 폭발 이펙트를 부드럽게 확대하는 코루틴입니다.
        /// </summary>
        /// <param name="boomTransform">확대할 이펙트 트랜스폼</param>
        /// <param name="duration">확대에 걸리는 시간(초)</param>
        /// <param name="maxScale">최대 확대 배율</param>
        private IEnumerator ScaleBoomOverTime(Transform boom, float duration = 1f, float maxScale = 40f)
        {
            float time = 0f;
            Vector3 startScale = Vector3.zero;
            Vector3 endScale = Vector3.one * maxScale;

            while (time < duration)
            {
                time += Time.deltaTime;
                float t = time / duration;

                // 부드러운 이징 (느리게 시작 → 빠르게 → 다시 느리게)
                t = Mathf.SmoothStep(0f, 1f, t);

                boom.localScale = Vector3.Lerp(startScale, endScale, t);
                yield return null;
            }

            boom.localScale = endScale; // 정확히 도달

            yield return new WaitForSeconds(1f);

            Loader.Load(Loader.Scene.GameScene);
        }

        #endregion

        #region Private Methods

        private void UpdatePanel()
        {
            if (isStory)
            {
                YesText.color = Color.red;
                NoText.color = Color.white;
            }
            else
            {
                YesText.color = Color.white;
                NoText.color = Color.red;
            }
        }

        #endregion
    }
}