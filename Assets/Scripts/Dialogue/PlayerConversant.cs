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
        bool isChoosing = false;

        private void Awake()
        {
            currentNode = currentDialogue.GetRootNode();
        }

        public bool IsChoosing()
        {
            return isChoosing;
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
            int numPlayerResponses = currentDialogue.GetPlayerChildren(currentNode).Count();
            if (numPlayerResponses > 0)
            {
                isChoosing = true;
                return;
            }

            DialogueNode[] currentNodeChildren = currentDialogue.GetAIChildren(currentNode).ToArray<DialogueNode>();
            int newNextIndex = Random.Range(0, currentNodeChildren.Length);
            currentNode = currentNodeChildren[newNextIndex];
        }

        public bool HasNext()
        {            
            return currentDialogue.GetAllChildren(currentNode).Count() > 0;
        }

        public IEnumerable<DialogueNode> GetChoices()
        {
            foreach (DialogueNode node in currentDialogue.GetPlayerChildren(currentNode))
            {
                yield return node;
            }
        }
    }
}
