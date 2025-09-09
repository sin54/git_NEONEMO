using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatePrefab : MonoBehaviour
{
    [SerializeField] private TMP_Text stateTxt;
    [SerializeField] private Image stateImg;

    protected RectTransform RT;

    private void Awake()
    {
        RT=GetComponent<RectTransform>();
    }
    public void SetPanel(string text, Sprite img,Color textColor)
    {
        stateTxt.text = text;
        stateTxt.color = textColor;
        stateImg.sprite = img;
        Vector2 size = RT.sizeDelta;
        size.x = stateTxt.preferredWidth + stateImg.rectTransform.sizeDelta.x + 0.03f;
        RT.sizeDelta = size;
    }
}
