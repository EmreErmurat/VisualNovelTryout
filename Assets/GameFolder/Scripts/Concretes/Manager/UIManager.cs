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


        Coroutine _changeImageRoutine;

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

            switch (UIManagerState)
            {

                case UIManagerState.FadeOut:

                    _transitionHolder.StopFade();
                    _newFadeTime = 0.2f;
                    _changeImageRoutine = StartCoroutine(ChangeImageCoroutine(importedSprite));

                    break;

                case UIManagerState.FadeIn:

                    _transitionHolder.StopFade();
                    _newFadeTime = 0.2f;
                    _skipFadeOut = true;
                    _changeImageRoutine = StartCoroutine(ChangeImageCoroutine(importedSprite));


                    break;

                default:
                    if (importedSprite == _background.sprite) return;
                    _changeImageRoutine = StartCoroutine(ChangeImageCoroutine(importedSprite));
                   
                    break;
            }

        }


        public void HardStopChangeImage()
        {
            StopCoroutine(_changeImageRoutine);
            LastSprite = null;
            _skipFadeOut = false;
            UIManagerState = UIManagerState.Complated;
        }
        #endregion

        #region Private Functions

        IEnumerator ChangeImageCoroutine(Sprite importedSprite)
        {

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

        #endregion

    }

}
