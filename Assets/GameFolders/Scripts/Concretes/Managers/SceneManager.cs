using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualNovelTryout.Controller;
using VisualNovelTryout.ScriptableObjects;

namespace VisualNovelTryout.Managers
{
    public class SceneManager : MonoBehaviour
    {
        [SerializeField] DialogueSystemController DialogueSystemController;
        [SerializeField] SceneUIManager sceneUIManager;
        [SerializeField] DialogObject eventOne;

        int index = 0;
        
        private void Awake()
        {
            
        }

        private void OnEnable()
        {
            
            
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //if (DialogueSystemController.Typing)
                //{
                //    DialogueSystemController.TypeDialogue(eventOne.eventDialogs[index].context, eventOne.eventDialogs[index].characters);
                //}
                //else
                //{

                //    DialogueSystemController.TypeDialogue(eventOne.eventDialogs[index].context, eventOne.eventDialogs[index].characters);
                //    sceneUIManager.ChangeImage(eventOne.eventDialogs[index].sprite, eventOne.eventDialogs[index].FadeTime);
                //    index++;
                //}
                if (DialogueSystemController.Typing || sceneUIManager.changeImageRunning)
                {
                    index--;
                    sceneUIManager.Test(eventOne.eventDialogs[index].sprite, eventOne.eventDialogs[index].FadeTime, eventOne.eventDialogs[index].context, eventOne.eventDialogs[index].characters);
                    index++;
                }
                else
                {
                    sceneUIManager.Test(eventOne.eventDialogs[index].sprite, eventOne.eventDialogs[index].FadeTime, eventOne.eventDialogs[index].context, eventOne.eventDialogs[index].characters);
                    index++;
                }

                
            }
        }


    }

}
