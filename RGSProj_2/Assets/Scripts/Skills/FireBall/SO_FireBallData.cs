using UnityEngine;
[CreateAssetMenu(fileName = "FireBall", menuName = "Data/Item Data/FireBall Data")]
public class SO_FireBallData : SO_BaseItemData
{
    public float fireBallSpeed;
    public float fireBallTime;
    public float[] damageByLevel = new float[5];
    public float[] explosionRadiusByLevel=new float[5];
    public int[] fireBallAmountByLevel=new int[5];
    public float[] coolTimeByLevel = new float[5];
}
