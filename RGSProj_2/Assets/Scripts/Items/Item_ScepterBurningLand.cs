using InventorySystem;
using UnityEngine;
[CreateAssetMenu(fileName = "Item_ScepterBurningLand", menuName = "Inventory/SkillEvent/S+/Item_ScepterBurningLand")]
public class Item_ScepterBurningLand : InventoryItemSkillEvent
{
    private bool isFireSynerge_Act;
    private bool isFireSynerge_Pas;
    public override void OnActivate(InventoryItem item, string ID)
    {
        base.OnActivate(item, ID);
        float f_emp = 1.8f;
        float f_cool = 0.8f;

        bool fireSyn_A = GameManager.instance.playerTypeManager.Types[1].typeActiveLevel == 5;
        bool fireSyn_P = GameManager.instance.playerTypeManager.Types[1].typePassiveLevel == 4;

        if (fireSyn_A != isFireSynerge_Act)
        {
            GameManager.instance.SM.RemoveModifiersByTag(ID+"_SynA");
            isFireSynerge_Act = fireSyn_A;
            if (isFireSynerge_Act) f_emp += 0.4f;
            GameManager.instance.SM.AddModifier("Fire_AtkMul", multiplier: f_emp, tag: ID + "_SynA");
        }
        if (fireSyn_P != isFireSynerge_Pas) {
            GameManager.instance.SM.RemoveModifiersByTag(ID + "_SynP");
            isFireSynerge_Pas=fireSyn_P;
            if (isFireSynerge_Pas) f_cool -= 0.15f;
            GameManager.instance.SM.AddModifier("F_Cool", multiplier: f_cool, tag: ID + "_SynP");
        }
    }

    public override void OnEnter(InventoryItem item, string ID)
    {
        base.OnEnter(item, ID);
        isFireSynerge_Act = false;
        isFireSynerge_Pas = false;
        GameManager.instance.SM.AddModifier("M_AtkMul", multiplier: 1.3f, tag: ID);
        GameManager.instance.SM.AddModifier("Fire_AtkMul", multiplier: 1.8f, tag: ID + "_SynA");
        GameManager.instance.SM.AddModifier("F_Cool", multiplier: 0.8f, tag: ID+"_SynP");
        GameManager.instance.AddExeCondition(ID, e => e.eSS.GetFireStack() >= 35);
    }

    public override void OnExit(InventoryItem item, string ID)
    {
        base.OnExit(item, ID);
        GameManager.instance.SM.RemoveModifiersByTag(ID);
        GameManager.instance.SM.RemoveModifiersByTag(ID+"_SynA");
        GameManager.instance.SM.RemoveModifiersByTag(ID + "_SynP");
        GameManager.instance.RemoveExeCondition(ID);
    }
}