using UnityEngine;
[CreateAssetMenu(fileName = "Dash", menuName = "Data/Item Data/Dash Data")]
public class SO_DashData : SO_BaseItemData
{
    public float[] dashSpeed = new float[5];
    public float[] dashCool=new float[5];
    public float[] lightDashSpeed=new float[5];
    public float dashTime;
}
