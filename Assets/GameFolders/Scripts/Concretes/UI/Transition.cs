using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace VisualNovelTryout.UI
{
    public class Transition : MonoBehaviour
    {
        Coroutine fadeInCoroutine;
        Coroutine fadeOutCoroutine;

        public void FadeOut(CanvasGroup canvasGroup, float duration)
        {

            fadeOutCoroutine = StartCoroutine(FadeOutRou(canvasGroup, duration));
        }

        public void FadeIn(CanvasGroup canvasGroup, float duration)
        {

            fadeInCoroutine = StartCoroutine(FadeInRou(canvasGroup, duration));
        }

        public void StopFade(CanvasGroup canvasGroup)
        {
            StopCoroutine(fadeInCoroutine);
            StopCoroutine(fadeOutCoroutine);
            canvasGroup.alpha = 1;
        }

        IEnumerator FadeOutRou(CanvasGroup canvasGroup, float duration)
        {
            float counter = 0f;

            while (counter < duration)
            {
                counter += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(1, .3f, counter / duration);

                yield return null;
            }

        }
        IEnumerator FadeInRou(CanvasGroup canvasGroup, float duration)
        {
            float counter = 0f;

            while (counter < duration)
            {
                counter += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(.3f, 1, counter / duration);

                yield return null;
            }

        }


    }


    /*
     * 
        Tween fadeTween;

        private void Fade(CanvasGroup canvasGroup, float endValue, float duration, TweenCallback onEnd)
        {
            if (fadeTween != null)
            {
                fadeTween.Kill(false);

            }

            fadeTween = canvasGroup.DOFade(endValue, duration);
            fadeTween.onComplete += onEnd;
        }

        public void FadeIn(CanvasGroup canvasGroup, float duration)
        {
            Fade(canvasGroup, 1f, duration, () =>
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            });
        }

        public void FadeOut(CanvasGroup canvasGroup, float duration)
        {
            Fade(canvasGroup, 0f, duration, () =>
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            });
        }
     * */

}
