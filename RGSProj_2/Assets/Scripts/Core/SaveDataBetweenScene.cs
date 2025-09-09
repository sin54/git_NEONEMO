using UnityEngine;

namespace Core
{
    /// <summary>
    /// �� ��ȯ �ÿ��� �ı����� �ʰ� �����Ǿ�� �ϴ� �����͸� �����ϴ� �̱��� ������Ʈ�Դϴ�.
    /// ���� ���ݿ��� Persist �ؾ� �� ���� �̰��� �����ϼ���.
    /// </summary>
    [DisallowMultipleComponent]
    public class SaveDataBetweenScene : MonoBehaviour
    {
        #region Singleton

        /// <summary>
        /// ���� �ν��Ͻ�. null ������ ���� <see cref="Awake"/>���� �����˴ϴ�.
        /// </summary>
        public static SaveDataBetweenScene Instance { get; private set; }

        private void Awake()
        {
            // �̱��� �ߺ� ����
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

        // TODO: ���� �Ʒ��� ������ �����ؾ� �� �ʵ塤������Ƽ�� �����ϼ���.
        // ��) public int playerLevel;
        //     public float volumeSetting;

        #endregion
    }
}