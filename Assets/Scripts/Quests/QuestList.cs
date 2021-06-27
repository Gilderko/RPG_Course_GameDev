using GameDevTV.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour, ISaveable
    {
        public List<QuestStatus> statuses = new List<QuestStatus>();

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

        public void CompleteObjective(Quest quest, string objective)
        {
            var questStat = GetQuestStatus(quest);
            if (questStat == null || questStat.IsObjectiveComplete(objective))
            {
                return;
            }

            questStat.CompleteObjective(objective);
            onQuestListUpdated();
        }

        private QuestStatus GetQuestStatus(Quest quest)
        {
            return statuses.First(status => status.GetQuest() == quest);
        }

        public object CaptureState()
        {
            List<object> state = new List<object>();
            foreach (var status in statuses)
            {
                print(status.CaptureState());
                state.Add(status.CaptureState());
            }
            print(state.Count);
            return state;
        }

        public void RestoreState(object state)
        {
            var loadedStatuses = state as List<object>;
            if (loadedStatuses == null)
            {
                return;
            }

            statuses.Clear();
            foreach (var objectState in loadedStatuses)
            {
                statuses.Add(new QuestStatus(objectState));                
            }
        }
    }
}
