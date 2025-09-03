using UnityEngine;
[CreateAssetMenu(fileName = "BowAttack", menuName = "Data/Item Data/BowAttack Data")]
public class SO_BowAttackData : SO_BaseItemData
{
    public float[] arrowSpeedByLevel=new float[5];
    public float arrowLifeTime;
    public AttackInfo[] attackInfoByLevel = new AttackInfo[5];
    public int[] maxPenetrationLimit=new int[5];
    public float[] coolTimeByLevel = new float[5];
}
