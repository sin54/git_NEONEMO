using NUnit.Framework;
using TMPro;
using UnityEngine;

public class LoadingSceneManager : MonoBehaviour
{
    [SerializeField] private string[] infos;
    [SerializeField] private TMP_Text info_txt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        info_txt.text = infos[Random.Range(0, infos.Length)];
    }
}
