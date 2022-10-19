using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualNovelTryout.ScriptableObjects;
using VisualNovelTryout.Enums;

namespace VisualNovelTryout.Controller
{
    public class StoryController : MonoBehaviour
    {
        // Event cache
        [SerializeField] SceneObject intro;

        public ContentOfTheEventObject activeSceneContent { get; private set; }
        
        public int activeSceneIndex { get; private set; }

        private IndexState indexState;

        Coroutine IndexListenerRoutine;

        private void Awake()
        {
            //cache self
            GameManager.Instance.storyController = this;
            indexState = new IndexState();

            activeSceneIndex = 0;
            activeSceneContent = intro.SceneContent[0];

            
        }


        //find index shoud be while

        public void SceneIndexController()
        {
            SceneState sceneState = activeSceneContent.sceneState;
            
            if (indexState == IndexState.Working)
            {
                StopCoroutine(IndexListenerRoutine);
            }
            
            IndexListenerRoutine = StartCoroutine(SceneIndexListener(sceneState));
            
        }

        IEnumerator SceneIndexListener(SceneState sceneState)
        {
            indexState = IndexState.Working;

            while (indexState != IndexState.Complated)
            {
                
               
                switch (sceneState)
                {
                    case SceneState.Normal:
                        if (true && true)
                        {
                            indexState = IndexState.Complated;
                            activeSceneIndex++; // bu da bir metod olur. Yoksa bitti?ini anlamayabiliriz.
                            
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

    }

}
