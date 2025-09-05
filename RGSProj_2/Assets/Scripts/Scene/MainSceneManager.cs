using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField]private TMP_Text[] menus;
    [SerializeField] private TMP_Text[] starts;
    [SerializeField]private int nowCursor = 0;
    private bool isGame;
    private void Start()
    {
        menus[nowCursor].color = Color.red;
        startPanel.SetActive(true);
        gamePanel.SetActive(false);
        isGame = false;
    }
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
    private void InitTextColor()
    {
        for(int i=0;i<menus.Length; i++)
        {
            menus[i].color = Color.white;
            starts[i].color = Color.white;
        }
    }
}
