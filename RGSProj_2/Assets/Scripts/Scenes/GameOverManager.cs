using UnityEngine;
using Core;

namespace Scenes
{
    /// <summary>
    /// ���ӿ��� ȭ�鿡�� �����̽� Ű �Է��� �����Ͽ�
    /// ���� ���� �ٽ� �ε��մϴ�.
    /// </summary>
    public class GameOverManager : MonoBehaviour
    {
        #region Unity Callbacks

        /// <summary>
        /// �� ������ �����̽� Ű�� Ȯ���ϰ�,
        /// ������ ���� ������ ��ȯ�մϴ�.
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Loader.Load(Loader.Scene.GameScene);
            }
        }

        #endregion
    }
}

