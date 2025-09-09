using UnityEngine;

public class BaseRage : MonoBehaviour
{
    public float needGauge;
    public float currentGauge;
    public bool canAddGauge;

    public BaseType type;
    
    protected virtual void Start()
    {
        canAddGauge = true;
    }

    public virtual void RageStart()
    {

    }
}
