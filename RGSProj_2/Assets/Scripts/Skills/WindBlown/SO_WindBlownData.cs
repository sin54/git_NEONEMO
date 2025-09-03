using UnityEngine;
[CreateAssetMenu(fileName = "WindBlownData", menuName = "Data/Item Data/WindBlown Data")]
public class SO_WindBlownData : SO_BaseItemData
{
    public float speedMulAmount;
    public float[] coolTimeByLevel = new float[5];
    public float[] durationByLevel = new float[5];
    public AttackInfo[] damageByLevel = new AttackInfo[5];
    public float[] rangeByLevel=new float[5];
}
