using InventorySystem;
using UnityEngine;
using Core;
[CreateAssetMenu(fileName = "Item_RedRedRedStone", menuName = "Inventory/SkillEvent/B+/Item_RedRedRedStone")]
public class Item_RedRedRedStone : InventoryItemSkillEvent
{
    private bool synerge_H;
    public override void OnActivate(InventoryItem item, string ID)
    {
        base.OnActivate(item, ID);
        bool newSyn = (slotNum == 4);

        if (newSyn != synerge_H)
        {
            synerge_H = newSyn;
            if (synerge_H)
            {
                GameManager.Instance.player.DecreaseMaxHealth(20);
                GameManager.Instance.player.IncreaseMaxHealth(30);
            }
            else
            {
                GameManager.Instance.player.DecreaseMaxHealth(30);
                GameManager.Instance.player.IncreaseMaxHealth(20);
            }
        }
        

    }

    public override void OnEnter(InventoryItem item, string ID)
    {
        base.OnEnter(item, ID);
        synerge_H = false;
        GameManager.Instance.player.IncreaseMaxHealth(20);
        GameManager.Instance.SM.AddModifier("F_Cool", multiplier: 0.9f, tag: ID);
    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, ID);
        GameManager.Instance.player.DecreaseMaxHealth(synerge_H?30:20);
        GameManager.Instance.SM.RemoveModifiersByTag(ID);
    }
}