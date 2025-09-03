using UnityEngine;
[CreateAssetMenu(fileName = "ShootingStar", menuName = "Data/Item Data/ShootingStar Data")]
public class SO_ShootingStarData : SO_BaseItemData
{
    public float[] coolTimeByLevel = new float[5];
    public float[] explosionRadiusByLevel=new float[5];
    public int[] numOfStar=new int[5];
    public AttackInfo[] explosionDamageByLevel = new AttackInfo[5];
}
