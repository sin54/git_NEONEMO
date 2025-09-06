using System.Collections;
using UnityEngine;

public class BlinkText : MonoBehaviour
{
    [SerializeField] private GameObject blinkObj;
    [SerializeField] private float blinkSpeed;
    private void Start()
    {
        StartCoroutine(blinKText());
    }
    private IEnumerator blinKText()
    {
        while (true)
        {
            yield return new WaitForSeconds(blinkSpeed);
            blinkObj.SetActive(!blinkObj.activeSelf);
        }
    }
}
