using UnityEngine;

namespace Core
{
    /// <summary>
    /// 콜백 스크립트
    /// </summary>
    public class LoaderCallback : MonoBehaviour
    {
        private bool isFirstUpdate = true;

        private void Update()
        {
            if (isFirstUpdate)
            {
                isFirstUpdate = false;
                Loader.LoaderCallback();
            }
        }
    }
}