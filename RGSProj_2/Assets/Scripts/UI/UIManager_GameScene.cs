using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using InventorySystem;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using Core;
using Type;

namespace UI
{
    /// <summary>
    /// 게임 씬 내 UI 전체(일시정지, 인벤토리, 스킬, 아이템 정보, 스탯, 타입 변경 등)를
    /// 관리하는 클래스입니다.
    /// </summary>
    [DisallowMultipleComponent]
    public class UIManager_GameScene : MonoBehaviour
    {
        #region Serialized Fields

        [Header("Pause UI")]
        [Tooltip("ESC 키 입력 시 표시할 일시정지 패널")]
        [SerializeField] private GameObject pausePanel;

        [Header("Inventory UI")]
        [Tooltip("Tab 키 입력 시 표시할 인벤토리 패널")]
        [SerializeField] private GameObject invenPanel;
        [Tooltip("인벤토리 슬롯을 배치할 Content")]
        [SerializeField] private Transform content;
        [Tooltip("인벤토리 슬롯 프리팹 배열")]
        [SerializeField] private GameObject[] inventories;
        [Tooltip("인벤토리에서 드래그 중이지 않을 때 표시할 스탯 패널")]
        [SerializeField] private GameObject statsPanel;
        [Tooltip("아이템 드래그 중 표시할 휴지통 패널")]
        [SerializeField] private GameObject trashPanel;
        [Tooltip("인벤토리 패널 위치 전환용 RectTransform")]
        [SerializeField] private RectTransform inventoryPivoter;

        [Header("Skill UI")]
        [Tooltip("액티브 스킬을 생성할 Prefab")]
        [SerializeField] private GameObject skillPF;
        [Tooltip("현재 활성화된 액티브 스킬 목록")]
        [SerializeField] public List<BaseSkill> activeSkills = new List<BaseSkill>();

        [Header("Item Info UI")]
        [Tooltip("아이템 명칭 텍스트")]
        [SerializeField] private TMP_Text itemNameTxt;
        [Tooltip("아이템 설명 텍스트")]
        [SerializeField] private TMP_Text itemDescriptionTxt;
        [Tooltip("아이템 레어도 텍스트")]
        [SerializeField] private TMP_Text itemRarityTxt;
        [Tooltip("아이템 능력치 텍스트")]
        [SerializeField] private TMP_Text itemAbilityTxt;
        [Tooltip("아이템 시너지 텍스트")]
        [SerializeField] private TMP_Text itemSynergeTxt;
        [Tooltip("아이템 태그 텍스트")]
        [SerializeField] private TMP_Text itemTagTxt;

        [Header("Stats UI")]
        [Tooltip("플레이어 스탯 요약 텍스트")]
        [SerializeField] private TMP_Text statsText;

        [Header("Type UI")]
        [Tooltip("패시브 타입 텍스트")]
        [SerializeField] private TMP_Text typePassiveText;
        [Tooltip("액티브 타입 텍스트 배열(최대 5)")]
        [SerializeField] private TMP_Text[] typeActiveText = new TMP_Text[5];
        [Tooltip("패시브 레벨 텍스트")]
        [SerializeField] private TMP_Text typePassiveLvTxt;
        [Tooltip("액티브 레벨 텍스트")]
        [SerializeField] private TMP_Text typeActiveLvTxt;
        [Tooltip("타입 이름 텍스트")]
        [SerializeField] private TMP_Text typeNameTxt;
        [Tooltip("타입 변경 버튼 배열(최대 7)")]
        [SerializeField] private Button[] typeButton = new Button[7];
        [Tooltip("PlayerTypeManager 참조")]
        [SerializeField] private PlayerTypeManager TM;

        [Header("Change UI")]
        [Tooltip("인벤토리↔교체 패널 위치 전환용 인벤토리 기준 좌표")]
        [SerializeField] private Vector3 pos_inventory;
        [Tooltip("인벤토리↔교체 패널 위치 전환용 교체 기준 좌표")]
        [SerializeField] private Vector3 pos_change;
        [Tooltip("교체(Change) 패널")]
        [SerializeField] private GameObject ChangePanel;
        [Tooltip("교체 시 스탯 비교 패널")]
        [SerializeField] private GameObject Change_StatPanel;
        [Tooltip("교체 시 아이템 정보 패널")]
        [SerializeField] private GameObject Change_InfoPanel;
        [Tooltip("교체 패널의 스탯 텍스트")]
        [SerializeField] private TMP_Text changeStatTxt;
        [Tooltip("교체 확정 버튼 텍스트")]
        [SerializeField] private TMP_Text buttonTxt;

        [Tooltip("교체 패널 왼쪽 아이템 이름 텍스트")]
        [SerializeField] private TMP_Text L_itemNameTxt;
        [SerializeField] private TMP_Text L_itemDescriptionTxt;
        [SerializeField] private TMP_Text L_itemRarityTxt;
        [SerializeField] private TMP_Text L_itemAbilityTxt;
        [SerializeField] private TMP_Text L_itemSynergeTxt;
        [SerializeField] private TMP_Text L_itemTagTxt;

