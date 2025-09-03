using System.Collections;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueHolder : MonoBehaviour
    {
        private void Awake()
        {
            DeActivate();
        }
        public void StartDialogue()
        {
            StartCoroutine(DialogueSequence());
        }
        private IEnumerator DialogueSequence()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                DeActivate();

                var child = transform.GetChild(i).gameObject;
                var dialogueLine = child.GetComponent<DialogueLine>();


                child.SetActive(true);
                yield return new WaitUntil(() => dialogueLine.finished);
            }

            gameObject.SetActive(false);
        }
        private void DeActivate()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}

