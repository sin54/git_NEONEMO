using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Core;

namespace Scenes
{
    /// <summary>
    /// ���� �޴� ������ �޴� �׺���̼� �� �� ��ȯ�� �����մϴ�.
    /// ȭ��ǥ Ű�� �޴� �̵�, Enter Ű�� ���õ� �޴��� �����մϴ�.
    /// </summary>
    [DisallowMultipleComponent]
    public class MainSceneManager : MonoBehaviour
    {
        #region Serialized Fields

        /// <summary>�ʱ� �޴� UI�� ���Ե� �г�</summary>
        [Tooltip("���� ���� �� �ʱ� �޴��� ǥ�õ� �г��� �Ҵ��ϼ���.")]
        [SerializeField] private GameObject startPanel;

        /// <summary>���� ���� �� ���� �޴� UI�� ���Ե� �г�</summary>
        [Tooltip("���� ���� �� ǥ�õ� ���� �޴� �г��� �Ҵ��ϼ���.")]
        [SerializeField] private GameObject gamePanel;

        /// <summary>�ʱ� �޴� �׸� �ؽ�Ʈ��</summary>
        [Tooltip("���� �гο��� ������ �޴� �ؽ�Ʈ �迭�� �����ϼ���.")]
        [SerializeField] private TMP_Text[] menus;

        /// <summary>���� �޴� �׸� �ؽ�Ʈ��</summary>
        [Tooltip("���� ���� �гο��� ������ �޴� �ؽ�Ʈ �迭�� �����ϼ���.")]
        [SerializeField] private TMP_Text[] starts;

        #endregion

        #region Fields

        /// <summary>���� ���õ� Ŀ�� �ε���</summary>
        [SerializeField]private int nowCursor = 0;

        /// <summary>����(���� �޴�) ��� Ȱ��ȭ ����</summary>
        private bool isGame;

        #endregion

        #region Unity Callbacks

        /// <summary>
        /// ���� �� �ʱ� Ŀ�� ��ġ, �޴� ����, �г� Ȱ�� ���¸� �����մϴ�.
        /// </summary>
        private void Start()
        {
            menus[nowCursor].color = Color.red;
            startPanel.SetActive(true);
            gamePanel.SetActive(false);
            isGame = false;
        }

        /// <summary>
        /// �� ������ �Է��� �����Ͽ� Ŀ�� �̵� �� �޴� ���� ó���� �����մϴ�.
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                nowCursor++;
                if (nowCursor >= menus.Length) nowCursor = 0;
                InitTextColor();
                if (!isGame)
                {
                    menus[nowCursor].color = Color.red;
                }
                else
                {
                    starts[nowCursor].color = Color.red;
                }
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                nowCursor--;
                if (nowCursor < 0) nowCursor = menus.Length - 1;
                InitTextColor();
                if (!isGame)
                {
                    menus[nowCursor].color = Color.red;
                }
                else
                {
                    starts[nowCursor].color = Color.red;
                }

            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (nowCursor == 0)
                {
                    if (!isGame)
                    {
                        startPanel.SetActive(false);
                        gamePanel.SetActive(true);
                        nowCursor = 0;
                        isGame = true;
                        starts[nowCursor].color = Color.red;
                    }
                    else
                    {
                        Loader.Load(Loader.Scene.CutScene);
                    }

                }
                else if (nowCursor == 1)
                {

                }
                else if (nowCursor == 2)
                {
                    if (!isGame)
                    {
                        Application.Quit();
                    }
                    else
                    {
                        startPanel.SetActive(true);
                        gamePanel.SetActive(false);
                        nowCursor = 0;
                        isGame = false;
                        InitTextColor() ;
                        menus[nowCursor].color = Color.red;
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        private void InitTextColor()
        {
            for(int i=0;i<menus.Length; i++)
            {
                menus[i].color = Color.white;
                starts[i].color = Color.white;
            }
        }

        #endregion
    }
}