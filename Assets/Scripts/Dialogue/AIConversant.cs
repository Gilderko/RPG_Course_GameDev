using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.Attributes;

namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour, IRaycastable
    {
        [SerializeField] Dialogue dialogue = null;
        [SerializeField] string npcName = "Guardsman";

        public CursorType GetCursorType()
        {
            return CursorType.Dialogue;
        }

        public string GetNPCName()
        {
            return npcName;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (dialogue == null || gameObject.GetComponent<Health>().IsDead())
            {
                return false;
            }

            if (Input.GetMouseButtonDown(0))
            {
                PlayerConversant playerConversant = callingController.GetComponent<PlayerConversant>();
                playerConversant.StartDialogue(this, dialogue);
            }
            return true;
        }        
    }
}
