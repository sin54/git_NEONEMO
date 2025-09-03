using UnityEngine;
[CreateAssetMenu(fileName = "WindSword", menuName = "Data/Item Data/WindSword Data")]
public class SO_WindSwordData :SO_BaseItemData
{
    public float[] atkCoolByLevel = new float[5];
    public int[] swordNumByLevel=new int[5];
    public float[] damageByLevel=new float[5];
    public float[] sizeByLevel=new float[5];
}
