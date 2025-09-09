using UnityEngine;

namespace Core
{
    /// <summary>
    /// �ݹ� ��ũ��Ʈ
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