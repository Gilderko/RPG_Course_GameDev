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
        public List<string> dialogueChildren = new List<string>();
        public Rect inEditorPosition = new Rect(10,10,200,200);

        public DialogueNode()
        {
            uniqueID = System.Guid.NewGuid().ToString();
        }
    }
}
