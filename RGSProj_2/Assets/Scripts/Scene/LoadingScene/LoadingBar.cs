using UnityEngine;
using UnityEngine.UI;
using Core;
public class LoadingBar : MonoBehaviour
{
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();

    }
    private void Update()
    {
        image.fillAmount = Loader.GetLoadingProgress();
    }
}
