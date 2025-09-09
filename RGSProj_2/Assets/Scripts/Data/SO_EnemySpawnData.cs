using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Data/Enemy")]
public class SO_EnemySpawnData : ScriptableObject
{
    public string enemyName; // �����Ϳ��� �����ϱ� ���� �̸�
    public int enemyPrefabNum;
    public float markerSize;
}
