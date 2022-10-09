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
        [SerializeField] GameObject TextPanel;
        

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


        public void TypingDialogue(string text, Enums.Characters charactersEnum)
        {

            if (text == string.Empty)
            {
                TextPanel.SetActive(false);
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
                TextPanel.SetActive(true);
                ClearDialogue();
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

            
            //type each character 1 by 1
            foreach (char c in Dialoguetext.ToCharArray())
            {
                textBox.text += c;
                yield return new WaitForSeconds(typingSpeed);
            }

            Typing = false;
            

        }
 

        void ClearDialogue()
        {
            
            
            //characterName = string.Empty;
            textBox.text = string.Empty;
            //nameBox.text = string.Empty;
        }




    }
}

