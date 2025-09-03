using TMPro;
using UnityEngine;
using System.Collections;

public class TypeManager : MonoBehaviour
{
    public TextMeshProUGUI textUI;     // 출력할 TextMeshPro 텍스트
    public string fullText; // 전체 문장
    public float delay = 0.15f;        // 글자 간 딜레이 시간

    public void ShowText()
    {
        StartCoroutine(ShowTextLetterByLetter());
    }
    public void DeleteText()
    {
        textUI.text = "";
    }
    IEnumerator ShowTextLetterByLetter()
    {
        textUI.text = "";
        foreach (char letter in fullText)
        {
            textUI.text += letter;
            yield return new WaitForSecondsRealtime(delay);

        }
    }
}