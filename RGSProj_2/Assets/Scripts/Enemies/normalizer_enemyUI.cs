using UnityEngine;
using System.Collections;
public class normalizer_enemyUI : MonoBehaviour
{
    [SerializeField] private RectTransform normalizer;
    public void Normalize()
    {
        if (normalizer == null) return;

        int count = transform.childCount;
        if (count == 0) return;

        RectTransform lastActiveChild = null;

        // �ڿ������� Ȱ��ȭ�� �ڽ��� ã�Ƽ�
        for (int i = count - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            if (child.gameObject.activeSelf)
            {
                lastActiveChild = child as RectTransform;
                break;
            }
        }

        if (lastActiveChild == null) return;

        float targetWidth = lastActiveChild.rect.width; // rect.width�� �� ������

        Vector2 size = normalizer.sizeDelta;
        size.x = targetWidth;
        normalizer.sizeDelta = size;
    }
}
