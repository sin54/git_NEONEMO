using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Unity.Cinemachine;
using TMPro;
using DialogueSystem;

namespace Scenes
{
    /// <summary>
    /// �ƽ� ���� ���� ����(���� ��������, ���� ����, ���, �浹 ��)�� �����մϴ�.
    /// DialogueSystem ȣ�� �� ���� �� �ε��� �� Ŭ�������� ó���մϴ�.
    /// </summary>
    [DisallowMultipleComponent]
    public class CutSceneManager : MonoBehaviour
    {
        #region Fields

        [Header("Landing Protocol")]
        [Tooltip("�ν��� ����Ʈ ��ƼŬ �ý���")]
        [SerializeField] private ParticleSystem boost;
        [Tooltip("���� ���� ��ƼŬ �ý���")]
        [SerializeField] private ParticleSystem PS;
        [Tooltip("��ƼŬ �Ӽ� ���̵忡 �ɸ��� �ð�(��)")]
        [SerializeField] private float fadeDuration = 1f;

        [Header("Connection")]
        [Tooltip("���� �õ� ���¸� ǥ���ϴ� ��������Ʈ ������")]
        [SerializeField] private SpriteRenderer connectSR;

        [Header("Warning")]
        [Tooltip("��� �̹��� UI")]
        [SerializeField] private Image warningImg;
        [Tooltip("��� �� ī�޶� ��鸲�� Perlin ������ ������Ʈ")]
        [SerializeField] private CinemachineBasicMultiChannelPerlin CBMCP;
        [Tooltip("��� �� �༺ �� ���ּ� ������Ʈ")]
        [SerializeField] private GameObject planet;
        [SerializeField] private GameObject spaceShip;
        [SerializeField] private GameObject boom;

        [Header("Story Selection")]
        [Tooltip("���丮 ���� ����")]
        private bool isStory = true;
        private bool storyStart;
        [SerializeField] private GameObject isStoryPanel;
        [SerializeField] private TMP_Text YesText;
        [SerializeField] private TMP_Text NoText;
        [Tooltip("��ȭ �ý��� ���ۿ� Ȧ��")]
        [SerializeField] private DialogueHolder DH;

        #endregion

        #region Unity Callbacks

        /// <summary>
        /// ���� �� ���丮 ���� �г��� �ֽ� ���·� �ʱ�ȭ�մϴ�.
        /// </summary>
        private void Start()
        {
            isStory = true;
            UpdatePanel();
        }

        /// <summary>
        /// �� ������ �Է��� üũ�Ͽ� ���丮/���� ���� ����
        /// �Ǵ� ��ȭ ���ۡ����� �� �ε��� ó���մϴ�.
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
        /// ���� ���������� ����: ��ƼŬ ���ⷮ�� ������Ű�� ���̵� �ڷ�ƾ�� �����մϴ�.
        /// </summary>
        public void LandingProtocol()
        {
            var PSMain = PS.emission;
            PSMain.rateOverTime=5;
            StartCoroutine(FadeToTarget());
        }

        /// <summary>
        /// ���� �õ� �� ���� �������� ��������� Ȱ��ȭ�մϴ�.
        /// </summary>
        public void ConnectTry()
        {
            connectSR.gameObject.SetActive(true);
            connectSR.color = Color.yellow;
        }

        /// <summary>
        /// ���� ���� �� ���� �������� ���������� ǥ���մϴ�.
        /// </summary>
        public void ConnectFailed()
        {
            connectSR.color = Color.red;
        }

        /// <summary>
        /// ���� �������� ��Ȱ��ȭ�մϴ�.
        /// </summary>
        public void ConnectOff()
        {
            connectSR.gameObject.SetActive(false);
        }

        /// <summary>
        /// ���� ��� ����: �̹��� ���� ���̵� �� ī�޶� ��鸲�� Ȱ��ȭ�մϴ�.
        /// </summary>
        public void RadioActive()
        {
            warningImg.gameObject.SetActive(true);
            StartCoroutine(FadeWarningLoop());
            CBMCP.FrequencyGain = 1;
            CBMCP.AmplitudeGain = 1;
        }

        /// <summary>
        /// ��� ������ ���� ī�޶� ��鸲�� ���ϰ� ����ϴ�.
        /// </summary>
        public void Error()
        {
            CBMCP.FrequencyGain = 3;
            CBMCP.AmplitudeGain = 3;
        }

