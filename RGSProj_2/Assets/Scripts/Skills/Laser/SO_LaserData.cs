using UnityEngine;
[CreateAssetMenu(fileName = "Laser", menuName = "Data/Item Data/Laser Data")]
public class SO_LaserData : SO_BaseItemData
{
    public int[] laserNumByLevel=new int[5];
    public float[] damageByLevel= new float[5];
    public float[] sizeByLevel = new float[5];
    public float[] coolTimeByLevel = new float[5];
}
