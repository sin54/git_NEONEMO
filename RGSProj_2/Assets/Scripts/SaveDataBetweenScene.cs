using UnityEngine;

public class SaveDataBetweenScene : MonoBehaviour
{
    public static SaveDataBetweenScene Instance;
    void Awake()
    {
        // 싱글톤 중복 방지
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 씬 이동 시 파괴되지 않음
    }
}
