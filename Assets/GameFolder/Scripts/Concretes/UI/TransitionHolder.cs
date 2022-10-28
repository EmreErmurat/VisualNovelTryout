using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelTryout.UI
{
    public class TransitionHolder : MonoBehaviour
    {
        #region Private Fields

        bool _activeFadeOut;
        bool _activeFadeIn;

        Coroutine _fadeInCoroutine;
        Coroutine _fadeOutCoroutine;

        #endregion

        #region Public Functions

        public void FadeOut(CanvasGroup canvasGroup, float duration)
        {
            if (_activeFadeOut)
            {
                StopCoroutine(_fadeOutCoroutine);
                _activeFadeOut = false;
            }

            _fadeOutCoroutine = StartCoroutine(FadeOutRou(canvasGroup, duration));
        }


        public void FadeIn(CanvasGroup canvasGroup, float duration)
        {
            if (_activeFadeIn)
            {
                StopCoroutine(_fadeInCoroutine);
                _activeFadeIn = false;
            }

            _fadeInCoroutine = StartCoroutine(FadeInRou(canvasGroup, duration));
        }

        public void StopFade()
        {
            if (_activeFadeOut)
            {
                StopCoroutine(_fadeOutCoroutine);
                _activeFadeOut = false;
            }
            if (_activeFadeIn)
            {
                StopCoroutine(_fadeInCoroutine);
                _activeFadeIn = false;
            }

        }

        #endregion

        #region Private Functions


        IEnumerator FadeOutRou(CanvasGroup canvasGroup, float duration)
        {
            _activeFadeOut = true;

            float _counter = 0f;
            float _imageAlpha = canvasGroup.alpha;

            while (_counter < duration)
            {
                _counter += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(_imageAlpha, 0f, _counter / duration);

                yield return null;
            }

            _activeFadeOut = false;
        }

        IEnumerator FadeInRou(CanvasGroup canvasGroup, float duration)
        {
            _activeFadeIn = true;
            float _counter = 0f;
            float _imageAlpha = canvasGroup.alpha;

            while (_counter < duration)
            {
                _counter += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(_imageAlpha, 1, _counter / duration);

                yield return null;
            }

            _activeFadeIn = false;
        }



        #endregion

    }

}
