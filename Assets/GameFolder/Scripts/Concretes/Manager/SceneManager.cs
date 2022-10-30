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


        #region Private Fields

        //Object Cache
        PcInputsReceiver _pcInputsReceiver;
        StoryController _storyController;
        TypingController _typingController;
        UIManager _uIManager;

        // For SceneManager
        bool _gameIsRunning = true;
        bool _basicStateIsActive;
        bool _unSkippableStateIsActive;


        // For Works Delegate
        Coroutine _basicStateCoroutine;

        // For GameEvent
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

            if (_pcInputsReceiver.JumpInput) 
            {
                RollBackGame();
            }

        }

        private void OnEnable()
        {
            Invoke("StartWorking", .1f); // First click when Load Scene
            
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

        public void RollBackGame()
        {
            _storyController.HardStopSceneIndexController();
            _uIManager.HardStopChangeImage();
            _typingController.HardStopTyping();

            _storyController.RollBackIndex();
            StartWorking();
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


        void DelegateTheDuties()
        {

            switch (_storyController.ActiveSceneContent.sceneState)
            {
                case SceneState.Basic:

                    StartCoroutine(InputCooldown());

                    if (_basicStateIsActive || _typingController.TypingState == TypingState.Typing || _uIManager.UIManagerState != UIManagerState.Complated)
                    {
                        if (_basicStateIsActive)
                        {
                            StopCoroutine(_basicStateCoroutine);
                            _basicStateIsActive = false;
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

                    if (_basicStateIsActive)
                    {
                        StopCoroutine(_basicStateCoroutine);
                        _basicStateIsActive = false;
                    
                    }

                    StartCoroutine(InputCooldown(_storyController.ActiveSceneContent.WaitTime));

                    StartCoroutine(BasicStateRoutine());    // There is no difference with BasicState.

                    break;

                case SceneState.AutoSkip:   

                    if (_basicStateIsActive)
                    {
                        StopCoroutine(_basicStateCoroutine);
                        _basicStateIsActive = false;

                    }

                    StartCoroutine(InputCooldown(_storyController.ActiveSceneContent.WaitTime, true));

                    StartCoroutine(BasicStateRoutine());    // There is no difference with BasicState. 

                    break;

                case SceneState.ChoiceMenu: // Bir liste ile oyunda ilerlenen bölümleri tutmak ve daha sonra geri dönüslerde bu listeden yararlanmak mantikli olabilir. Yoksa catallasmalarda bulundugun yeri kaybedecegiz. 
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


        IEnumerator InputCooldown(float value = .5f, bool outoSkip = false)
        {
            yield return new WaitForSeconds(value);
            _gameIsRunning = true;
            if (outoSkip)
            {
                StartWorking();
            }
        }

        // Game Routine

        IEnumerator BasicStateRoutine()
        {
            _basicStateIsActive = true;

            if (_storyController.ActiveSceneContent.sprite != _uIManager.LastSprite)
            {       
                _typingController.TextBoxController(false);
            }

            _uIManager.ChangeImage(_storyController.ActiveSceneContent.sprite, _storyController.ActiveSceneContent.FadeTime);

            if (_uIManager.UIManagerState != UIManagerState.Complated)
            {
                yield return new WaitForSeconds(_storyController.ActiveSceneContent.FadeTime);
            }

            _typingController.TypeDialogue(_storyController.ActiveSceneContent.context, _storyController.ActiveSceneContent.characters);

            SceneManagerState = SceneManagerState.Complated;

            _basicStateIsActive = false;
        }


            #region Scene Events // Detailed Codes

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



        #endregion






    }

}
