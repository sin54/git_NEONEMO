using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Data/Enemy")]
public class SO_EnemySpawnData : ScriptableObject
{
    public string enemyName; // 에디터에서 구분하기 위한 이름
    public int enemyPrefabNum;
    public float markerSize;
}
