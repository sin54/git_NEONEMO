using UnityEngine;
[CreateAssetMenu(fileName ="LevelData",menuName ="Data/Level Data")]
public class SO_LevelData : ScriptableObject
{
    public SpawnData[] spawnDatas;
    public int waveCount => spawnDatas.Length;
}

[System.Serializable]
public struct SpawnData
{
    public EnemyPercentage[] enemyToSpawn;
    public float waveLength;
    public float timeBetweenSpawn;
}
[System.Serializable]
public struct EnemyPercentage
{
    public SO_EnemySpawnData enemySpawnData;
    public float percent;
}
