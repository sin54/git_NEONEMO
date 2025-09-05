using UnityEngine;
using Core;

public class BaseType : MonoBehaviour,IUpgradable
{
    public SO_TypeData typeData;
    public int typePassiveLevel;
    public int typeActiveLevel;
    protected Player player;
    public int typeCode;
    private void Start()
    {
        player = GameManager.Instance.player;
    }
    public virtual void Upgrade()
    {
        typePassiveLevel++;
        if (typePassiveLevel == 4)
        {
            GameManager.Instance.levelManager.RemoveAbility(this);
        }
    }
    protected virtual void OnEnable()
    {
        GameManager.Instance.player.typeList.Add(typeCode);
        GameManager.Instance.playerTypeManager.AddType(typeCode);
    }
}
