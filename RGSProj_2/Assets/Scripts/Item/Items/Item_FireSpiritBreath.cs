using InventorySystem;
using UnityEngine;
using Core;
[CreateAssetMenu(fileName = "Item_FireSpiritBreath", menuName = "Inventory/SkillEvent/A+/FireSpiritBreath")]
public class Item_FireSpiritBreath :InventoryItemSkillEvent
{
    private bool isFireSynerge;
    public override void OnActivate(InventoryItem item, string ID)
    {
        base.OnActivate(item, ID);
        float f_emp = 1.4f;

        bool fireSyn = GameManager.Instance.playerTypeManager.Types[1].typeActiveLevel == 5;
        if (fireSyn != isFireSynerge)
        {
            GameManager.Instance.SM.RemoveModifiersByTag(ID + "_Syn");
            isFireSynerge = fireSyn;
            if (isFireSynerge) f_emp += 0.3f;
            GameManager.Instance.SM.AddModifier("Fire_AtkMul", multiplier: f_emp, tag: ID+"_Syn");
        }
    }

    public override void OnEnter(InventoryItem item, string ID)
    {
        base.OnEnter(item, ID);
        isFireSynerge = false;
        GameManager.Instance.SM.AddModifier("M_AtkMul", multiplier: 1.2f, tag: ID);
        GameManager.Instance.SM.AddModifier("Fire_AtkMul", multiplier: 1.4f, tag: ID + "_Syn");
        GameManager.Instance.SM.AddModifier("F_Cool", multiplier: 0.9f, tag: ID);
    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, ID);
        GameManager.Instance.SM.RemoveModifiersByTag(ID);
        GameManager.Instance.SM.RemoveModifiersByTag(ID + "_Syn");
    }

}
