using UnityEngine;
[CreateAssetMenu(fileName = "WindChargeData", menuName = "Data/Item Data/WindCharge Data")]
public class SO_WindChargeData : SO_BaseItemData
{
    public float bulletSpeed;
    public float[] atkCoolByLevel=new float[5];
    public float[] atkRadByLevel=new float[5];
    public AttackInfo[] atkInfoByLevel=new AttackInfo[5];
}
