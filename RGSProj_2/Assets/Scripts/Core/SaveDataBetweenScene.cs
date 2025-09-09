using UnityEngine;

namespace Core
{
    /// <summary>
    /// 씬 전환 시에도 파괴되지 않고 유지되어야 하는 데이터를 보관하는 싱글톤 컴포넌트입니다.
    /// 게임 전반에서 Persist 해야 할 값을 이곳에 저장하세요.
    /// </summary>
    [DisallowMultipleComponent]
    public class SaveDataBetweenScene : MonoBehaviour
    {
        #region Singleton

        /// <summary>
        /// 전역 인스턴스. null 방지를 위해 <see cref="Awake"/>에서 설정됩니다.
        /// </summary>
        public static SaveDataBetweenScene Instance { get; private set; }

        private void Awake()
        {
            // 싱글톤 중복 방지
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        #endregion

        #region Persisted Data

        // TODO: 여기 아래에 실제로 유지해야 할 필드·프로퍼티를 선언하세요.
        // 예) public int playerLevel;
        //     public float volumeSetting;

        #endregion
    }
}