using UnityEngine;
[CreateAssetMenu(fileName = "MeteorData", menuName = "Data/Item Data/Meteor Data")]
public class SO_MeteorData : SO_BaseItemData
{
    public float[] coolTimeByLevel = new float[5];
    public int[] meteorNumByLevel=new int[5];
    public float[] explosionRadiusByLevel=new float[5];
    public AttackInfo[] attackInfoByLevel= new AttackInfo[5];
}
