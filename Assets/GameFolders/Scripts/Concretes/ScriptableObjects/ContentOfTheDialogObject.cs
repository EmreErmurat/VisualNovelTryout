using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VisualNovelTryout.Enums;

namespace VisualNovelTryout.ScriptableObjects
{
    [System.Serializable]
    public class ContentOfTheDialogObject
    {
        public Characters characters;
        //public List<string> context = new List<string>();
        public string context;
        public Image image;

    }

}
