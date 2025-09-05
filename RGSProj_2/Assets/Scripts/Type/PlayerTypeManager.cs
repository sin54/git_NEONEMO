using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class PlayerTypeManager : MonoBehaviour
{
    public BaseType BT;
    public List<int> NowType = new List<int>();
    public List<BaseType> Types=new List<BaseType>();
    public Color[] Colors = new Color[4];
    public Gradient[] Gradients = new Gradient[4];
    private float lastChangeTime;
    [SerializeField] private float changeCool;
    [SerializeField] private SpriteRenderer SR;
    [SerializeField] private ParticleSystem PS;
    
    public int idx = 0;
    private void Start()
    {
    }
    public void AddType(int typeCode)
    {
        NowType.Add(typeCode);
    }
    private void Update()
    {

        if (Time.time>lastChangeTime+changeCool) {
            if (Input.GetKeyDown(KeyCode.A))
            {
                idx = (idx - 1 + NowType.Count) % NowType.Count;
                ChangeColor();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                idx = (idx + 1) % NowType.Count;
                ChangeColor();
            }
        }

    }
    private void ChangeColor()
    {
        lastChangeTime = Time.time;
        BT = Types[NowType[idx]];
        SR.color = Colors[BT.typeCode];
        var PSM = PS.colorOverLifetime;
        PSM.color = Gradients[BT.typeCode];
    }
}
