using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VisualNovelTryout.Controller;
using VisualNovelTryout.Inputs;
using VisualNovelTryout.Enums;
using VisualNovelTryout.ScriptableObjects;
using VisualNovelTryout.UI;

namespace VisualNovelTryout.Manager
{
    public class SceneManager : MonoBehaviour
    {
        PcInputsReceiver pcInputsReceiver;
        StoryController storyController;
        TypingController typingController;
        UIManager uIManager;


        public SceneManagerState sceneManagerState { get; private set; }
        Coroutine normalStateCoroutine;

        bool normalStateBool;
        bool gameIsRunning = true;

        IEnumerator selectedElaborated;

        private void Awake()
        {
            GameManager.Instance.sceneManager = this;
            pcInputsReceiver = new PcInputsReceiver();
           
        }

        private void Start()
        {
            storyController = GameManager.Instance.storyController;
            typingController = GameManager.Instance.typingController;
            uIManager = GameManager.Instance.uIManager;


            
        }
        
        private void Update()
        {
            //if (pcInputsReceiver.MouseZeroInput)
            //{

            //    StartCoroutine( testt(index));
            //    print(index);
            //    index++;
            //}
           

            if (pcInputsReceiver.MouseZeroInput && gameIsRunning)
            {
                sceneManagerState = SceneManagerState.Working;

                gameIsRunning = false;

                FindSceneElaborated();


                StartCoroutine(selectedElaborated);

                
            }
        }


        void DelegateTheDuties()
        {
            
            StartCoroutine(InputCooldown());

            switch (storyController.activeSceneContent.sceneState)
            {
                case SceneState.Basic:

                    if (storyController.activeSceneContent.FadeTime != 0)
                    {
                        if (normalStateBool || typingController.typingState == TypingState.Typing || uIManager.uIManagerState != UIManagerState.Complated)
                        {
                            if (normalStateBool)
                            {
                                StopCoroutine(normalStateCoroutine);
                                normalStateBool = false;
                            }

                            uIManager.ChangeImage(storyController.activeSceneContent.sprite, storyController.activeSceneContent.FadeTime);
                            typingController.TypeDialogue(storyController.activeSceneContent.context, storyController.activeSceneContent.characters);
                            sceneManagerState = SceneManagerState.Complated;

                        }
                        else
                        {
                            normalStateCoroutine = StartCoroutine(NormalStateRoutine());
                        }

                    }
                    else
                    {
                        typingController.TypeDialogue(storyController.activeSceneContent.context, storyController.activeSceneContent.characters);


                        sceneManagerState = SceneManagerState.Complated;
                    }
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



        IEnumerator NormalStateRoutine()
        {
            normalStateBool = true;

            //typingController.TextBoxSwitch(false);
            StartCoroutine(typingController.TransitionTextBox(false));
            uIManager.ChangeImage(storyController.activeSceneContent.sprite, storyController.activeSceneContent.FadeTime); 

            yield return new WaitForSeconds(storyController.activeSceneContent.FadeTime);

            typingController.TypeDialogue(storyController.activeSceneContent.context, storyController.activeSceneContent.characters);

            sceneManagerState = SceneManagerState.Complated;

            normalStateBool = false;
        }


        IEnumerator InputCooldown(float value = .5f)
        {
            yield return new WaitForSeconds(value);
            gameIsRunning = true;
        }


        void FindSceneElaborated()
        {
            switch (storyController.eventList)
            {
                case EventList.Intro:
                    selectedElaborated = IntroElaborated();
                    break;
                case EventList.RavenGame:
                    selectedElaborated = RavenGameElaborated();
                    break;
                default:
                    break;
            }
            
        }


        #region Scene Events // Elaborated Codes

        IEnumerator IntroElaborated()
        {
            print("w");
            switch (storyController.activeSceneIndex)
            {


                case 5:
                    if (true) // private ??
                    {
                        storyController.activeSceneContent.context = "Here we go. Back to Home again...";
                    }
                    else
                    {
                        storyController.activeSceneContent.context = "Here we go. Back to Nicole's again...";
                    }

                    DelegateTheDuties();
                    // standart ?ekilde devam edecek...
                    storyController.eventList = EventList.RavenGame;
                    storyController.FindEventData();

                    break;



                default:
                    DelegateTheDuties();
                    break;
            }

            yield return null;
        }

        IEnumerator RavenGameElaborated()
        {
            switch (storyController.activeSceneIndex)
            {
                

                default:
                    DelegateTheDuties();
                    break;
            }


            yield return null;
        }

        #endregion





    }

}
