using UnityEngine;
using UnityEngine.UI;
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
