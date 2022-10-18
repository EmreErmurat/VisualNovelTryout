using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VisualNovelTryout.UI;
using VisualNovelTryout.Controller;
using DG.Tweening;

namespace VisualNovelTryout.Managers
{
    public class SceneUIManager : MonoBehaviour
    {
        [SerializeField] DialogueSystemController DialogueSystemController;

        Transition transition;

        [SerializeField] Image background;
        

        [SerializeField] CanvasGroup backgroundGroup;

        public bool changeImageRunning { get; private set; }
       
        Coroutine coroutine;

        private void Awake()
        {
            transition = GetComponent<Transition>();
        }



        //public void ChangeImage(Sprite spriteImage, float fadeTime)
        //{

        //    coroutine = StartCoroutine(ChangeImageRoutine(spriteImage, fadeTime));


            

        //}



        IEnumerator ChangeImageRoutine(Sprite spriteImage, float fadeTime, string text, Enums.Characters charactersEnum)
        {
            changeImageRunning = true;

            if (fadeTime != 0)
            {
                DialogueSystemController.VisableTextBox();

                transition.FadeOut(backgroundGroup, fadeTime);

                yield return new WaitForSeconds(fadeTime + 0.05f);

                background.sprite = spriteImage;

                DialogueSystemController.TypeDialogue(text, charactersEnum);

                transition.FadeIn(backgroundGroup, fadeTime);

                yield return new WaitForSeconds(fadeTime);
            }
            else
            {
                background.sprite = spriteImage;
            }

            changeImageRunning = false;
        }


        public void Test(Sprite spriteImage, float fadeTime, string text, Enums.Characters charactersEnum)
        {
            if (changeImageRunning || DialogueSystemController.Typing)
            {
                transition.StopFade(backgroundGroup);
                background.sprite = spriteImage;
                //for stop
                DialogueSystemController.TypeDialogue(text, charactersEnum);
                StopCoroutine(coroutine);
                changeImageRunning = false;
            }
            else if (false) // burdan devam
            {

            }
            else
            {
                coroutine = StartCoroutine(ChangeImageRoutine(spriteImage, fadeTime, text, charactersEnum));
            }



        }
     
       
    }

}
