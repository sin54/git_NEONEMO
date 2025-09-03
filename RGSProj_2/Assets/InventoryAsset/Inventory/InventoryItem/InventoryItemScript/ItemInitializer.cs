using UnityEngine;

namespace InventorySystem
{
    //Author: Jaxon Schauer
    /// <summary>
    /// Takes input to initialize items when game is played
    /// </summary>
    [System.Serializable]
    public class ItemInitializer
    {
        private int amount = 1;

        // Essential Item Properties
        [Header("========[ Item Properties ]========")]

        [Tooltip("Create a type/classification of items.")]
        [SerializeField]
        private string itemType;

        [Tooltip("Visual representation of the item.")]
        [SerializeField]
        private Sprite itemImage;

        [Tooltip("Maximum number of this item that can be stacked together.")]
        [SerializeField]
        private int maxStackAmount;

        [Tooltip("Display the quantity/amount of the item on its icon.")]
        [SerializeField]
        private bool displayItemAmount;
        // Optional Item Configurations
        [Header("========[ Optional Configurations ]========")]
        [Tooltip("Determines if the item can be dragged within inventories.")]
        [SerializeField]
        private bool draggable;

        [Tooltip("Determines if the item can be highlighted when selected.")]
        [SerializeField]
        private bool pressable;

        [Tooltip("Game object implementation of the item that can be set up to be affected it.")]
        [SerializeField]
        private GameObject RelatedGameObject;

        [Tooltip("Event triggered in relation to this item.")]
        [SerializeField]
        private InventoryItemEvent itemAction;

        [SerializeField]
        private bool isWeapon;

        [SerializeField]
        private ItemTag tags;

        [SerializeField]
        private ItemRarity rarity;

        [SerializeField]
        private InventoryItemSkillEvent itemSkillEvent;

        private string itemGUID;

        [SerializeField, HideInInspector]
        private bool isNull = false;

        [SerializeField, HideInInspector]
        private InventoryItem myItem;
        public ItemInitializer(bool isNull)
        {
            this.isNull = isNull;
        }
        public void SetIsNull(bool isNull)
        {
            this.isNull = isNull;
        }
        public bool GetIsNull()
        {
            return isNull;
        }
        public string GetItemType()
        {
            return itemType;
        }
        public Sprite GetItemImage()
        {
            return itemImage;
        }
        public int GetItemStackAmount()
        {
            return maxStackAmount;
        }
        public bool GetPressable()
        {
            return pressable;
        }
        public bool GetDraggable()
        {
            return draggable;
        }
        public int GetAmount()
        {
            return amount;
        }
        public bool GetIsWeapon()
        {
            return isWeapon;
        }
        public void SetAmount(int amount)
        {
            this.amount = amount;
        }
        public InventoryItemEvent GetEvent()
        {
            return itemAction;
        }
        public GameObject GetRelatedGameObject()
        {
            return RelatedGameObject;
        }
        public bool GetDisplayAmount()
        {
            return displayItemAmount;
        }

        public ItemTag GetTags() => tags;

        public bool HasTag(ItemTag tag)
        {
            return (tags & tag) == tag;
        }
        public ItemRarity GetRarity() => rarity;
        public void SetRarity(ItemRarity r) => rarity = r;

        public void SetSkillEvent(InventoryItemSkillEvent e)
        {
            itemSkillEvent = e;
        }

        public InventoryItemSkillEvent GetSkillEvent()
        {
            return itemSkillEvent;
        }

        public string GetGUID()
        {
            return itemGUID;
        }
        public void SetInventoryItem(InventoryItem item)
        {
            myItem = item;
        }
        public InventoryItem GetInventoryItem()
        {
            return myItem;
        }
    }
}
