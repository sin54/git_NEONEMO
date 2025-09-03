using UnityEngine;
[CreateAssetMenu(fileName = "BowAttack", menuName = "Data/Item Data/MagicBullet Data")]
public class SO_MagicBulletData : SO_BaseItemData
{
    public float[] arrowSpeedByLevel = new float[5];
    public float arrowLifeTime;
    public AttackInfo[] attackInfoByLevel = new AttackInfo[5];
    public float[] coolTimeByLevel = new float[5];
}
