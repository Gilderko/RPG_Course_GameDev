using RPG.Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestListUI : MonoBehaviour
    {
        [SerializeField]
        List<Quest> tempQuests;

        [SerializeField]
        QuestItemUI questPrefarb;

        private void Start()
        {
            foreach (var quest in tempQuests)
            {
                var questUIInstance = Instantiate<QuestItemUI>(questPrefarb, gameObject.transform);
                questUIInstance.Setup(quest);
            }
        }
    }
}
