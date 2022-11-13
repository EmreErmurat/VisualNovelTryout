using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisualNovelTryout.Enums;

namespace VisualNovelTryout.ScriptableObjects
{
    
    [System.Serializable]
    public class ContentOfTheEventObject
    {  
        public SceneState sceneState;
        public Characters characters;
        public string context;
        public Sprite sprite;
        public float FadeTime;
        public float WaitTime; // forInput
        public bool HardStop;
        public Sprite PanImage;
        public string PanAnimation;
    }

}

