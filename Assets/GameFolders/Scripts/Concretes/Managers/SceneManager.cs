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
        [SerializeField] DialogObject eventOne;

        int index = 0;
        string name = "";
        private void Awake()
        {
            
        }

        private void OnEnable()
        {

            // DialogueSystemController.StartDialogue(eventOne.eventDialogs[0].context, eventOne.eventDialogs[0].characters);

            eventOne.eventDialogs[0].context = $"{name} <b>Hi.</b>";
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (DialogueSystemController.Typing)
                {
                    DialogueSystemController.TypingDialogue(eventOne.eventDialogs[index -1].context, eventOne.eventDialogs[index -1].characters);
                }
                else
                {
                    index++;
                    DialogueSystemController.TypingDialogue(eventOne.eventDialogs[index -1].context, eventOne.eventDialogs[index -1].characters);
                }

             
            }
        }

        


    }

}