        /// <summary>
        /// �浹 ���� ����: ��ƼŬ �Ͻ� ���� �� ũ���� �������� �����մϴ�.
        /// </summary>
        public void Crash()
        {
            PS.Pause();
            StartCoroutine(PlanetCrashSequence());
        }

        #endregion

        #region Coroutines

        /// <summary>
        /// ��� �̹����� ���������� ���̵� �Ρ��ƿ� ������ �ݺ��մϴ�.
        /// </summary>
        private IEnumerator FadeWarningLoop()
        {
            float alphaDuration = 1f; // �� �� �������� �ٲ�� �� �ɸ��� �ð�
            Color baseColor = warningImg.color;

            while (true)
            {
                // Fade In: ���� 0 �� 1
                float time = 0f;
                while (time < alphaDuration)
                {
                    time += Time.deltaTime;
                    float t = time / alphaDuration;
                    float alpha = Mathf.Lerp(0f, 1f, t);
                    warningImg.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
                    yield return null;
                }

                // Fade Out: ���� 1 �� 0
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
        /// ���� ��ƼŬ�� �ӵ� �� ���� �ֱ� �Ӽ��� ��ǥ ������ ���̵��մϴ�.
        /// </summary>
        private IEnumerator FadeToTarget()
        {
            float time = 0f;

            // ���۰�
            float startVelocityX = -10f;
            float startLifetime = 1.0f;

            // ��ǥ��
            float targetVelocityX = -5f;
            float targetLifetime = 0.5f;

            // �ʱ� ����
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

            // ������ ����
            var finalVel = PS.velocityOverLifetime;
            finalVel.x = new ParticleSystem.MinMaxCurve(targetVelocityX);

            var finalBoost = boost.main;
            finalBoost.startLifetime = new ParticleSystem.MinMaxCurve(targetLifetime);
        }

        /// <summary>
        /// �༺ �浹 �������� �����ϰ� ���� ����Ʈ�� Ȯ���� �� ���� ������ ��ȯ�մϴ�.
        /// </summary>
        private IEnumerator PlanetCrashSequence()
        {
            // �غ� �ܰ�
            planet.SetActive(true);
            spaceShip.SetActive(true);

            // ��ġ �ʱ�ȭ
            Vector3 planetStartPos = new Vector3(15f, 1f, 0f); // ������ �ٱ�
            Vector3 planetTargetPos = new Vector3(11.35f, 1f, 0f); // �߾�

            Vector3 shipStartPos = spaceShip.transform.position;
            Vector3 shipTargetPos = new Vector3(6.36f,1,0);
            Quaternion shipStartRot = spaceShip.transform.rotation;
            Quaternion shipTargetRot = Quaternion.Euler(0f, 0f, 200f); // 2���� ȸ��
            float totalRotation = 1440*1.4f; // z������ 720�� ȸ��

            float duration = 3f; // ��ü ���� �ð�
            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                float t = time / duration;

                // �༺ �̵�
                planet.transform.position = Vector3.Lerp(planetStartPos, planetTargetPos, t);

                float currentZRotation = Mathf.Lerp(0f, totalRotation, t);
                spaceShip.transform.rotation = Quaternion.Euler(0f, 0f, currentZRotation);
                spaceShip.transform.position = Vector3.Lerp(shipStartPos, shipTargetPos, t); // �༺�� ���� �̵�

                yield return null;
            }

            StartCoroutine(ScaleBoomOverTime(boom.transform));
        }

        /// <summary>
        /// ���� ����Ʈ�� �ε巴�� Ȯ���ϴ� �ڷ�ƾ�Դϴ�.
        /// </summary>
        /// <param name="boomTransform">Ȯ���� ����Ʈ Ʈ������</param>
        /// <param name="duration">Ȯ�뿡 �ɸ��� �ð�(��)</param>
        /// <param name="maxScale">�ִ� Ȯ�� ����</param>
        private IEnumerator ScaleBoomOverTime(Transform boom, float duration = 1f, float maxScale = 40f)
        {
            float time = 0f;
            Vector3 startScale = Vector3.zero;
            Vector3 endScale = Vector3.one * maxScale;

            while (time < duration)
            {
                time += Time.deltaTime;
                float t = time / duration;

                // �ε巯�� ��¡ (������ ���� �� ������ �� �ٽ� ������)
                t = Mathf.SmoothStep(0f, 1f, t);

                boom.localScale = Vector3.Lerp(startScale, endScale, t);
                yield return null;
            }

            boom.localScale = endScale; // ��Ȯ�� ����

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