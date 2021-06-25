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

        private void Start()
        {
            var questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            foreach (var questStatus in questList.GetStatuses())
            {
                var questUIInstance = Instantiate<QuestItemUI>(questPrefarb, gameObject.transform);
                questUIInstance.Setup(questStatus);
            }
        }
    }
}
