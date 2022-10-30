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

        #region Serialized Fields

        // Event cache
        [SerializeField] SceneObject _intro;
        [SerializeField] SceneObject _ravenGame;

        #endregion

        #region Private Fields

        //Object Cache
        TypingController _typingController;
        SceneManager _sceneManager;
        UIManager _uiManager;

        // For StoryFind
        SceneObject _selectedSceneEvent; // scene obje için seçilen obje ayr?m? yapmam gerek.

        private StoryWorkingState _storyWorkingState;

        Coroutine _indexListenerRoutine;

        #endregion

        #region Public Properties

        public EventList EventList { get; private set; }
        
        // ForSceneManager
        public ContentOfTheEventObject ActiveSceneContent { get; private set; }
        public int ActiveSceneIndex { get; private set; }

        #endregion

        #region Base Functions

        private void Awake()
        {
            //cache self
            GameManager.Instance.SetSelfCache(this.gameObject);

            ActiveSceneIndex = 0;
            ActiveSceneContent = _intro.SceneContent[0]; // Bunlar save ile gelebilir. Bu yüzden GamaManager'de tutulmas? daha do?ru


        }

        private void Start()
        {
            _typingController = GameManager.Instance.TypingControllerCache;
            _sceneManager = GameManager.Instance.SceneManagerCache;
            _uiManager = GameManager.Instance.UIManagerCache;

        } 

        private void OnEnable()
        {
            FindEventData(EventList.Intro); //onenable daha iyi olabilir
            
        }

        #endregion



        #region Public Functions

        //find index shoud be while

        public void SceneIndexController()
        {
            SceneState _sceneState = ActiveSceneContent.sceneState;

            if (_storyWorkingState == StoryWorkingState.Working)
            {
                StopCoroutine(_indexListenerRoutine);
            }

            _indexListenerRoutine = StartCoroutine(SceneIndexControllerRou(_sceneState));

        }

        public void FindEventData(EventList eventList)
        {
            switch (eventList)
            {
                case EventList.Intro:
                    _selectedSceneEvent = _intro;
                    break;

                case EventList.RavenGame:
                    _selectedSceneEvent = _ravenGame;
                    break;

                default:
                    break;
            }

            ActiveSceneIndex = 0;
        }


        public void HardStopSceneIndexController()
        {
            StopCoroutine(_indexListenerRoutine);
            _storyWorkingState = StoryWorkingState.Complated;
        }

        public void RollBackIndex()
        {
            if (ActiveSceneIndex > 0)
            {
                ActiveSceneIndex--;
                ActiveSceneContent = _selectedSceneEvent.SceneContent[ActiveSceneIndex];
            }
        }

        #endregion


        #region Private Functions


        void SceneContentChanger(int value = 1)
        {
            if (ActiveSceneIndex + 1 > _selectedSceneEvent.SceneContent.Count -1) return;

            ActiveSceneIndex += value;
            ActiveSceneContent = _selectedSceneEvent.SceneContent[ActiveSceneIndex];
            
                  
        }


        IEnumerator SceneIndexControllerRou(SceneState sceneState)
        {
            _storyWorkingState = StoryWorkingState.Working;

            while (_storyWorkingState != StoryWorkingState.Complated)
            {


                switch (sceneState)
                {
                    case SceneState.Basic:
                        if (_sceneManager.SceneManagerState != SceneManagerState.Working &&
                            _typingController.TypingState != TypingState.Typing &&
                            _uiManager.UIManagerState == UIManagerState.Complated) // Compated ba?a gelecek bunlar de?i?ecek
                        {
                            _storyWorkingState = StoryWorkingState.Complated;

                            SceneContentChanger();

                        }

                        break;

                    case SceneState.UnSkippable: // Ayni kod birlesmeli
                        if (_sceneManager.SceneManagerState != SceneManagerState.Working &&
                            _typingController.TypingState != TypingState.Typing &&
                            _uiManager.UIManagerState == UIManagerState.Complated) // Compated ba?a gelecek bunlar de?i?ecek
                        {
                            _storyWorkingState = StoryWorkingState.Complated;

                            SceneContentChanger();

                        }
                        break;

                    case SceneState.AutoSkip: // ayni kod birlesmeli
                        
                        if (_sceneManager.SceneManagerState != SceneManagerState.Working &&
                           _typingController.TypingState != TypingState.Typing &&
                           _uiManager.UIManagerState == UIManagerState.Complated) // Compated ba?a gelecek bunlar de?i?ecek
                        {
                            _storyWorkingState = StoryWorkingState.Complated;

                            SceneContentChanger();

                        }

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
                        _storyWorkingState = StoryWorkingState.Complated;
                        break;
                }

                yield return new WaitForSeconds(0.1f);
            }

        }


        #endregion



    }

}
