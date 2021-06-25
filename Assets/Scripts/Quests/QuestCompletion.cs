using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestCompletion : MonoBehaviour
    {
        [SerializeField] Quest relatedQuest;
        [SerializeField] string objective;
        
        public void CompleteObjective()
        {
            var questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            questList.CompleteObjective(relatedQuest, objective);
        }
    }
}
