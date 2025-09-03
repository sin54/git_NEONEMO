using UnityEngine;
[CreateAssetMenu(fileName = "Radar", menuName = "Data/Item Data/Radar Data")]
public class SO_RadarData : SO_BaseItemData
{
    public int[] maxTargetByLevel = new int[5];
    public float[] coolTimeByLevel = new float[5];
    public AttackInfo[] damageByLevel = new AttackInfo[5];
    public float[] raderRadiusByLevel=new float[5];
    public float[] divideValueByLevel = new float[5];
}
