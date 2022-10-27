using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualNovelTryout.ScriptableObjects;
using VisualNovelTryout.Enums;
using VisualNovelTryout.Manager;

namespace VisualNovelTryout.Controller
{
    public class StoryController : MonoBehaviour
    {
        public EventList eventList { get; set; }

        //Object Cache
        TypingController typingController;
        SceneManager sceneManager;
        UIManager uiManager;

        // Event cache
        [SerializeField] SceneObject intro;
        [SerializeField] SceneObject ravenGame;

        SceneObject selectedSceneObject; // scene obje için seçilen obje ayr?m? yapmam gerek.

 
        public ContentOfTheEventObject activeSceneContent { get; private set; }
        
        public int activeSceneIndex { get; private set; }

        private StoryIndexState storyIndexState;

        Coroutine IndexListenerRoutine;

        private void Awake()
        {
            //cache self
            GameManager.Instance.storyController = this;
            storyIndexState = new StoryIndexState();

            activeSceneIndex = 0;
            activeSceneContent = intro.SceneContent[0];

            
        }

        private void Start()
        {
            typingController = GameManager.Instance.typingController;
            sceneManager = GameManager.Instance.sceneManager;
            uiManager = GameManager.Instance.uIManager;

            FindEventData();
        }

        


        //find index shoud be while

        public void SceneIndexController()
        {
            SceneState sceneState = activeSceneContent.sceneState;
            
            if (storyIndexState == StoryIndexState.Working)
            {
                StopCoroutine(IndexListenerRoutine);
            }
            
            IndexListenerRoutine = StartCoroutine(SceneIndexControllerRou(sceneState));
            
        }

        IEnumerator SceneIndexControllerRou(SceneState sceneState)
        {
            storyIndexState = StoryIndexState.Working;

            while (storyIndexState != StoryIndexState.Complated)
            {
                
               
                switch (sceneState)
                {
                    case SceneState.Basic:
                        if (sceneManager.sceneManagerState != SceneManagerState.Working &&
                            typingController.typingState != TypingState.Typing &&
                            uiManager.uIManagerState == UIManagerState.Complated) // Compated ba?a gelecek bunlar de?i?ecek
                        {
                            storyIndexState = StoryIndexState.Complated;

                            SceneContentChanger();
                            
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

                yield return new WaitForSeconds(0.1f);
            }

        }


        void SceneContentChanger(int value = 1)
        {
            activeSceneIndex+= value; // range kontrol edilmeli
            activeSceneContent = selectedSceneObject.SceneContent[activeSceneIndex]; // intro nas?l gelecek belki bir list yap?p s?ras?yla çektirmek mant?kl? olabilir.S?ralama belli olabilir. Son content sonra hangisi gelece?ini söylebilir.
        }

        public void FindEventData()
        {
            switch (eventList)
            {
                case EventList.Intro:
                    selectedSceneObject = intro;
                    break;
                case EventList.RavenGame:
                    selectedSceneObject = ravenGame;
                    break;
                default:
                    break;
            }

            activeSceneIndex = 0;
        }


    }

}