        [Tooltip("교체 패널 오른쪽 아이템 정보 텍스트")]
        [SerializeField] private TMP_Text R_itemNameTxt;
        [SerializeField] private TMP_Text R_itemDescriptionTxt;
        [SerializeField] private TMP_Text R_itemRarityTxt;
        [SerializeField] private TMP_Text R_itemAbilityTxt;
        [SerializeField] private TMP_Text R_itemSynergeTxt;
        [SerializeField] private TMP_Text R_itemTagTxt;

        #endregion

        #region Public Fields & Properties

        /// <summary>현재 UI(ESC 패널) 오픈 여부</summary>
        public bool isOpenUI { get; private set; }

        /// <summary>현재 인벤토리 패널 오픈 여부</summary>
        public bool isOpenInven { get; private set; }

        /// <summary>현재 교체(Change) 패널 오픈 여부</summary>
        public bool isOpenChange { get; private set; }

        /// <summary>아이템 드래그 중 여부</summary>
        public bool isItemDragging;

        /// <summary>휴지통 활성 상태</summary>
        public bool isTrashOn;

        #endregion

        #region Private Fields

        private Queue<InventoryItem> changeQueue = new Queue<InventoryItem>();
        private bool isStatOpen;
        private int selectTypeNum;

        #endregion

