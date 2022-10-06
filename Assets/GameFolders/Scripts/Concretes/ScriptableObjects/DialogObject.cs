using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VisualNovelTryout.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Dialog 1", menuName = "Dialogs/DialogsObject")]
    public class DialogObject : ScriptableObject
    {
        public int ID;
        public List<ContentOfTheDialogObject> eventDialogs = new List<ContentOfTheDialogObject>();
    }

}
