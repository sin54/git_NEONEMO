using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class TrashCan : MonoBehaviour
{
    private Image panelImage;
    private RectTransform rect;

    [SerializeField] private Color normalColor = new Color(0.3f, 0.3f, 0.3f, 1f); // 기본 색
    [SerializeField] private Color highlightColor = new Color(0.6f, 0.6f, 0.6f, 1f); // 밝은 색
    [SerializeField] private UIManager_GameScene UIM;



    private void Awake()
    {
        panelImage = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        panelImage.color = normalColor;
    }

    private void Update()
    {
        float mouseX = Input.mousePosition.x;

        // 패널의 영역 가져오기
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);
        float left = corners[0].x;
        float right = corners[2].x;

        // x좌표만 비교
        if (mouseX >= left && mouseX <= right)
        {
            panelImage.color = highlightColor;
            UIM.isTrashOn = true;
        }
        else
        {
            panelImage.color = normalColor;
            UIM.isTrashOn= false;
        }
    }
}
