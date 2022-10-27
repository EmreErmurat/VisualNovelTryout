using System.Collections.Generic;
using UnityEngine;


namespace VisualNovelTryout.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SceneContent 1", menuName = "Events/SceneObject")]
    public class SceneObject : ScriptableObject
    {
        
        public int ID;
        public List<ContentOfTheEventObject> SceneContent = new List<ContentOfTheEventObject>();


    }
      
}
