using TMPro;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace DialogueSystem
{
    public class DialogueLine : DialogueBaseClass
    {
        [SerializeField][TextArea]private string input;
        [SerializeField] private Color textColor;
        [SerializeField] private float delay;
        private TMP_Text textHolder;
        public UnityEvent OnDialogueStart; 
        private void Awake()
        {
            textHolder = GetComponent<TMP_Text>();
            textHolder.text = "";
        }
        private void Start()
        {
            OnDialogueStart?.Invoke();

            StartCoroutine(WriteText(input, textHolder, textColor, delay));
        }

    }

}
