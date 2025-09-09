using InventorySystem;
using Scenes;
using UnityEngine;

public class ItemObj : MonoBehaviour
{
    [SerializeField] private SpriteRenderer backSR;
    [SerializeField] private SpriteRenderer itemSR;
    [SerializeField] private GameObject selectedGO;
    [SerializeField] private SetToolTip STT;
    private InventoryItem II;
    private CollectSceneManager SCM;
    private bool isSelected;
    private void Awake()
    {
        STT=GameObject.Find("Canvas").transform.GetChild(1).GetComponent<SetToolTip>();
        SCM=GameObject.Find("CollectSceneManager").GetComponent<CollectSceneManager>();
    }
    private void Start()
    {
    }
    private void Update()
    {
        
    }
    public void Init(InventoryItem IItem)
    {
        selectedGO.SetActive(true);
        isSelected = true;
        Color newColor = backSR.color;
        newColor.a = 0.4f;

        backSR.color = newColor;
        ItemRarity IR=IItem.GetRarity();
        II = IItem;
        SCM.AddCollectedItem(II);
        if (IR == ItemRarity.Alpha)
        {
            backSR.color=new Color(1,1,1,0.4f);
        }
        else if (IR == ItemRarity.Beta)
        {
            backSR.color = new Color(0, 0.9f, 1, 0.4f);
        }
        else if (IR == ItemRarity.Gamma)
        {
            backSR.color = new Color(0, 1, 0, 0.4f);
        }
        else if (IR == ItemRarity.Delta)
        {
            backSR.color = new Color(1, 1, 0, 0.4f);
        }
        else
        {
            backSR.color = new Color(1, 0, 0, 0.4f);
        }
        itemSR.sprite=IItem.GetItemImage();
    }
    private void OnMouseDown()
    {
        isSelected = !isSelected;
        selectedGO.SetActive(isSelected);
        Color newColor = backSR.color;
        if (isSelected)
        {
            newColor.a = 0.4f;
            SCM.AddCollectedItem(II);
        }
        else
        {
            newColor.a = 0.1f;
            SCM.RemoveCollectedItem(II);
        }
        backSR.color = newColor;

    }
    private void OnMouseEnter()
    {
        STT.ShowToolTip(transform,II);
    }
    private void OnMouseExit()
    {
        STT.HideToolTip();
    }
    private void DestroyThis()
    {
        Destroy(gameObject);
    }
}
