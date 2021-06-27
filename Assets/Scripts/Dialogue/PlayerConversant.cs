using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using RPG.Core;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField]
        string characterName = "Feralkin"; 

        Dialogue currentDialogue;
        DialogueNode currentNode = null;
        AIConversant currentConversant = null;
        bool isChoosing = false;

        public event Action onConversationUpdated;       

        public void StartDialogue(AIConversant aIConversant,Dialogue newDialogue)
        {
            currentConversant = aIConversant;
            currentDialogue = newDialogue;
            currentNode = currentDialogue.GetRootNode();
            TriggerEnterAction();
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
            TriggerEnterAction();
            isChoosing = false;
            Next();
        }

        public void Next()
        {
            int numPlayerResponses = FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode)).Count();
            int childrenCount = currentDialogue.GetAllChildren(currentNode).Count();
            if (childrenCount == 0)
            {                
                Quit();
                return;
            }

            if (numPlayerResponses > 0)
            {
                isChoosing = true;
                onConversationUpdated();
                return;
            }            

            DialogueNode[] currentNodeChildren = FilterOnCondition(currentDialogue.GetAIChildren(currentNode)).ToArray<DialogueNode>();
            int newNextIndex = UnityEngine.Random.Range(0, currentNodeChildren.Length);
            TriggerExitAction();
            currentNode = currentNodeChildren[newNextIndex];
            TriggerEnterAction();
            onConversationUpdated();
        }

        private IEnumerable<DialogueNode> FilterOnCondition(IEnumerable<DialogueNode> inputNodes)
        {
            foreach (var node in inputNodes)
            {
                if (node.CheckCondition(GetEvaluators()))
                {
                    yield return node;
                }
            }
        }

        private IEnumerable<IPredicateEvaluator> GetEvaluators()
        {
            return GetComponents<IPredicateEvaluator>();
        }

        public string GetCurrentConversantName()
        {
            if (currentNode == null)
            {
                return "";
            }

            return isChoosing ? characterName : currentConversant.GetNPCName();
        }

        public void Quit()
        {
            TriggerExitAction();
            currentConversant = null;
            currentDialogue = null;            
            currentNode = null;
            isChoosing = false;
            onConversationUpdated();
        }

        public bool HasNext()
        {            
            return FilterOnCondition(currentDialogue.GetAllChildren(currentNode)).Count() > 0;
        }

        public IEnumerable<DialogueNode> GetChoices()
        {
            return FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode));
        }

        private void TriggerEnterAction()
        {
            if (currentNode != null)
            {
                TriggerAction(currentNode.GetOnEnterAction());
            }
        }

        private void TriggerExitAction()
        {
            if (currentNode != null)
            {
                TriggerAction(currentNode.GetOnExitAction());
            }
        }

        private void TriggerAction(string action)
        {
            if (action == "") { return; }

            foreach (DialogueTrigger trigger in currentConversant.GetComponents<DialogueTrigger>())
            {
                trigger.Trigger(action);
            }
        }

    }
}
