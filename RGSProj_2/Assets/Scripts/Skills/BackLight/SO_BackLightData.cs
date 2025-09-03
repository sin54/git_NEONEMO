using UnityEngine;
[CreateAssetMenu(fileName = "BackLight", menuName = "Data/Item Data/BackLight Data")]
public class SO_BackLightData : SO_BaseItemData
{
    public float[] damageMultiplier = new float[5];
    public float[] coolTimeByLevel = new float[5];
    public float[] durationByLevel=new float[5];
}
