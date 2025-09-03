using UnityEngine;
[CreateAssetMenu(fileName = "AreaAttacker", menuName = "Data/Item Data/AreaAttacker Data")]
public class SO_AreaAttackerData : SO_BaseItemData
{
    public float damageInterval = 0.2f;
    public float[] attackDmgByLevel = new float[5];
    public float[] sizeByLevel=new float[5];
}
