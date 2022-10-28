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
        #region Serialized Fields

        //TextBox IU Element
        [SerializeField] GameObject textPanel;
        [SerializeField] TextMeshProUGUI characterNameText;
        [SerializeField] TextMeshProUGUI dialogueTextArea;

        #endregion

        #region Private Fields
        
        // Object Cache
        TextBoxAnimationController _textBoxAnimationController;
        TransitionHolder _transitionHolder;
        CanvasGroup _textBoxCanvasGroup;

        // Typing System Needs
        float _typingSpeed;
        string _characterName;
        string _selectedDialogueText;
        float _nameTypingSpeed = 0.02f;

        //Coroutine
        Coroutine _typeDialogueCoroutine;
        Coroutine _typeNameCoroutine;

        #endregion

        #region Public Properties

        public TypingState TypingState { get; private set; }

        #endregion

        #region Base Functions
        private void Awake()
        {
            GameManager.Instance.SetSelfCache(this.gameObject);

            _textBoxAnimationController = textPanel.GetComponentInParent<TextBoxAnimationController>();
            _transitionHolder = GetComponent<TransitionHolder>();
            _textBoxCanvasGroup = textPanel.gameObject.GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            _typingSpeed = GameManager.Instance.TypingSpeed;
        }

        #endregion

        #region Public Functions

        public void TypeDialogue(string importedText, Enums.Characters charactersEnum)
        {
            if (importedText == string.Empty)
            {
                _textBoxAnimationController.TextBoxSetTransiton(false);  
                return;
            }


            if (TypingState == TypingState.Typing)
            {
                _typingSpeed = 0.01f;
            }
            else
            {
                if (_characterName != charactersEnum.ToString())
                {
                    _characterName = charactersEnum.ToString();
                    characterNameText.text = string.Empty;
                    _typeNameCoroutine = StartCoroutine(TypeName());
                }

                _selectedDialogueText = importedText;
                _textBoxAnimationController.TextBoxSetTransiton(true);
                dialogueTextArea.text = string.Empty;
                _typeDialogueCoroutine = StartCoroutine(TypeLine());
            }


        }

        #endregion

        #region Private Functions

        IEnumerator TypeName()
        {
            yield return new WaitForSeconds(.2f);

            foreach (char c in _characterName.ToCharArray())
            {
                characterNameText.text += c;
                yield return new WaitForSeconds(_nameTypingSpeed);
            }
        }

        IEnumerator TypeLine()
        {
            TypingState = TypingState.Typing;

            yield return new WaitForSeconds(.2f);

            for (int i = 0; i < _selectedDialogueText.Length; i++)
            {

                if (_selectedDialogueText[i] == '<')
                {
                    dialogueTextArea.text += SkipRichText(ref i);
                }
                else
                {
                    dialogueTextArea.text += _selectedDialogueText[i];
                }

                yield return new WaitForSeconds(_typingSpeed);

            }

            TypingState = TypingState.Complated;

            if (_typingSpeed != GameManager.Instance.TypingSpeed)
            {
                _typingSpeed = GameManager.Instance.TypingSpeed;
            }

        }


        string SkipRichText(ref int i)
        {
            string _richText = string.Empty;

            while (i < _selectedDialogueText.Length)
            {

                _richText += _selectedDialogueText[i];

                if (_selectedDialogueText[i] == '>')
                {

                    return _richText;
                    
                }

                i++;
               

            }

            return _richText;
        }

        #endregion


    }

}

