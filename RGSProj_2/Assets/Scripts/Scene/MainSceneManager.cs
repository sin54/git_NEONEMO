using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Core;

namespace Scene
{
    /// <summary>
    /// 메인 메뉴 씬에서 메뉴 네비게이션 및 씬 전환을 관리합니다.
    /// 화살표 키로 메뉴 이동, Enter 키로 선택된 메뉴를 실행합니다.
    /// </summary>
    [DisallowMultipleComponent]
    public class MainSceneManager : MonoBehaviour
    {
        #region Serialized Fields

        /// <summary>초기 메뉴 UI가 포함된 패널</summary>
        [Tooltip("게임 시작 전 초기 메뉴가 표시될 패널을 할당하세요.")]
        [SerializeField] private GameObject startPanel;

        /// <summary>게임 시작 후 서브 메뉴 UI가 포함된 패널</summary>
        [Tooltip("게임 시작 후 표시될 서브 메뉴 패널을 할당하세요.")]
        [SerializeField] private GameObject gamePanel;

        /// <summary>초기 메뉴 항목 텍스트들</summary>
        [Tooltip("시작 패널에서 선택할 메뉴 텍스트 배열을 설정하세요.")]
        [SerializeField] private TMP_Text[] menus;

        /// <summary>서브 메뉴 항목 텍스트들</summary>
        [Tooltip("게임 진행 패널에서 선택할 메뉴 텍스트 배열을 설정하세요.")]
        [SerializeField] private TMP_Text[] starts;

        #endregion

        #region Fields

        /// <summary>현재 선택된 커서 인덱스</summary>
        [SerializeField]private int nowCursor = 0;

        /// <summary>게임(서브 메뉴) 모드 활성화 여부</summary>
        private bool isGame;

        #endregion

        #region Unity Callbacks

        /// <summary>
        /// 시작 시 초기 커서 위치, 메뉴 색상, 패널 활성 상태를 설정합니다.
        /// </summary>
        private void Start()
        {
            menus[nowCursor].color = Color.red;
            startPanel.SetActive(true);
            gamePanel.SetActive(false);
            isGame = false;
        }

        /// <summary>
        /// 매 프레임 입력을 감지하여 커서 이동 및 메뉴 선택 처리를 수행합니다.
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