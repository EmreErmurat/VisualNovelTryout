using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelTryout.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameData", menuName = "GameDatas")]
    public class GameData : ScriptableObject
    {
        public bool Intro;
        public bool RavenGame;
    }

}
