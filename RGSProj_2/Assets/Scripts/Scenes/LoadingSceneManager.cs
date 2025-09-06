using NUnit.Framework;
using TMPro;
using UnityEngine;

namespace Scenes
{

    /// <summary>
    /// �ε� ������ ������ �ȳ� ������ ������ ȭ�鿡 ǥ���մϴ�.
    /// </summary>
    [DisallowMultipleComponent]
    public class LoadingSceneManager : MonoBehaviour
    {
        #region Serialized Fields

        /// <summary>
        /// �ε� �� ǥ���� �ȳ� ���ڿ� ���
        /// </summary>
        [Tooltip("�ε� ȭ�鿡 ǥ���� �޽����� ���ڿ� �迭�� �����ϼ���.")]
        [SerializeField] private string[] infos;

        /// <summary>
        /// �ȳ� ������ ����� TMP_Text ������Ʈ
        /// </summary>
        [SerializeField] private TMP_Text info_txt;

        #endregion

        #region Unity Callbacks

        /// <summary>
        /// �� ���� �� infos �迭���� �������� �ϳ��� ������ �ؽ�Ʈ�� �����մϴ�.
        /// </summary>
        void Start()
        {
            info_txt.text = infos[Random.Range(0, infos.Length)];
        }

        #endregion
    }
}


