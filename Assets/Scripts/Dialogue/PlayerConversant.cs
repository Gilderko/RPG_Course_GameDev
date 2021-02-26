using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] Dialogue testDialogue;
        Dialogue currentDialogue;
        DialogueNode currentNode;
        bool isChoosing = false;

        public event Action onConversationUpdated;

        private void Awake()
        {
            
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(2);
            StartDialogue(testDialogue);
        }

        public void StartDialogue(Dialogue newDialogue)
        {
            currentDialogue = newDialogue;
            currentNode = currentDialogue.GetRootNode();
            onConversationUpdated();
        }

        public bool IsDialogueActive()
        {
            return currentDialogue != null;
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
        
        public void SelectChoice(DialogueNode chosenNode)
        {
            currentNode = chosenNode;
            isChoosing = false;
            Next();
        }

        public void Next()
        {
            int numPlayerResponses = currentDialogue.GetPlayerChildren(currentNode).Count();
            if (numPlayerResponses > 0)
            {
                isChoosing = true;
                onConversationUpdated();
                return;
            }

            DialogueNode[] currentNodeChildren = currentDialogue.GetAIChildren(currentNode).ToArray<DialogueNode>();
            int newNextIndex = UnityEngine.Random.Range(0, currentNodeChildren.Length);
            currentNode = currentNodeChildren[newNextIndex];
            onConversationUpdated();
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
