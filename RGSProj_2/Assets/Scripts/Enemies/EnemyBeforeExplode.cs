using UnityEngine;
using System.Collections;

public class EnemyBeforeExplode : MonoBehaviour
{
    private SpriteRenderer sr;
    private Gradient gradient;
    private float duration;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Play(Gradient gradient, float duration)
    {
        this.gradient = gradient;
        this.duration = duration;

        StopAllCoroutines();
        StartCoroutine(ChangeColorOverTime());
    }

    private IEnumerator ChangeColorOverTime()
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            sr.color = gradient.Evaluate(t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        sr.color = gradient.Evaluate(1f);
    }
}
