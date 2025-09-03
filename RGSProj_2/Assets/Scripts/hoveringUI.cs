using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class hoveringUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isHovering;
    public bool isFinalUpgrade;
    [SerializeField] private Image panelImg;
    [SerializeField] private Image panel_subImage;
    [SerializeField] private TMP_Text weaponName;
    [SerializeField] private TMP_Text weaponDescrip;
    [SerializeField] private TMP_Text weaponLevel;
    [SerializeField] private Image weaponImg;
    public void OnEnable()
    {
        isHovering = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        Color panelColor = panelImg.color;
        panelColor.a = 150.0f / 255f;
        panelImg.color = panelColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        Color panelColor = panelImg.color;
        panelColor.a = 101.0f / 255f;
        panelImg.color = panelColor;
    }

    public void SetPanel(BaseSkill BS)
    {
        weaponName.text = BS.baseSkillData.itemName;
        weaponDescrip.text = BS.baseSkillData.itemDescription[BS.itemLevel+1];

        weaponLevel.text = new string('*', BS.itemLevel + 2);
        if (BS.itemLevel == 3)
        {
            isFinalUpgrade = true;
        }
        else
        {
            isFinalUpgrade = false;
        }
        weaponLevel.color = Color.white;
        panelImg.color = BS.TC.typeData.typeColors;
        panel_subImage.color = BS.TC.typeData.typeColors;
    }

    public void SetPanel(MaxLevelData maxLevelData)
    {
        weaponName.text = maxLevelData.itemName;
        weaponDescrip.text = maxLevelData.itemDescription;

        weaponLevel.text = new string('*', 5);
    }

    public void SetPanel(BaseType TC)
    {
        if (TC.typePassiveLevel == 3)
        {
            isFinalUpgrade = true;
        }
        else
        {
            isFinalUpgrade = false;
        }
        weaponName.text = TC.typeData.typeName;
        weaponDescrip.text = TC.typeData.typeDesc[TC.typePassiveLevel+1];
        weaponLevel.text= new string('*', TC.typePassiveLevel + 2);
        weaponLevel.color = Color.white;
        panelImg.color = TC.typeData.typeColors;
        panel_subImage.color = TC.typeData.typeColors;
    }
}
