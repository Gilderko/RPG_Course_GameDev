using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] Dialogue currentDialogue;
        DialogueNode currentNode;

        private void Awake()
        {
            currentNode = currentDialogue.GetRootNode();
        }

        public string GetText()
        {
            if (currentNode == null)
            {
                return "";
            }
           return currentNode.GetText();
        }

        public void Next()
        {
            DialogueNode[] currentNodeChildren = currentDialogue.GetAllChildren(currentNode).ToArray<DialogueNode>();
            int newNextIndex = Random.Range(0, currentNodeChildren.Length);
            currentNode = currentNodeChildren[newNextIndex];
        }

        public bool HasNext()
        {            
            return currentDialogue.GetAllChildren(currentNode).Count() > 0;
        }

        public IEnumerable<string> GetChoices()
        {
            yield return "Something";
            yield return "Nothing";
            yield return "Everything";
        }
    }
}
