using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VisualNovelTryout.Controller;
using VisualNovelTryout.Inputs;
using VisualNovelTryout.Enums;
using VisualNovelTryout.ScriptableObjects;
using VisualNovelTryout.UI;
using VisualNovelTryout.Animation;

namespace VisualNovelTryout.Manager
{
    public class SceneManager : MonoBehaviour
    {

        #region Serialized Fields

        [SerializeField] TextBoxAnimationController _textBoxAnimationController;

        #endregion

        PcInputsReceiver _pcInputsReceiver;
        StoryController _storyController;
        TypingController _typingController;
        UIManager _uIManager;

        #region Private Fields

        // For SceneManager
        bool _gameIsRunning = true;
        bool _normalStateIsActive;
        bool _unSkippableStateIsActive;

        Coroutine _basicStateCoroutine;

        IEnumerator _selectedDetailedEvent;

        #endregion

        #region Public Properties

        public SceneManagerState SceneManagerState { get; private set; }

        #endregion

        #region Base Functions

        private void Awake()
        {
            GameManager.Instance.SetSelfCache(this.gameObject);
            _pcInputsReceiver = new PcInputsReceiver();

        }

        private void Start()
        {
            _storyController = GameManager.Instance.StoryControllerCache;
            _typingController = GameManager.Instance.TypingControllerCache;
            _uIManager = GameManager.Instance.UIManagerCache;

        }


        private void Update()
        {

            if (_pcInputsReceiver.MouseZeroInput && _gameIsRunning)
            {

                StartWorking();

            }

        }
        #endregion



        #region Public Functions

        public void StartWorking()
        {
            if (!_gameIsRunning) return;

            SceneManagerState = SceneManagerState.Working;

            _gameIsRunning = false;

            FindDetailedEvent();

            StartCoroutine(_selectedDetailedEvent);

        }

        #endregion


        #region Private Functions


        void FindDetailedEvent()
        {
            switch (_storyController.EventList)
            {
                case EventList.Intro:
                    _selectedDetailedEvent = DetailedIntro();
                    break;

                case EventList.RavenGame:
                    _selectedDetailedEvent = DetailedRavenGame();
                    break;

                default:
                    break;
            }

        }

        IEnumerator InputCooldown(float value = .5f, bool outoSkip = false)
        {
            yield return new WaitForSeconds(value);
            _gameIsRunning = true;
            if (outoSkip)
            {
                StartWorking();
            }
        }

        void DelegateTheDuties()
        {

            switch (_storyController.ActiveSceneContent.sceneState)
            {
                case SceneState.Basic:

                    StartCoroutine(InputCooldown());

                    if (_normalStateIsActive || _typingController.TypingState == TypingState.Typing || _uIManager.UIManagerState != UIManagerState.Complated)
                    {
                        if (_normalStateIsActive)
                        {
                            StopCoroutine(_basicStateCoroutine);
                            _normalStateIsActive = false;
                        }

                        _uIManager.ChangeImage(_storyController.ActiveSceneContent.sprite, _storyController.ActiveSceneContent.FadeTime);
                        _typingController.TypeDialogue(_storyController.ActiveSceneContent.context, _storyController.ActiveSceneContent.characters);

                        SceneManagerState = SceneManagerState.Complated;

                    }
                    else
                    {
                        _basicStateCoroutine = StartCoroutine(BasicStateRoutine());
                    }

                    break;

                case SceneState.UnSkippable:

                    StartCoroutine(InputCooldown(_storyController.ActiveSceneContent.WaitTime));

                    StartCoroutine(UnSkippableRoutine());

                    break;

                case SceneState.AutoSkip:

                    StartCoroutine(InputCooldown(_storyController.ActiveSceneContent.WaitTime, true));

                    StartCoroutine(UnSkippableRoutine());

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

            _storyController.SceneIndexController();
        }


        IEnumerator BasicStateRoutine()
        {
            _normalStateIsActive = true;

            if (_storyController.ActiveSceneContent.sprite != _uIManager.LastSprite)
            {
                _textBoxAnimationController.TextBoxSetTransiton(false);
            }

            _uIManager.ChangeImage(_storyController.ActiveSceneContent.sprite, _storyController.ActiveSceneContent.FadeTime);

            yield return new WaitForSeconds(_storyController.ActiveSceneContent.FadeTime);

            _typingController.TypeDialogue(_storyController.ActiveSceneContent.context, _storyController.ActiveSceneContent.characters);

            SceneManagerState = SceneManagerState.Complated;

            _normalStateIsActive = false;
        }

        IEnumerator UnSkippableRoutine() // Basic ile ayni kod. Birlestirilmesi gerek
        {
            _unSkippableStateIsActive = true;

            if (_storyController.ActiveSceneContent.sprite != _uIManager.LastSprite)
            {
                _textBoxAnimationController.TextBoxSetTransiton(false);
            }

            _uIManager.ChangeImage(_storyController.ActiveSceneContent.sprite, _storyController.ActiveSceneContent.FadeTime);

            yield return new WaitForSeconds(_storyController.ActiveSceneContent.FadeTime);

            _typingController.TypeDialogue(_storyController.ActiveSceneContent.context, _storyController.ActiveSceneContent.characters);

            SceneManagerState = SceneManagerState.Complated;

            _unSkippableStateIsActive = false;
        }

        #endregion




















        #region Scene Events // Elaborated Codes

        IEnumerator DetailedIntro()
        {
            switch (_storyController.ActiveSceneIndex)
            {


                case 10:

                    print(10);
                    DelegateTheDuties();
                    // standart ?ekilde devam edecek...
                    //storyController.eventList = EventList.RavenGame;
                    yield return new WaitForSeconds(0.3f);
                    _storyController.FindEventData(EventList.RavenGame);

                    break;



                default:
                    DelegateTheDuties();
                    break;
            }

            yield return null;
        }

        IEnumerator DetailedRavenGame()
        {
            switch (_storyController.ActiveSceneIndex)
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
