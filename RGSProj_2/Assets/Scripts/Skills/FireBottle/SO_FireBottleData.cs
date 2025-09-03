using UnityEngine;
[CreateAssetMenu(fileName = "FireBottleData", menuName = "Data/Item Data/FireBottle Data")]
public class SO_FireBottleData : SO_BaseItemData
{
    public int[] numOfIncen = new int[5];
    public float[] attackDamage = new float[5];
    public float[] attackCool = new float[5];
    public float[] incenRadius = new float[5];
    public float[] incenDuration = new float[5];
}
