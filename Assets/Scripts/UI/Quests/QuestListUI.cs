using RPG.Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestListUI : MonoBehaviour
    {        
        [SerializeField]
        QuestItemUI questPrefarb;

        QuestList questList;

        private void Start()
        {
            questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            questList.onQuestListUpdated += RedrawUI;           
        } 
        
        private void RedrawUI()
        {
            transform.DetachChildren();
            foreach (var questStatus in questList.GetStatuses())
            {
                var questUIInstance = Instantiate<QuestItemUI>(questPrefarb, gameObject.transform);
                questUIInstance.Setup(questStatus);
            }
        }
    }
}
