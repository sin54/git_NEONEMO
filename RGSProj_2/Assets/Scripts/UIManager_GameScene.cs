using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using InventorySystem;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class UIManager_GameScene : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject invenPanel;
    [SerializeField] private GameObject skillPF;
    [SerializeField] private Transform content;
    public List<BaseSkill> activeSkills = new List<BaseSkill>();

    [Header("ItemInfo")]
    [SerializeField] private TMP_Text itemNameTxt;
    [SerializeField] private TMP_Text itemDescriptionTxt;
    [SerializeField] private TMP_Text itemRarityTxt;
    [SerializeField] private TMP_Text itemAbilityTxt;
    [SerializeField] private TMP_Text itemSynergeTxt;
    [SerializeField] private TMP_Text itemTagTxt;

    [Header("Stats")]
    [SerializeField] private TMP_Text statsText;

    [Header("Type")]
    [SerializeField] private TMP_Text typePassiveText;
    [SerializeField] private TMP_Text[] typeActiveText = new TMP_Text[5];
    [SerializeField] private TMP_Text typePassiveLvTxt;
    [SerializeField] private TMP_Text typeActiveLvTxt;
    [SerializeField] private TMP_Text typeNameTxt;
    [SerializeField] private Button[] typeButton = new Button[7];
    [SerializeField] private PlayerTypeManager TM;

    [Header("Inventory")]
    public bool isItemDragging;
    [SerializeField] private GameObject[] inventories;
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private GameObject trashPanel;
    [SerializeField] private RectTransform inventoryPivoter;
    public bool isTrashOn;

    [Header("Change")]
    [SerializeField]private Vector3 pos_inventory;
    [SerializeField]private Vector3 pos_change;
    [SerializeField] private GameObject ChangePanel;
    [SerializeField] private GameObject Change_StatPanel;
    [SerializeField] private GameObject Change_InfoPanel;
    [SerializeField] private TMP_Text changeStatTxt;
    [SerializeField] private TMP_Text buttonTxt;
    private Queue<InventoryItem> changeQueue = new Queue<InventoryItem>();
    private bool isStatOpen;
    public bool isOpenUI {  get; private set; }
    public bool isOpenInven { get; private set; }
    public bool isOpenChange { get; private set; }

    private int selectTypeNum;
    private void Awake()
    {
        isOpenUI = false;
        selectTypeNum = 0;
    }
    private void Start()
    {
        isOpenUI = false;
        isOpenInven = false;
        isOpenChange = false;
        isStatOpen = false;
        pausePanel.SetActive(false);
        invenPanel.SetActive(false);
        ChangePanel.SetActive(false);
        SetInventoryVisibility(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&&!GameManager.instance.player.isPlayerDeath)
        {
            if (isOpenUI)
            {
                isOpenUI=false;
                selectTypeNum = 0;
                TimeScaleManager.Instance.TimeStopStackMinus();
                pausePanel.SetActive(false);
            }
            else
            {
                isOpenUI = true;
                selectTypeNum = 0;
                SetTypeTxt();
                TimeScaleManager.Instance.TimeStopStackPlus();
                pausePanel.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab) && !GameManager.instance.player.isPlayerDeath && !isOpenUI&&!isOpenChange)
        {
            if (isOpenInven)
            {
                isOpenInven = false;
                TimeScaleManager.Instance.TimeStopStackMinus();
                invenPanel.SetActive(false);
                SetInventoryVisibility(false);
            }
            else
            {
                isOpenInven = true;
                inventoryPivoter.localPosition = pos_inventory;
                TimeScaleManager.Instance.TimeStopStackPlus();
                invenPanel.SetActive(true);
                SetInventoryVisibility(true);
                //test
                InventoryController.instance.AddPassiveItem("Èò ¿¬±â", 1);
                InventoryController.instance.AddPassiveItem("Ä«Å¸³ª", 1);
                InventoryController.instance.AddPassiveItem("ºÓÀº µ¹¸æÀÌ", 1);

            }
        }
        if (GameManager.instance.player.isPlayerDeath)
        {
            if (isOpenUI)
            {
                isOpenUI = false;
                TimeScaleManager.Instance.TimeStopStackMinus();
                pausePanel.SetActive(false);
            }
            if (isOpenInven)
            {
                isOpenInven = false;
                TimeScaleManager.Instance.TimeStopStackMinus();
                invenPanel.SetActive(false);
                SetInventoryVisibility(false);
            }
        }
        if (isOpenInven)
        {
            statsText.text = GetStatsTxt();
            statsPanel.SetActive(!isItemDragging);
            trashPanel.SetActive(isItemDragging);
        }
        if (isOpenChange)
        {
            changeStatTxt.text=GetStatsTxt();
        }
    }
    public void Resume()
    {
        isOpenUI = false;
        TimeScaleManager.Instance.TimeStopStackMinus();
        pausePanel.SetActive(false);
    }

    public void AddSkill(BaseSkill BS)
    {
        activeSkills.Add(BS);
        UpdateAll();
    }
    public void UpdateAll()
    {
        DestoryChild(content.gameObject);
        for (int i = 0; i < activeSkills.Count; i++) {
            GameObject GO = Instantiate(skillPF, content);
            GO.GetComponent<skillPFInit>().Init(activeSkills[i].baseSkillData.itemIcon, activeSkills[i].baseSkillData.itemName, activeSkills[i].itemLevel + 1, GameManager.instance.playerTypeManager.Colors[activeSkills[i].TC.typeCode]);
        }
    }
    void DestoryChild(GameObject parentObject)
    {
        Transform[] childList = parentObject.GetComponentsInChildren<Transform>(true);
        if (childList != null)
        {
            for (int i = 1; i < childList.Length; i++)
            {
                if (childList[i] != transform)
                    Destroy(childList[i].gameObject);
            }
        }
    }

    public void SetItemDescription(InventoryItem item)
    {
        if (item==null||item.GetSkillEvent()==null)
        {
            return;
        }
        itemNameTxt.text = "["+item.GetSkillEvent().itemName+"]";
        itemDescriptionTxt.text = item.GetSkillEvent().itemDescription;
        itemAbilityTxt.text = item.GetSkillEvent().itemAbility;
        itemRarityTxt.text = UtilClass.ToLocalizedString(item.GetRarity());
        itemSynergeTxt.text = item.GetSkillEvent().synergeString;
        itemTagTxt.text = UtilClass.ToLocalizedString(item.GetTags());  
    }

    private string GetStatsTxt()
    {
        string returnstring = 
            @$"{GameManager.instance.player.playerCurrentHealth:F1}/{GameManager.instance.player.playerMaxHealth:F1}
{GameManager.instance.SM.GetFinalValue("NaturalHeal")}/s

{Mathf.RoundToInt(GameManager.instance.SM.GetFinalValue("defenceRate") * 100)}%
{Mathf.RoundToInt((1f-GameManager.instance.SM.GetFinalValue("dodgeMul"))*100)}%
{Mathf.RoundToInt(GameManager.instance.SM.GetFinalValue("PlayerSpeed")*100)}

{Mathf.RoundToInt(GameManager.instance.SM.GetFinalValue("CriticalPercent") * 100)}%
x{GameManager.instance.SM.GetFinalValue("CriticalMul"):F2}

x{GameManager.instance.SM.GetFinalValue("AtkMul"):F2}
x{GameManager.instance.SM.GetFinalValue("P_AtkMul"):F2}
x{GameManager.instance.SM.GetFinalValue("M_AtkMul"):F2}
{Mathf.RoundToInt(GameManager.instance.SM.GetFinalValue("CoolReduce") * 100)}%
x{GameManager.instance.SM.GetFinalValue("KnockBackMul"):F2}

x{GameManager.instance.SM.GetFinalValue("xpMul"):F2}
{GameManager.instance.SM.GetFinalValue("ItemRange"):F2}
";

        return returnstring;
    }

    public void UpdateSelected(int num)
    {
        selectTypeNum = num;
        SetTypeTxt();
    }
    private void SetTypeTxt()
    {
        for (int i = 0; i < typeButton.Length; i++) {
            typeButton[i].interactable = false;
        }
        for (int i = 0; i < TM.NowType.Count; i++) {
            typeButton[TM.NowType[i]].interactable = true;
        }
        BaseType selectedType = TM.Types[selectTypeNum];
        typePassiveText.text = selectedType.typeData.passiveTxt[selectedType.typePassiveLevel];
        for(int i = 0; i < 5; i++)
        {
            typeActiveText[i].text=selectedType.typeData.activeTxt[i];
            if (i >= selectedType.typeActiveLevel)
            {
                typeActiveText[i].color = new Color(0.48f, 0.48f, 0.48f, 1f);
            }
            else
            {
                typeActiveText[i].color = Color.white;
            }
        }
        typePassiveLvTxt.text = "Lv " + (selectedType.typePassiveLevel+1).ToString();
        typeActiveLvTxt.text = "Lv " + selectedType.typeActiveLevel.ToString();
        typeNameTxt.text = "["+selectedType.typeData.typeName+"]";
    }
    private void SetInventoryVisibility(bool active)
    {
        for(int i = 0; i < 3; i++)
        {
            inventories[i].SetActive(active);
        }
    }

    public void ShowChangePanel(InventoryItem ChangeItem)
    {
        if (isOpenChange)
        {
            changeQueue.Enqueue(ChangeItem);
            return;
        }

        TimeScaleManager.Instance.TimeStopStackPlus();
        isOpenChange = true;
        isStatOpen = false;
        Change_StatPanel.SetActive(isStatOpen);
        Change_InfoPanel.SetActive(!isStatOpen);
        buttonTxt.text = "½ºÅÈ ÄÑ±â";

        InventoryController.instance.AddItem("Change", ChangeItem.GetItemType());

        inventories[0].SetActive(false);
        inventoryPivoter.localPosition = pos_change;
        ChangePanel.SetActive(true);
    }
    public void ChangeBetweenStats()
    {
        isStatOpen = !isStatOpen;
        Change_StatPanel.SetActive(isStatOpen);
        Change_InfoPanel.SetActive(!isStatOpen);
        if (isStatOpen)
        {
            buttonTxt.text = "½ºÅÈ ²ô±â";
        }
        else{
            buttonTxt.text = "½ºÅÈ ÄÑ±â";
        }
    }
    public void EndChange()
    {
        isOpenChange = false;
        inventories[0].SetActive(true);
        inventoryPivoter.localPosition = pos_inventory;
        InventoryController.instance.InventoryClear("Change");
        ChangePanel.SetActive(false);
        TimeScaleManager.Instance.TimeStopStackMinus();

        if (changeQueue.Count > 0)
        {
            var nextItem = changeQueue.Dequeue();
            ShowChangePanel(nextItem);

        }
    }
}
