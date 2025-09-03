using UnityEngine;
[CreateAssetMenu(fileName = "FireSpirit", menuName = "Data/Item Data/FireSpirit Data")]
public class SO_SpiritData : SO_BaseItemData
{
    public float bulletSpeed;
    public int[] countByLevel = new int[5];
    public int[] breathCount=new int[5];
    public float[] breathCool=new float[5];
    public float[] boomRadByLevel = new float[5];
    public float[] boomDmgByLevel= new float[5];    
}
