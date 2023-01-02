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
        PanController _panController;

        // For SceneManager
        bool _gameIsRunning = true;

        bool _basicStateIsActive;
        bool _unSkippableStateIsActive;
        bool _panStateIsActive;


        bool _panPanelIsActive;
        // For Works Delegate
        Coroutine _InputCooldownCoroutine;

        Coroutine _basicStateCoroutine;
        Coroutine _panStateCoroutine;
        

        // For GameEvent
        [SerializeField] IEnumerator _selectedDetailedEvent;
        DetailedEventBase _detailedEventBase;

        #endregion

        #region Public Properties

        public SceneManagerState SceneManagerState { get; private set; }

        #endregion

        #region Base Functions

        private void Awake()
        {
            GameManager.Instance.SetSelfCache(this.gameObject);
            _pcInputsReceiver = new PcInputsReceiver();
            _detailedEventBase = GetComponent<DetailedEventBase>();
        }

        private void Start()
        {
            _storyController = GameManager.Instance.StoryControllerCache;
            _typingController = GameManager.Instance.TypingControllerCache;
            _uIManager = GameManager.Instance.UIManagerCache;
            _panController = GameManager.Instance.PanControllerCache;
            
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

            if (_panPanelIsActive && _storyController.ActiveSceneContent.sceneState != SceneState.Pan)
            {
                _panController.PanPanelActivator(false);
                _panPanelIsActive = false;
            }

            SceneManagerState = SceneManagerState.Working;

            _gameIsRunning = false;

            //FindDetailedEvent();
            _detailedEventBase.FindDetailedEvent();

            //StartCoroutine(_selectedDetailedEvent);

        }

        public void RollBackGame()
        {
            HardStopSceneManager(); // + Detailed Codes rutinlerini de getirmemiz gerekir.
            _storyController.HardStopSceneIndexController();
            _uIManager.HardStopChangeImage();
            _typingController.HardStopTyping();

            _storyController.RollBackIndex();
            StartWorking();
        }

        #endregion


        #region Private Functions

        

        //void FindDetailedEvent() // Buradan yapamay?z. Her Scene icin farkli olacak. Nasil yapilacak buradan.
        //{
        //    switch (_storyController.EventList)
        //    {
        //        case EventList.Intro:
        //            _selectedDetailedEvent = DetailedIntro(); // buray? bir ba?ka dosyadan çekece?iz. Her bölüm için farkl? olacak.
        //            break;

        //        case EventList.RavenGame:
        //            _selectedDetailedEvent = DetailedRavenGame();
        //            break;

        //        default:
        //            break;
        //    }

        //}


        public void DelegateTheDuties()
        {

            switch (_storyController.ActiveSceneContent.sceneState)
            {
                case SceneState.Basic:

                    _InputCooldownCoroutine = StartCoroutine(InputCooldown());

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

                    _InputCooldownCoroutine = StartCoroutine(InputCooldown(_storyController.ActiveSceneContent.WaitTime, false , SceneState.UnSkippable));

                    _basicStateCoroutine = StartCoroutine(BasicStateRoutine());    // There is no difference with BasicState.

                    break;

                case SceneState.AutoSkip:   

                    if (_basicStateIsActive)
                    {
                        StopCoroutine(_basicStateCoroutine);
                        _basicStateIsActive = false;

                    }

                    _InputCooldownCoroutine = StartCoroutine(InputCooldown(_storyController.ActiveSceneContent.WaitTime, _storyController.ActiveSceneContent.HardStop, SceneState.AutoSkip));

                    _basicStateCoroutine = StartCoroutine(BasicStateRoutine());    // There is no difference with BasicState. 

                    break;

                case SceneState.ChoiceMenu: // Bir liste ile oyunda ilerlenen bölümleri tutmak ve daha sonra geri dönüslerde bu listeden yararlanmak mantikli olabilir. Yoksa catallasmalarda bulundugun yeri kaybedecegiz. 
                    break;

                case SceneState.Pan:

                    /* Oyun durdurulacak
                     * Textbox kapat?lacak.
                     * Background Image kapat?lacak.
                     * Pan Image de?i?tirelecek
                     * PanBackground aç?lacak.
                     * Pan Animation oynat?lacak.
                     * Pan Bekleme süresince durdurulacak.
                     * Animasyon bitiminde Input beklenecek.
                     * Input sonras? PanBackground kapat?lacak.
                     * Pan Image de?i?tirilecek.
                     * Background Image de?i?ecek
                     * Background Panel aç?lacak.
                     * Gerekiyorsa Textbox aç?lacak ve rutine dönülecek.
                     */
                    _InputCooldownCoroutine = StartCoroutine(InputCooldown(default, default, SceneState.Pan)); // Pan icin ayri yapilacak
                    _panStateCoroutine = StartCoroutine(PanStateRoutine());

                    _panPanelIsActive = true;

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


        private void HardStopSceneManager()
        {
            if (_basicStateIsActive)
            {
                StopCoroutine(_basicStateCoroutine);
                _basicStateIsActive = false;
            }
            if (!_gameIsRunning)
            {
                StopCoroutine(_InputCooldownCoroutine);
                _gameIsRunning = true;
            }
            if (_panStateIsActive) //Pan da durdurulmal?
            {
                StopCoroutine(_panStateCoroutine);
                _panController.HardStopPanController();
                _panPanelIsActive = false;
            }
            
        }

        IEnumerator InputCooldown(float value = .5f,  bool hardStop = false, SceneState sceneState = SceneState.Basic)
        {
            switch (sceneState)
            {
                case SceneState.Basic:

                    yield return new WaitForSeconds(value);
                    _gameIsRunning = true;
                    
                    break;
                
                case SceneState.UnSkippable:

                    yield return new WaitForSeconds(value);
                    _gameIsRunning = true;
                    
                    break;
                
                case SceneState.AutoSkip:

                    yield return new WaitForSeconds(.5f);
                   
                    if (!hardStop)
                    {            
                        _gameIsRunning = true;
                    }

                    yield return new WaitForSeconds(value - .5f);

                    _gameIsRunning = true;
                    StartWorking();
                    
                    break;

                case SceneState.ChoiceMenu:
                    break;

                case SceneState.Pan:
                    
                    yield return new WaitForSeconds(1);

                    while (_panController.PanState != PanState.Complated)
                    {
                        yield return new WaitForSeconds(.05f);
                    }

                    yield return new WaitForSeconds(.1f);
                    _gameIsRunning = true;

                    break;

                case SceneState.Animation:
                    break;
                case SceneState.End:
                    break;
                default:

                    yield return new WaitForSeconds(value);
                    _gameIsRunning = true;

                    break;
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


        IEnumerator PanStateRoutine()
        {
            /* Oyun durdurulacak
                    * Textbox kapat?lacak.
                    * Background Image kapat?lacak.
                    * Pan Image de?i?tirelecek
                    * PanBackground aç?lacak.
                    * Pan Animation oynat?lacak.
                    * Pan Bekleme süresince durdurulacak.
                    * Animasyon bitiminde Input beklenecek.
                    * Input sonras? PanBackground kapat?lacak.
                    * Pan Image de?i?tirilecek.
                    * Background Image de?i?ecek
                    * Background Panel aç?lacak.
                    * Gerekiyorsa Textbox aç?lacak ve rutine dönülecek.
                    */

            _panStateIsActive = true;

            _typingController.TextBoxController(false);
            _uIManager.CloseBackgroundImage(_storyController.ActiveSceneContent.FadeTime);
            
            yield return new WaitForSeconds(_storyController.ActiveSceneContent.FadeTime);

            _panController.RunPanAnimation(_storyController.ActiveSceneContent.PanImage, _storyController.ActiveSceneContent.PanAnimation);

            _panStateIsActive = false;

            SceneManagerState = SceneManagerState.Complated;
        }


        
            //#region Scene Events // Detailed Codes

            //IEnumerator DetailedIntro()
            //{
            //    switch (_storyController.ActiveSceneIndex)
            //    {

            //        case 5:

            //        if (true)
            //        {
            //            _storyController.ActiveSceneContent.context = "Here we go. Back to Home again...";
            //        }
            //        else
            //        {
            //            _storyController.ActiveSceneContent.context = "Here we go. Back to Nicole's again...";
            //        }
            //        DelegateTheDuties();
            //        break;

            //        case 10:

            //            print(10);
            //            DelegateTheDuties();
            //            // standart sekilde devam edecek...
            //            //storyController.eventList = EventList.RavenGame;
            //            yield return new WaitForSeconds(0.3f);
            //            _storyController.FindEventData(EventList.RavenGame);

            //            break;



            //        default:
            //            DelegateTheDuties();
            //            break;
            //    }

            //    yield return null;
            //}

            //IEnumerator DetailedRavenGame()
            //{
            //    switch (_storyController.ActiveSceneIndex)
            //    {


            //        default:
            //            DelegateTheDuties();
            //            break;
            //    }


            //    yield return null;
            //}

            //#endregion



        #endregion






    }

}
