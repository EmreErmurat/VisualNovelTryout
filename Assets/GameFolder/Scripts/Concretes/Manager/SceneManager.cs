using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualNovelTryout.Controller;
using VisualNovelTryout.Inputs;
using VisualNovelTryout.Enums;
using VisualNovelTryout.ScriptableObjects;

namespace VisualNovelTryout.Manager
{
    public class SceneManager : MonoBehaviour
    {
        PcInputsReceiver pcInputsReceiver;
        StoryController storyController;

     



        private void Awake()
        {
            GameManager.Instance.sceneManager = this;
            pcInputsReceiver = new PcInputsReceiver();
            
        }

        private void Start()
        {
            storyController = GameManager.Instance.storyController;
        }

        private void Update()
        {
            if (pcInputsReceiver.MouseZeroInput)
            {
                
                
                switch (storyController.activeSceneContent.sceneState)
                {
                    case SceneState.Normal:
                        break;
                    case SceneState.UnSkippable:
                        break;
                    case SceneState.AutoSkip:
                        break;
                    case SceneState.ChoiceMenu:
                        break;
                    case SceneState.Pan:
                        break;
                    case SceneState.Animation:
                        break;
                    case SceneState.End:
                        break;
                    default:
                        break;
                }

                storyController.SceneIndexController();
            }
        }






    }

}
