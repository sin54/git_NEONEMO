using UnityEngine;
[CreateAssetMenu(fileName = "WindKnockBack", menuName = "Data/Item Data/WindKnockBack Data")]
public class SO_WindKnockbackData : SO_BaseItemData
{
    public float speedMul;
    public float[] coolTimeByLevel = new float[5];
    public float[] radiusByLevel = new float[5];
    public AttackInfo[] attackInfoByLevel=new AttackInfo[5];
}
