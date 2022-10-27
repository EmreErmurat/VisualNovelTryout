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
        //Object cache
        TypingController typingController;
        Transition transition;

        //UI
        [SerializeField] CanvasGroup backgroundCanvasGroup;
        [SerializeField] Image background;

        public UIManagerState uIManagerState { get; private set; }


        Coroutine changeImageRoutine;

        float newFadeTime;
        bool skipFadeOut;

        private void Awake()
        {
            GameManager.Instance.uIManager = this;

            transition = GetComponent<Transition>();
        }

        private void Start()
        {
            typingController = GameManager.Instance.typingController;
        }


        public void ChangeImage(Sprite importedSprite, float fadeTime)
        {
            newFadeTime = fadeTime;
            switch (uIManagerState)
            {

                case UIManagerState.FadeOut:

                    transition.StopFade();
                    newFadeTime = 0.2f;
                    changeImageRoutine = StartCoroutine(ChangeImageCoroutine(importedSprite));

                    break;

                case UIManagerState.FadeIn:

                    transition.StopFade();
                    newFadeTime = 0.2f;
                    skipFadeOut = true;
                    changeImageRoutine = StartCoroutine(ChangeImageCoroutine(importedSprite));


                    break;

                default:
                    if (importedSprite == background.sprite) return;
                    changeImageRoutine = StartCoroutine(ChangeImageCoroutine(importedSprite));
                    break;
            }
        }

        IEnumerator ChangeImageCoroutine(Sprite importedSprite)
        {

            if (!skipFadeOut)
            {
                uIManagerState = UIManagerState.FadeOut;

                transition.FadeOut(backgroundCanvasGroup, (newFadeTime));
                yield return new WaitForSeconds(newFadeTime);

                background.sprite = importedSprite;
            }

            uIManagerState = UIManagerState.FadeIn;

            transition.FadeIn(backgroundCanvasGroup, (newFadeTime));
            yield return new WaitForSeconds(newFadeTime);

            uIManagerState = UIManagerState.Complated;
            skipFadeOut = false;
           
            

        }


        //IEnumerator ChangeImageCoroutine(Sprite importedSprite, float fadeTime)
        //{


        //    if (fadeTime != 0)
        //    {
        //        uIManagerState = UIManagerState.Fade;

        //        transition.FadeOut(backgroundCanvasGroup, (fadeTime));
        //        yield return new WaitForSeconds(fadeTime);

        //        background.sprite = importedSprite;

        //        transition.FadeIn(backgroundCanvasGroup, (fadeTime));
        //        yield return new WaitForSeconds(fadeTime);

        //        uIManagerState = UIManagerState.Complated;
        //    }
        //    //else
        //    //{
        //    //    background.sprite = importedSprite;
        //    //    uIManagerState = UIManagerState.Complated;
        //    //}


        //}



    }

}
