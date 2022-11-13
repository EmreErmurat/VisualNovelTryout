using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualNovelTryout.UI;
using VisualNovelTryout.Enums;
using UnityEngine.UI;

namespace VisualNovelTryout.Controller
{
    public class PanController : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] GameObject _panPanel;

        #endregion

        #region Private Fields

        TransitionHolder _transitionHolder;
        PanAnimationController _panAnimationController;

        CanvasGroup _panCanvasGroup;
        Image _panPanelImage;

        Coroutine _panAnimationCoroutine;
        #endregion

        #region Public Properties

        public PanState PanState { get; private set; }

        #endregion

        #region Base Functions

        private void Awake()
        {

            //Object Cache
            _transitionHolder = GetComponent<TransitionHolder>();

            _panAnimationController = _panPanel.GetComponent<PanAnimationController>();
            _panCanvasGroup = _panPanel.GetComponent<CanvasGroup>();
            _panPanelImage = _panPanel.GetComponent<Image>();

            // Self Cache
            GameManager.Instance.SetSelfCache(this.gameObject);
        }

        #endregion

        #region Public Functions
        public void PanPanelActivator(bool isActive)
        {
            if (isActive)
            {
                _transitionHolder.FadeIn(_panCanvasGroup, .5f);
            }
            else
            {
                StartCoroutine(PanPanelCloseRoutine());

            }

        }


        public void RunPanAnimation(Sprite panImage, string trigger)
        {
            PanState = PanState.Running;

            _panAnimationCoroutine = StartCoroutine(PanAnimationRoutine(panImage, trigger)); 

           
        }

            

        public void PanStateController()
        {
            PanState = PanState.Complated;
        }


        public void HardStopPanController()
        {
            StopCoroutine(_panAnimationCoroutine);
            StartCoroutine(PanPanelCloseRoutine(true));
        }

        #endregion

        #region Private Functions



        IEnumerator PanAnimationRoutine(Sprite panImage, string trigger)
        {

            if (!_panCanvasGroup.gameObject.activeSelf)
            {
                _panCanvasGroup.alpha = 0;
                _panPanelImage.sprite = panImage;
                _panCanvasGroup.gameObject.SetActive(true);
                PanPanelActivator(true);
                yield return new WaitForSeconds(.5f);
            }
            else
            {
                _transitionHolder.FadeOut(_panCanvasGroup, .5f);
                yield return new WaitForSeconds(.5f);
                _panPanelImage.sprite = panImage;
                _panAnimationController.ExitAnimation();
                _transitionHolder.FadeIn(_panCanvasGroup, .5f);
                yield return new WaitForSeconds(.5f);
            }
            
            _panAnimationController.RunPanAnimation(trigger);

            

        }

        IEnumerator PanPanelCloseRoutine(bool hardStop = false)
        {

            if (hardStop)
            {
                _transitionHolder.FadeOut(_panCanvasGroup, .1f);
                _panAnimationController.ExitAnimation();
                yield return new WaitForSeconds(.1f);

            }
            else
            {
                _transitionHolder.FadeOut(_panCanvasGroup, .5f);

                yield return new WaitForSeconds(.5f);

                _panAnimationController.ExitAnimation();
                yield return new WaitForSeconds(.1f);
            }

            _panCanvasGroup.gameObject.GetComponent<Image>().sprite = null;
            _panCanvasGroup.gameObject.SetActive(false);

            PanState = PanState.Complated;

        }

        #endregion











    }

}
