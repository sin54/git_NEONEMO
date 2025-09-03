using UnityEngine;

public class BaseType : MonoBehaviour,IUpgradable
{
    public SO_TypeData typeData;
    public int typePassiveLevel;
    public int typeActiveLevel;
    protected Player player;
    public int typeCode;
    private void Start()
    {
        player = GameManager.instance.player;
    }
    public virtual void Upgrade()
    {
        typePassiveLevel++;
        if (typePassiveLevel == 4)
        {
            GameManager.instance.levelManager.RemoveAbility(this);
        }
    }
    protected virtual void OnEnable()
    {
        GameManager.instance.player.typeList.Add(typeCode);
        GameManager.instance.playerTypeManager.AddType(typeCode);
    }
}
