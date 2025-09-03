using UnityEngine;
using System.Collections;
using TMPro;
namespace DialogueSystem
{
    public class DialogueBaseClass : MonoBehaviour
    {
        public bool finished { get; private set; }
        protected IEnumerator WriteText(string input, TMP_Text textHolder,Color textColor,float delay)
        {
            textHolder.color= textColor;

            for(int i = 0; i < input.Length; i++)
            {
                textHolder.text += input[i];
                yield return new WaitForSeconds(delay);
            }

            yield return new WaitUntil(()=>Input.GetKeyDown(KeyCode.Return));
            finished = true;
        }
    }

}
