using UnityEngine;
[CreateAssetMenu(fileName = "WindGhostData", menuName = "Data/Item Data/WindGhost Data")]
public class SO_GhostData : SO_BaseItemData
{
    public float playerSpeedAddAmount;
    public float[] coolTimeByLevel = new float[5];
    public float[] durationByLevel = new float[5];
    public float[] damageByLevel = new float[5];
}
