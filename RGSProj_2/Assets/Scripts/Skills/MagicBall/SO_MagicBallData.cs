using UnityEngine;
[CreateAssetMenu(fileName = "MagicBall", menuName = "Data/Item Data/MagicBall Data")]
public class SO_MagicBallData : SO_BaseItemData
{
    public int[] numOfBall = new int[5];
    public float[] attackDamage = new float[5];
    public float[] ballRadius = new float[5];
    public float[] ballSpeed = new float[5];
}
