using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using VisualNovelTryout.Enums;
using VisualNovelTryout.Animation;
using VisualNovelTryout.UI;

namespace VisualNovelTryout.Controller
{
    public class TypingController : MonoBehaviour
    {
        //TextBox IU Element
        [SerializeField] GameObject textPanel;
        [SerializeField] TextMeshProUGUI characterNameText;
        [SerializeField] TextMeshProUGUI dialogueTextArea;

        TextBoxAnimationController textBoxAnimationController;
        Transition transition;
        CanvasGroup canvasGroup;

        //Control
        public bool Typing { get; private set; }
        
        public TypingState typingState { get; private set; }

        //textSpeed to GameManager
        float typingSpeed;


        // Selected Character Name
        string characterName;


        // Line Text
        string selectedDialogueText;


        // NameBox Typing speed. -- Lines Index
        float nameTypingSpeed = 0.02f;


        //Coroutine
        Coroutine typeDialogueCoroutine;
        Coroutine typeNameCoroutine;

        private void Awake()
        {
            GameManager.Instance.typingController = this;

            typingState = new TypingState(); // gerek var m? emin de?ilim.

            textBoxAnimationController = textPanel.GetComponent<TextBoxAnimationController>();
            transition = GetComponent<Transition>();
            canvasGroup = textPanel.gameObject.GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            typingSpeed = GameManager.Instance.typingSpeed;
        }

        public void TypeDialogue(string importedText, Enums.Characters charactersEnum)
        {
            if (importedText == string.Empty)
            {
                StartCoroutine(TransitionTextBox(false));
                //TextBoxSwitch(false);
                return;
            }


            if (typingState == TypingState.Typing)
            {
                typingSpeed = 0.01f;
            }
            else
            {
                if (characterName != charactersEnum.ToString())
                {
                    characterName = charactersEnum.ToString();
                    characterNameText.text = string.Empty;
                    typeNameCoroutine = StartCoroutine(TypeName());
                }

                selectedDialogueText = importedText;
                //TextBoxSwitch(true);
                StartCoroutine(TransitionTextBox(true));
                dialogueTextArea.text = string.Empty;
                typeDialogueCoroutine = StartCoroutine(TypeLine());
            }


        }

        IEnumerator TypeName()
        {
            yield return new WaitForSeconds(.2f);

            foreach (char c in characterName.ToCharArray())
            {
                characterNameText.text += c;
                yield return new WaitForSeconds(nameTypingSpeed);
            }
        }

        IEnumerator TypeLine()
        {
            typingState = TypingState.Typing;

            yield return new WaitForSeconds(.2f);

            for (int i = 0; i < selectedDialogueText.Length; i++)
            {

                if (selectedDialogueText[i] == '<')
                {
                    selectedDialogueText += SkipRichText(ref i);
                }
                else
                {
                    dialogueTextArea.text += selectedDialogueText[i];
                }

                yield return new WaitForSeconds(typingSpeed);

            }

            typingState = TypingState.Complated;

            if (typingSpeed != GameManager.Instance.typingSpeed)
            {
                typingSpeed = GameManager.Instance.typingSpeed;
            }

        }


        string SkipRichText(ref int i)
        {
            string _richText = string.Empty;

            while (true)
            {
                _richText += selectedDialogueText[i];

                if (selectedDialogueText[i] == '>')
                {
                    return _richText;
                    //break; // while döngüsü içinde return varsa break e gerek yok mu bilmiyorum.
                }               

                i++;

            }
        }



        //public void TextBoxSwitch(bool value)
        //{
        //    if (value)
        //    {
        //        if (textPanel.activeSelf) return;
        //        textPanel.SetActive(true);
        //        textBoxAnimationController.TextBoxSetAnimation(true);
        //    }
        //    else
        //    {
        //        textBoxAnimationController.TextBoxSetAnimation(false);
        //    }
        //}

        
        public IEnumerator TransitionTextBox(bool active) // bir de?i?kene atanmal? ve rutinlerin üstüste binmemesi sa?lanmal?.
        {
            if (active)
            {
                textPanel.SetActive(true);
                transition.FadeIn(canvasGroup, .3f);
            }
            else
            {
                transition.FadeOut(canvasGroup, .3f);
                yield return new WaitForSeconds(.4f);
                textPanel.SetActive(false);
            }
            
        }


    }
}

