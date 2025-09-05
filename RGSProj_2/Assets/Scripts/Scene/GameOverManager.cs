using UnityEngine;

namespace Scene
{
    /// <summary>
    /// 게임오버 화면에서 스페이스 키 입력을 감지하여
    /// 게임 씬을 다시 로드합니다.
    /// </summary>
    public class GameOverManager : MonoBehaviour
    {
        #region Unity Callbacks

        /// <summary>
        /// 매 프레임 스페이스 키를 확인하고,
        /// 누르면 게임 씬으로 전환합니다.
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

