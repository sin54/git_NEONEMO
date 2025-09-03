using UnityEngine;
[CreateAssetMenu(fileName = "LightCircle", menuName = "Data/Item Data/LightCircle Data")]
public class SO_LightCircleData : SO_BaseItemData
{
    public float[] increasingSpeed = new float[5];
    public AttackInfo[] damageByLevel=new AttackInfo[5];
    public float[] speedMulAmount=new float[5];
    public float[] coolTimeByLevel=new float[5];
}
