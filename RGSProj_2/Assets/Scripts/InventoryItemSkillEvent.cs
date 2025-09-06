using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public abstract class InventoryItemSkillEvent : ScriptableObject
    {
        public string itemName;
        [TextArea]
        public string itemDescription;
        [TextArea]
        public string itemAbility;
        [TextArea]
        public string synergeString;

        protected int slotNum;
        protected Inventory activeInv;
        protected bool isInit;
        public virtual void OnEnter(InventoryItem item, string ID)
        {
            Debug.Log("ENTER");
            activeInv=InventoryController.instance.GetInventory("Active");
            activeInv.AddTagsFromItem(item);

        }
        public virtual void OnActivate(InventoryItem item, string ID)
        {
            slotNum=item.GetPosition(); 
        }
        public virtual void OnExit(InventoryItem item, string ID)
        {
            activeInv.RemoveTagsFromItem(item);
        }
    }
}