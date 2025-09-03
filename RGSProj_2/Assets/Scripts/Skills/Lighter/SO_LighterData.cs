using UnityEngine;
[CreateAssetMenu(fileName = "Lighter", menuName = "Data/Item Data/Lighter Data")]
public class SO_LighterData : SO_BaseItemData
{
    public float[] coolTimeByLevel = new float[5];
    public int[] targetNumByLevel = new int[5];
    public int[] fireStackByLevel = new int[5];
}
