using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UI;
public class TrashCan : MonoBehaviour
{
    private Image panelImage;
    private RectTransform rect;

    [SerializeField] private Color normalColor = new Color(0.3f, 0.3f, 0.3f, 1f); // �⺻ ��
    [SerializeField] private Color highlightColor = new Color(0.6f, 0.6f, 0.6f, 1f); // ���� ��
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

        // �г��� ���� ��������
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);
        float left = corners[0].x;
        float right = corners[2].x;

        // x��ǥ�� ��
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
