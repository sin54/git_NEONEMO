using NUnit.Framework;
using TMPro;
using UnityEngine;

namespace Scene
{

    /// <summary>
    /// 로딩 씬에서 랜덤한 안내 문구를 선택해 화면에 표시합니다.
    /// </summary>
    [DisallowMultipleComponent]
    public class LoadingSceneManager : MonoBehaviour
    {
        #region Serialized Fields

        /// <summary>
        /// 로딩 중 표시할 안내 문자열 목록
        /// </summary>
        [Tooltip("로딩 화면에 표시할 메시지를 문자열 배열로 설정하세요.")]
        [SerializeField] private string[] infos;

        /// <summary>
        /// 안내 문구를 출력할 TMP_Text 컴포넌트
        /// </summary>
        [SerializeField] private TMP_Text info_txt;

        #endregion

        #region Unity Callbacks

        /// <summary>
        /// 씬 시작 시 infos 배열에서 랜덤으로 하나를 선택해 텍스트로 설정합니다.
        /// </summary>
        void Start()
        {
            info_txt.text = infos[Random.Range(0, infos.Length)];
        }

        #endregion
    }
}


