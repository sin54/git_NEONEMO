using UnityEngine;
[CreateAssetMenu(fileName = "Blade", menuName = "Data/Item Data/Blade Data")]
public class SO_BladeData : SO_BaseItemData
{
    public AttackInfo[] attackInfoByLevel=new AttackInfo[5];
    public int[] countByLevel=new int[5];
    public float[] rotateSpeedByLevel=new float[5];
}