        #region Unity Callbacks

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
            StartCoroutine(InitInven());
        }
        private void Update()
        {
            // ESC: 일시정지 토글
            if (Input.GetKeyDown(KeyCode.Escape)&&!GameManager.Instance.player.isPlayerDeath)
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

            // Tab: 인벤토리 토글
            if (Input.GetKeyDown(KeyCode.Tab) && !GameManager.Instance.player.isPlayerDeath && !isOpenUI&&!isOpenChange)
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
                    //InventoryController.instance.AddPassiveItem("불타는 땅의 왕홀", 1);
                }
            }

            // 플레이어 사망 시 모든 UI 닫기
            if (GameManager.Instance.player.isPlayerDeath)
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

            // 인벤토리 열림 상태에서 스탯/휴지통 토글
            if (isOpenInven)
            {
                statsText.text = GetStatsTxt();
                statsPanel.SetActive(!isItemDragging);
                trashPanel.SetActive(isItemDragging);
            }

            // Change UI 열림 상태에서 데이터 갱신
            if (isOpenChange)
            {
                changeStatTxt.text=GetStatsTxt();
                SetChangeItemDescription(InventoryController.instance.GetItem("Change", 0));
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 신규 스킬을 추가하고 스킬 UI를 갱신합니다.
        /// </summary>
        public void AddSkill(BaseSkill BS)
        {
            activeSkills.Add(BS);
            UpdateAll();
        }

        /// <summary>
        /// 현재 활성 스킬을 모두 인스턴트화하여 스킬 패널을 갱신합니다.
        /// </summary>
        public void UpdateAll()
        {
            DestoryChild(content.gameObject);
            for (int i = 0; i < activeSkills.Count; i++) {
                GameObject GO = Instantiate(skillPF, content);
                GO.GetComponent<skillPFInit>().Init(activeSkills[i].baseSkillData.itemIcon, activeSkills[i].baseSkillData.itemName, activeSkills[i].itemLevel + 1, GameManager.Instance.playerTypeManager.Colors[activeSkills[i].TC.typeCode]);
            }
        }
        
        /// <summary>
        /// 선택된 아이템의 상세 설명을 좌측 패널에 설정합니다.
        /// </summary>
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

            R_itemNameTxt.text = "[" + item.GetSkillEvent().itemName + "]";
            R_itemDescriptionTxt.text = item.GetSkillEvent().itemDescription;
            R_itemAbilityTxt.text = item.GetSkillEvent().itemAbility;
            R_itemRarityTxt.text = UtilClass.ToLocalizedString(item.GetRarity());
            R_itemSynergeTxt.text = item.GetSkillEvent().synergeString;
            R_itemTagTxt.text = UtilClass.ToLocalizedString(item.GetTags());
        }

        /// <summary>
        /// 선택된 타입 인덱스를 갱신하고 UI를 업데이트합니다.
        /// </summary>
        /// <param name="num">새로 선택된 타입 인덱스</param>
        public void UpdateSelected(int num)
        {
            selectTypeNum = num;
            SetTypeTxt();
        }

        /// <summary>
        /// 교체할 아이템을 전달받아 교체 패널을 열고 초기화합니다.
        /// </summary>
        /// <param name="ChangeItem">교체할 InventoryItem</param>
        public void ShowChangePanel(InventoryItem ChangeItem)
        {
            if (isOpenChange)
            {
                changeQueue.Enqueue(ChangeItem);
                return;
            }
            SetItemDescription(ChangeItem);
            SetChangeItemDescription(ChangeItem);
            TimeScaleManager.Instance.TimeStopStackPlus();
            isOpenChange = true;
            isStatOpen = false;
            Change_StatPanel.SetActive(isStatOpen);
            Change_InfoPanel.SetActive(!isStatOpen);
            buttonTxt.text = "스탯 열기";

            InventoryController.instance.AddItem("Change", ChangeItem.GetItemType());

            SetInventoryVisibility(true);
            inventories[0].SetActive(false);
           
            inventoryPivoter.localPosition = pos_change;
            ChangePanel.SetActive(true);
        }

        /// <summary>
        /// 교체 패널 내 스탯/정보 탭을 전환합니다.
        /// </summary>
        public void ChangeBetweenStats()
        {
            isStatOpen = !isStatOpen;
            Change_StatPanel.SetActive(isStatOpen);
            Change_InfoPanel.SetActive(!isStatOpen);
            if (isStatOpen)
            {
                buttonTxt.text = "스탯 끄기";
            }
            else{
                buttonTxt.text = "스탯 켜기";
            }
        }
        
        /// <summary>
        /// 교체 과정을 종료하고 대기 중인 아이템이 있으면 다음 아이템을 처리합니다.
        /// </summary>
        public void EndChange()
        {
            isOpenChange = false;
            inventories[0].SetActive(true);
            inventoryPivoter.localPosition = pos_inventory;
            InventoryController.instance.InventoryClear("Change");
            ChangePanel.SetActive(false);
            SetInventoryVisibility(isOpenInven);
            TimeScaleManager.Instance.TimeStopStackMinus();

            if (changeQueue.Count > 0)
            {
                var nextItem = changeQueue.Dequeue();
                ShowChangePanel(nextItem);

            }
        }

        /// <summary>
        /// 일시정지 UI를 닫고 게임을 재개합니다.
        /// </summary>
        public void Resume()
        {
            isOpenUI = false;
            TimeScaleManager.Instance.TimeStopStackMinus();
            pausePanel.SetActive(false);
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// 주어진 부모 오브젝트의 모든 자식 오브젝트를 파괴합니다.
        /// </summary>
        private void DestoryChild(GameObject parentObject)
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

        /// <summary>
        /// 인벤토리 슬롯 콘텐츠의 표시 여부를 설정합니다.
        /// </summary>
        private void SetInventoryVisibility(bool active)
        {
            for(int i = 0; i < 3; i++)
            {
                inventories[i].SetActive(active);
            }
        }

        /// <summary>
        /// Pause UI 내 타입 설명과 버튼 상태를 갱신합니다.
        /// </summary>
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

        /// <summary>
        /// 플레이어 상태를 문자열로 조합하여 반환합니다.
        /// </summary>
        private string GetStatsTxt()
        {
            string returnstring = 
                @$"{GameManager.Instance.player.playerCurrentHealth:F1}/{GameManager.Instance.player.playerMaxHealth:F1}
    {GameManager.Instance.SM.GetFinalValue("NaturalHeal")}/s

    {Mathf.RoundToInt(GameManager.Instance.SM.GetFinalValue("defenceRate") * 100)}%
    {Mathf.RoundToInt((1f-GameManager.Instance.SM.GetFinalValue("dodgeMul"))*100)}%
    {Mathf.RoundToInt(GameManager.Instance.SM.GetFinalValue("PlayerSpeed")*100)}

    {Mathf.RoundToInt(GameManager.Instance.SM.GetFinalValue("CriticalPercent") * 100)}%
    x{GameManager.Instance.SM.GetFinalValue("CriticalMul"):F2}

    x{GameManager.Instance.SM.GetFinalValue("AtkMul"):F2}
    x{GameManager.Instance.SM.GetFinalValue("P_AtkMul"):F2}
    x{GameManager.Instance.SM.GetFinalValue("M_AtkMul"):F2}
    {Mathf.RoundToInt(GameManager.Instance.SM.GetFinalValue("CoolReduce") * 100)}%
    x{GameManager.Instance.SM.GetFinalValue("KnockBackMul"):F2}

    x{GameManager.Instance.SM.GetFinalValue("xpMul"):F2}
    {GameManager.Instance.SM.GetFinalValue("ItemRange"):F2}
    ";

            return returnstring;
        }

        /// <summary>
        /// 교체 패널 우측에 표시할 아이템 설명을 설정합니다.
        /// </summary>
        public void SetChangeItemDescription(InventoryItem item)
        {
            if (item == null || item.GetSkillEvent() == null)
            {
                return;
            }
            L_itemNameTxt.text = "[" + item.GetSkillEvent().itemName + "]";
            L_itemDescriptionTxt.text = item.GetSkillEvent().itemDescription;
            L_itemAbilityTxt.text = item.GetSkillEvent().itemAbility;
            L_itemRarityTxt.text = UtilClass.ToLocalizedString(item.GetRarity());
            L_itemSynergeTxt.text = item.GetSkillEvent().synergeString;
            L_itemTagTxt.text = UtilClass.ToLocalizedString(item.GetTags());
        }
        IEnumerator InitInven()
        {
            SetInventoryVisibility(false);
            
            SetInventoryVisibility(true);
            yield return new WaitForEndOfFrame();
            SetInventoryVisibility(false);
        }
        #endregion
    }
}