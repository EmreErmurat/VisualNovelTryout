using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VisualNovelTryout.UI
{
    public class PanAnimationController : MonoBehaviour
    {

        #region Private Fields

        Animator _panAnimator;


        #endregion

        #region Base Functions

        private void Awake()
        {
            _panAnimator = GetComponent<Animator>();

        }


        #endregion

        #region Public Functions

        public void RunPanAnimation(string trigger)
        {
            _panAnimator.SetTrigger(trigger);
        }

        public void ExitAnimation()
        {
            _panAnimator.SetTrigger("exit");
            
        }

        #endregion







    }

}
