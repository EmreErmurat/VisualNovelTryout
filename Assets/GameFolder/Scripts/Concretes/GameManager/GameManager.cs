using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualNovelTryout.Controller;
using VisualNovelTryout.Manager;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Object cache
    public StoryController storyController { get; set; }
    public SceneManager sceneManager { get; set; }


    private void Awake()
    {
        
        SingletonObject();

    }

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

}
