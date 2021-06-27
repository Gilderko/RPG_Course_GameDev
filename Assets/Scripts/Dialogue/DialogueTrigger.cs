using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Dialogue
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(AIConversant))]
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField]
        [Dropdown("dialogueActions")]
        string actionRespond;

        [SerializeField]
        UnityEvent onTrigger;

        List<string> dialogueActions;

        private void Update()
        {
            var dialogue = GetComponent<AIConversant>().GetCorrespondingDialogue();
            var dialogueEnterActions = dialogue.GetAllNodes()
                .Where(diaNode => diaNode.GetOnEnterAction() != "")
                .Select(diaNode => diaNode.GetOnEnterAction());
            var dialogueExitActions = dialogue.GetAllNodes()
                .Where(diaNode => diaNode.GetOnExitAction() != "")
                .Select(diaNode => diaNode.GetOnExitAction());

            dialogueActions = dialogueEnterActions.Union(dialogueExitActions).ToList();
        }

        public void Trigger(string actionToTrigger)
        {
            if (actionToTrigger == actionRespond)
            {
                onTrigger.Invoke();
            }
        }
    }
}
