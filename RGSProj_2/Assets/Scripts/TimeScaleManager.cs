using Unity.VisualScripting;
using UnityEngine;

public class TimeScaleManager : MonoBehaviour
{
    public static TimeScaleManager Instance { get; private set; }
    public int dontMoveStack = 0;
    private void Awake()
    {
        // ½Ì±ÛÅæ À¯Áö
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        dontMoveStack = 0;
    }
    private void Update()
    {
        if (dontMoveStack == 0)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }
    }
    public void TimeStopStackPlus()
    {
        dontMoveStack++;
    }
    public void TimeStopStackMinus()
    {
        dontMoveStack--;
    }
}
