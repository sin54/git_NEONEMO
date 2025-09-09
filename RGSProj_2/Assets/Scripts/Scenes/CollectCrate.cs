using UnityEngine;
using System.Collections;
using NUnit.Framework.Internal.Commands;
using Scenes;
using Core;

public class CollectCrate : MonoBehaviour
{
    [SerializeField] private CollectSceneManager CSM;
    [SerializeField] private GameObject crateClickParticle;
    [SerializeField] private GameObject EndBtn;

    private Vector3 originalScale;
    private Vector3 targetScale;
    private float scaleSpeed = 5f; // Ŀ����/�پ��� �ӵ�
    private float maxScaleMultiplier = 1.3f;

    private bool isShaking = false;

    private int crateClick;
    private float spawnPercent;

    private void Start()
    {
        EndBtn.SetActive(false);
        crateClick = CSM.maxClickNum;
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    private void OnMouseEnter()
    {
        targetScale = originalScale * maxScaleMultiplier;
    }

    private void OnMouseExit()
    {
        targetScale = originalScale;
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
    }

    private void OnMouseDown()
    {
        if (crateClick <= 0) return;
        // ��ƼŬ ����
        if (crateClickParticle != null)
        {
            Instantiate(crateClickParticle, transform.position, Quaternion.identity);
        }
        // ��鸲 ȿ�� ����
        if (!isShaking)
        {
            StartCoroutine(ShakeObject(0.3f, 0.1f));
            // 0.3f : ��鸲 ���ӽð�, 0.1f : ��鸲 ����
        }
        crateClick--;
        if (UtilClass.GetPercent(spawnPercent))
        {
            CSM.SpawnItem();
            spawnPercent -= CSM.decreasePercent;
            if (spawnPercent < 0f) spawnPercent = 0f;
        }
        else
        {
            spawnPercent += CSM.increasePercent;
            if (spawnPercent >= 1f) spawnPercent = 1f;
        }

        if (crateClick <= 0)
        {
            gameObject.SetActive(false);

            CSM.isBoxBreak = true;
            EndBtn.SetActive(true);
        }
        
    }

    private IEnumerator ShakeObject(float duration, float magnitude)
    {
        isShaking = true;
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            // �¿� ��鸲 (sin �ĵ�)
            float x = Mathf.Sin(elapsed * 50f) * magnitude;
            // ���ϵ� �ణ ��鸲 �߰�
            float y = Mathf.Sin(elapsed * 70f) * (magnitude * 0.5f);

            transform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
        isShaking = false;
    }


}
