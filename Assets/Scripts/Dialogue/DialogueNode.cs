using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    [System.Serializable]
    public class DialogueNode
    {
        public string uniqueID;
        public string text;
        public string[] dialogueChildren;
        public Rect inEditorPosition = new Rect(10,10,100,100);
    }
}
