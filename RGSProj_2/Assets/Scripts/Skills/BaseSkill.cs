using UnityEngine;

public class BaseSkill : MonoBehaviour,IUpgradable
{
    public SO_BaseItemData baseSkillData;
    public BaseType TC;
    public int itemLevel;
    public int reinforcedNum = 0;
    public bool isReinforced => itemLevel == 3;
    public bool isMaxLevel => itemLevel == 4;
    public virtual void Upgrade()
    {
        if (itemLevel == -1)
        {
            TC.typeActiveLevel++;
        }
        itemLevel++;

    }
}
