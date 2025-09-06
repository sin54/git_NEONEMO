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

        // 뒤에서부터 활성화된 자식을 찾아서
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

        float targetWidth = lastActiveChild.rect.width; // rect.width가 더 안전함

        Vector2 size = normalizer.sizeDelta;
        size.x = targetWidth;
        normalizer.sizeDelta = size;
    }
}
