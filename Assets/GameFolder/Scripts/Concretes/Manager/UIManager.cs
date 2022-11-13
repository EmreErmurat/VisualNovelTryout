using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VisualNovelTryout.Controller;
using VisualNovelTryout.UI;
using VisualNovelTryout.Enums;

namespace VisualNovelTryout.Manager
{

    public class UIManager : MonoBehaviour
    {

        #region Serialized Fields

        //UI
        [SerializeField] CanvasGroup _backgroundCanvasGroup;
        [SerializeField] Image _background;

        #endregion

        #region Private Fields

        //Object cache
        TransitionHolder _transitionHolder;

        //for ImagePrint
        float _newFadeTime;
        bool _skipFadeOut;


        Coroutine _changeImageCoroutine;
        Coroutine _closeBackgroundCoroutine;
        #endregion

        #region Public Properties

        public UIManagerState UIManagerState { get; private set; }
        public Sprite LastSprite { get; private set; }
        #endregion

        #region Base Functions

        private void Awake()
        {
            GameManager.Instance.SetSelfCache(this.gameObject);

            _transitionHolder = GetComponent<TransitionHolder>();
        }

        #endregion

        #region Public Functions

        public void ChangeImage(Sprite importedSprite, float fadeTime)
        {
            _newFadeTime = fadeTime;
            LastSprite = importedSprite;

            if (!_backgroundCanvasGroup.gameObject.activeSelf)
            {
                OpenBackground(); // 
                
            }

            switch (UIManagerState)
            {

                case UIManagerState.FadeOut:

                    _transitionHolder.StopFade();
                    _newFadeTime = 0.2f;
                    _changeImageCoroutine = StartCoroutine(ChangeImageRoutine(importedSprite));

                    break;

                case UIManagerState.FadeIn:

                    _transitionHolder.StopFade();
                    _newFadeTime = 0.2f;
                    _skipFadeOut = true;
                    _changeImageCoroutine = StartCoroutine(ChangeImageRoutine(importedSprite));


                    break;

                default:
                    if (importedSprite == _background.sprite) return;
                    _changeImageCoroutine = StartCoroutine(ChangeImageRoutine(importedSprite));
                   
                    break;
            }

        }


        public void HardStopChangeImage()
        {
            StopCoroutine(_changeImageCoroutine);
            _transitionHolder.StopFade();
            LastSprite = null;
            _skipFadeOut = false;
            _backgroundCanvasGroup.alpha = 1;
            UIManagerState = UIManagerState.Complated;

        }

        public void CloseBackgroundImage(float duration)
        {
            _closeBackgroundCoroutine = StartCoroutine(CloseBackgroundImageRoutine(duration));
        }
        #endregion

        #region Private Functions

        private void OpenBackground()
        {
            _backgroundCanvasGroup.alpha = 0;
            _backgroundCanvasGroup.gameObject.SetActive(true);
        }

        IEnumerator ChangeImageRoutine(Sprite importedSprite)
        {

            if (!_backgroundCanvasGroup.gameObject.activeSelf)  // Deneme
            {
                _backgroundCanvasGroup.gameObject.SetActive(true);
            }

            if (!_skipFadeOut)
            {
                UIManagerState = UIManagerState.FadeOut;

                _transitionHolder.FadeOut(_backgroundCanvasGroup, (_newFadeTime));
                yield return new WaitForSeconds(_newFadeTime);

                _background.sprite = importedSprite;
            }

            UIManagerState = UIManagerState.FadeIn;

            _transitionHolder.FadeIn(_backgroundCanvasGroup, (_newFadeTime));
            yield return new WaitForSeconds(_newFadeTime);

            UIManagerState = UIManagerState.Complated;
            _skipFadeOut = false;

        }

        IEnumerator CloseBackgroundImageRoutine(float duration)
        {
            _transitionHolder.FadeOut(_backgroundCanvasGroup, duration);
            yield return new WaitForSeconds(duration + .05f);
            _background.sprite = null;
            _backgroundCanvasGroup.gameObject.SetActive(false);

        }

        #endregion

    }

}
