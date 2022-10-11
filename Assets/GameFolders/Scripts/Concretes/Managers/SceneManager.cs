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
                if (DialogueSystemController.Typing)
                {
                    DialogueSystemController.TypeDialogue(eventOne.eventDialogs[index -1].context, eventOne.eventDialogs[index -1].characters);
                }
                else
                {
                    index++;
                    DialogueSystemController.TypeDialogue(eventOne.eventDialogs[index -1].context, eventOne.eventDialogs[index -1].characters);
                }

             
            }
        }


    }

}
