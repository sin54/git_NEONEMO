using UnityEngine;
using InventorySystem;

namespace InventorySystem
{
    public class SlotItemManager : MonoBehaviour
    {
        private Slot slot;
        private void Awake()
        {
            slot = GetComponent<Slot>();
        }
    }
}

