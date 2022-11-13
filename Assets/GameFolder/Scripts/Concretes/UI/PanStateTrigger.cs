using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualNovelTryout.Controller;

namespace VisualNovelTryout.UI
{
    public class PanStateTrigger : MonoBehaviour
    {

        #region Private Fields

        PanController _panController;

        #endregion

        #region Base Functions

        private void Start()
        {
            _panController = GameManager.Instance.PanControllerCache;
        }

        #endregion

        #region Public Functions

        public void StateTrigger()
        {
            _panController.PanStateController();
        }

        #endregion



    }

}
