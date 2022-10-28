using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualNovelTryout.UI;


namespace VisualNovelTryout.Animation
{
    public class TextBoxAnimationController : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] GameObject textBox;

        #endregion

        #region Private Fields
        
        //Object Cache
        Animator _animator;
        CanvasGroup _textBoxCanvasGroup;
        TransitionHolder _transitionHolder;

        //Control

        bool _transitonIsActive;

        Coroutine _textBoxTrasitionRoutine;
        #endregion

        #region Base Functions

        private void Awake()
        {
            // cache
            _animator = textBox.GetComponent<Animator>();
            _textBoxCanvasGroup = textBox.GetComponent<CanvasGroup>();

            _transitionHolder = GetComponent<TransitionHolder>();

        }

        private void Start()
        {
            textBox.SetActive(false);
        }

        #endregion

        #region Public Functions

        public void TextBoxSetAnimation(bool status)
        {
            if (status)
            {
                _animator.SetTrigger("SetTrue");
            }
            else
            {
                _animator.SetTrigger("SetFalse");
            }
        }


        public void TextBoxSetTransiton(bool active)
        {
            if (_transitonIsActive)
            {
                _transitonIsActive = false;
                StopCoroutine(_textBoxTrasitionRoutine);
            }

            _textBoxTrasitionRoutine = StartCoroutine(TransitionTextBox(active));
        }

        #endregion

        #region Private Functions

        IEnumerator TransitionTextBox(bool active)
        {
            _transitonIsActive = true;

            if (active)
            {
                textBox.SetActive(true);
                _transitionHolder.FadeIn(_textBoxCanvasGroup, .3f);
            }
            else
            {
                _transitionHolder.FadeOut(_textBoxCanvasGroup, .3f);
                yield return new WaitForSeconds(.4f);
                textBox.SetActive(false);
            }

            _transitonIsActive = false;
        }


        #endregion

    }

}
