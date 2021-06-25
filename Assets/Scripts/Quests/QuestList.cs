using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour
    {
        List<QuestStatus> statuses = new List<QuestStatus>();

        public event Action onQuestListUpdated;

        public void AddQuest(Quest quest)
        {
            if (statuses.Select(status => status.GetQuest()).Contains(quest))
            {
                return;
            }

            var questStatus = new QuestStatus(quest);            
            statuses.Add(questStatus);
            onQuestListUpdated();
        }

        public IEnumerable<QuestStatus> GetStatuses()
        {
            return statuses;
        }
    }
}
