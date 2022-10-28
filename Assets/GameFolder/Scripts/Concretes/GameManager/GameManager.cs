using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualNovelTryout.Controller;
using VisualNovelTryout.Manager;

public class GameManager : MonoBehaviour
{

    #region Static Field

    public static GameManager Instance { get; private set; }

    #endregion

    #region Private Fields

    // Object Cache
    StoryController _storyController;
    SceneManager _sceneManager;
    TypingController _typingController;
    UIManager _uIManager;

    // GameData

    float _tyipngSpeed;



    #endregion

    #region Public Properties

    // Object Cache
    public StoryController StoryControllerCache => _storyController;
    public SceneManager SceneManagerCache => _sceneManager;
    public TypingController TypingControllerCache => _typingController;
    public UIManager UIManagerCache => _uIManager;


    //Game Data
    public float TypingSpeed => _tyipngSpeed;

    #endregion

    #region Base Fuctions

    private void Awake()
    {
        SingletonObject();

        _tyipngSpeed = 0.05f; // bu veri kayit ettirilecek
    }

    #endregion

    #region Public Functions

    public void SetSelfCache(GameObject gameObject)
    {
        if (gameObject.GetComponent<StoryController>() != null)
        {
            _storyController = gameObject.GetComponent<StoryController>();
        }
        else if (gameObject.GetComponent<SceneManager>() != null)
        {
            _sceneManager = gameObject.GetComponent<SceneManager>();
        }
        else if (gameObject.GetComponent<TypingController>() != null)
        {
            _typingController = gameObject.GetComponent<TypingController>();
        }
        else if (gameObject.GetComponent<UIManager>() != null)
        {
            _uIManager = gameObject.GetComponent<UIManager>();
        }
        else
        {
            return;
        }

    }

    #endregion

    #region Private Function

    void SingletonObject()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    #endregion


   

}
