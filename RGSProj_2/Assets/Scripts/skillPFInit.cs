using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class skillPFInit : MonoBehaviour
{
    [SerializeField] private Image skillImg;
    [SerializeField] private TMP_Text skillName;
    [SerializeField] private TMP_Text skillLv;

    public void Init(Sprite img,string skilName,int skilLv,Color color)
    {
        skillImg.sprite = img;
        skillName.text = skilName;
        skillLv.text=skilLv.ToString();
        skillName.color = color;
        if (skilLv == 5)
        {
            skillLv.color = Color.red;
        }
        else
        {
            skillLv.color = Color.white;
        }
    }
}
