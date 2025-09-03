using UnityEngine;
[CreateAssetMenu(fileName = "IncenAttack", menuName = "Data/Item Data/Incen Data")]
public class SO_IncenData : SO_BaseItemData
{
    public int[] numOfIncen = new int[5];
    public float[] attackDamage = new float[5];
    public float[] attackCool=new float[5];
    public float[] incenRadius= new float[5];
    public float[] incenDuration= new float[5];

    public float attackRadius;
}
