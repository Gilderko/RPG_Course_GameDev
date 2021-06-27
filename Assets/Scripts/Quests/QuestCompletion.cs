using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Quests
{
    [ExecuteInEditMode]
    public class QuestCompletion : MonoBehaviour
    {
        [SerializeField] Quest relatedQuest;

        [Dropdown("objectiveOptions")]
        [SerializeField] string objective;
        
        List<string> objectiveOptions;

        private void Update()
        {
            objectiveOptions = GetQuestReferences();
        }

        public void CompleteObjective()
        {
            var questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            Debug.Log($"Completing {objective}");
            questList.CompleteObjective(relatedQuest, objective);
        }

        public List<string> GetQuestReferences()
        {
            if (relatedQuest == null)
            {
                return new List<string>();
            }
            var references = relatedQuest.GetObjectives().Select(obj => obj.GetReference());
            return references.ToList();
        }
    }
}
