using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using VisualNovelTryout.Enums;

namespace VisualNovelTryout.Controller
{
    public class DialogueSystemController : MonoBehaviour
    {

        //TextBox IU Element
        [SerializeField] TextMeshProUGUI textBox;
        [SerializeField] TextMeshProUGUI nameBox;
        [SerializeField] GameObject textPanel;
        

        //textSpeed OnEnable da GamaManager'den al?nacak
        [SerializeField] float typingSpeed;
        
        // Selected Character Name
        string characterName;

        // Line Text
        string Dialoguetext;

        // NameBox Typing speed. -- Lines Index
        float nameTypingSpeed = 0.02f;


        //Control

        public bool Typing { get; private set; }

        //Coroutine
        Coroutine typeDialogueCoroutine;
        Coroutine typeNameCoroutine;


        public void TypeDialogue(string text, Enums.Characters charactersEnum)
        {

            if (text == string.Empty)
            {
                textPanel.SetActive(false);
                return;
            }

            if (characterName != charactersEnum.ToString() )
            {
                nameBox.text = string.Empty;
                characterName = charactersEnum.ToString();
                typeNameCoroutine = StartCoroutine(TypeName());
            }

            if (Typing)
            {

                StopCoroutine(typeDialogueCoroutine);
                Typing = false;
                textBox.text = Dialoguetext;
               
            }
            else
            {
                Dialoguetext = text;
                textPanel.SetActive(true);
                textBox.text = string.Empty;
                typeDialogueCoroutine = StartCoroutine(TypeLine());
                        
            }



        }

        IEnumerator TypeName()
        {
            foreach (char c in characterName.ToCharArray())
            {
                nameBox.text += c;
                yield return new WaitForSeconds(nameTypingSpeed);
            }
        }

        IEnumerator TypeLine()
        {
            Typing = true;

            for (int i = 0; i < Dialoguetext.Length; i++)
            {
                if (Dialoguetext[i] == '<')
                    textBox.text += SkipRichText(ref i);
                else
                    textBox.text += Dialoguetext[i];

                yield return new WaitForSeconds(typingSpeed);
            }

            Typing = false;
            


        }
        private string SkipRichText(ref int i)
        {
            string _richText = "";

            while (true)
            {
                if (Dialoguetext[i] != '>')
                {
                    _richText += Dialoguetext[i];
                }
                else
                {
                    _richText += Dialoguetext[i];
                    return _richText;
                    break;
                }

                i++;
            }

        }
        
    }
}

