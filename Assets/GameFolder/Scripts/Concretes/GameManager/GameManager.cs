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
    public TypingController typingController { get; set; }
    public UIManager uIManager { get; set; }




    // GameData

    public float typingSpeed { get; private set; }

    private void Awake()
    {
        
        SingletonObject();

        typingSpeed = 0.05f; // bu veri kay?t ettirilecek
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
