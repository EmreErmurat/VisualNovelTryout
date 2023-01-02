using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualNovelTryout.Manager;
using VisualNovelTryout.Controller;
using VisualNovelTryout.Enums;

public class DetailedEventBase : MonoBehaviour
{
    StoryController _storyController;
    SceneManager _sceneManager;

    IEnumerator _selectedDetailedEvent;

    private void Start()
    {
        _storyController = GameManager.Instance.StoryControllerCache;
        _sceneManager = GameManager.Instance.SceneManagerCache;
    }

    public void FindDetailedEvent() // Buradan yapamay?z. Her Scene icin farkli olacak. Nasil yapilacak buradan.
    {
        switch (_storyController.EventList)
        {
            case EventList.Intro:
                _selectedDetailedEvent = DetailedIntro(); // buray? bir ba?ka dosyadan çekece?iz. Her bölüm için farkl? olacak.
                break;

            case EventList.RavenGame:
                _selectedDetailedEvent = DetailedRavenGame();
                break;

            default:
                break;
        }

        StartCoroutine(_selectedDetailedEvent);
    }


    #region Scene Events // Detailed Codes

    IEnumerator DetailedIntro()
    {
        switch (_storyController.ActiveSceneIndex)
        {

            case 5:

                if (true)
                {
                    //_storyController.ActiveSceneContent.context = "Here we go. Back to Home again...";
                    _storyController.ActiveSceneContent.context = "Emre";
                }
                else
                {
                    _storyController.ActiveSceneContent.context = "Here we go. Back to Nicole's again...";
                }
                _sceneManager.DelegateTheDuties();
                break;

            case 10:

                print(10);
                _sceneManager.DelegateTheDuties();
                // standart sekilde devam edecek...
                //storyController.eventList = EventList.RavenGame;
                yield return new WaitForSeconds(0.3f);
                _storyController.FindEventData(EventList.RavenGame);

                break;



            default:
                _sceneManager.DelegateTheDuties();
                break;
        }

        yield return null;
    }

    IEnumerator DetailedRavenGame()
    {
        switch (_storyController.ActiveSceneIndex)
        {


            default:
                _sceneManager.DelegateTheDuties();
                break;
        }


        yield return null;
    }

    #endregion

}
