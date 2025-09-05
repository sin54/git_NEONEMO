using UnityEngine;
using System.Collections;
using Core;
public class MoveToOrigin : MonoBehaviour
{
    [SerializeField] private ParticleSystem PS;
    private RepairManager RP;
    private void Awake()
    {
        RP=GameObject.Find("RepairManager").GetComponent<RepairManager>();
    }
    private void Start()
    {
        StartCoroutine(MoveOverTime(Vector3.zero, 0.8f));
    }

    private IEnumerator MoveOverTime(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        transform.position = targetPosition; // 정확히 0,0 보정
        yield return new WaitForSeconds(0.1f);

        RP.HealPlayer();
        Destroy(gameObject);
    }
}
