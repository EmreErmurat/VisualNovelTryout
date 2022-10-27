using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelTryout.UI
{
    public class Transition : MonoBehaviour
    {
        Coroutine fadeInCoroutine;
        Coroutine fadeOutCoroutine;

        bool activeFadeOut;
        bool activeFadeIn;

        public void FadeOut(CanvasGroup canvasGroup, float duration)
        {

            fadeOutCoroutine = StartCoroutine(FadeOutRou(canvasGroup, duration));
        }

        public void FadeIn(CanvasGroup canvasGroup, float duration)
        {

            fadeInCoroutine = StartCoroutine(FadeInRou(canvasGroup, duration));
        }

        public void StopFade()
        {
            if (activeFadeOut)
            {
                StopCoroutine(fadeOutCoroutine);
                activeFadeOut = false;
            }
            if (activeFadeIn)
            {
                StopCoroutine(fadeInCoroutine);
                activeFadeIn = false;
            }
           
        }

        IEnumerator FadeOutRou(CanvasGroup canvasGroup, float duration)
        {
            activeFadeOut = true;

            float counter = 0f;
            float alpha = canvasGroup.alpha;

            while (counter < duration)
            {
                counter += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(alpha, .3f, counter / duration);

                yield return null;
            }

            activeFadeOut = false;
        }
        IEnumerator FadeInRou(CanvasGroup canvasGroup, float duration)
        {
            activeFadeIn = true;
            float counter = 0f;
            float alpha = canvasGroup.alpha;

            while (counter < duration)
            {
                counter += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(alpha, 1, counter / duration);

                yield return null;
            }

            activeFadeIn = false;
        }


    }

}
