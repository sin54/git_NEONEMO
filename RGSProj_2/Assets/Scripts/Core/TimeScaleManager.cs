using Unity.VisualScripting;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// 게임 전반의 시간 흐름을 관리합니다.
    /// dontMoveStack 값이 0일 때만 Time.timeScale을 1로 유지하고,
    /// 0보다 크면 일시정지(0) 상태로 만듭니다.
    /// </summary>
    [DisallowMultipleComponent]
    public class TimeScaleManager : MonoBehaviour
    {
        #region Singleton

        /// <summary>
        /// TimeScaleManager 싱글톤 인스턴스입니다.
        /// </summary>
        public static TimeScaleManager Instance { get; private set; }

        #endregion

        #region Fields

        /// <summary>
        /// 일시정지 요청 스택 카운트입니다.
        /// 0일 때만 시간 흐름이 재생됩니다.</summary>
        [Tooltip("0일 때만 시간 흐름이 재생됩니다.")]
        public int dontMoveStack = 0;

        #endregion

        #region Unity Callbacks

        /// <summary>
        /// 싱글톤 인스턴스를 설정하고 중복 시 파괴합니다.
        /// 씬 전환 시에도 파괴되지 않도록 설정합니다.
        /// </summary>
        private void Awake()
        {
            // 싱글톤 유지
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// 시작 시 일시정지 스택을 초기화합니다.
        /// </summary>
        private void Start()
        {
            dontMoveStack = 0;
        }

        /// <summary>
        /// 매 프레임마다 dontMoveStack 값을 검사하여
        /// Time.timeScale을 0 또는 1로 설정합니다.
        /// </summary>
        private void Update()
        {
            if (dontMoveStack == 0)
            {
                Time.timeScale = 1f;
            }
            else
            {
                Time.timeScale = 0f;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 일시정지 요청을 하나 추가하여
        /// timeScale을 멈추는 방향으로 만듭니다.
        /// </summary>
        public void TimeStopStackPlus()
        {
            dontMoveStack++;
        }

        /// <summary>
        /// 일시정지 요청을 하나 제거하여
        /// timeScale을 재생하는 방향으로 만듭니다.
        /// </summary>
        public void TimeStopStackMinus()
        {
            dontMoveStack--;
        }

        #endregion
    }
}