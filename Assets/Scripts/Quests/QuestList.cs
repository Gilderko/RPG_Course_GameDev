using GameDevTV.Inventories;
using GameDevTV.Saving;
using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestList : MonoBehaviour, ISaveable, IPredicateEvaluator
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

            if (questStat.IsQuestComplete())
            {
                GiveReward(quest);
            }

            onQuestListUpdated();
        }

        private void GiveReward(Quest quest)
        {
            foreach (var reward in quest.GetQuestRewards())
            {
                var rewardSucces = GetComponent<Inventory>().AddToFirstEmptySlot(reward.GetItemReward(), reward.GetHowMany());
                if (!rewardSucces)
                {
                    GetComponent<ItemDropper>().DropItem(reward.GetItemReward(), reward.GetHowMany());
                }
            }
        }

        private QuestStatus GetQuestStatus(Quest quest)
        {
            return statuses.First(status => status.GetQuest() == quest);
        }

        public bool HasQuest(Quest quest)
        {
            return statuses.Select(stat => stat.GetQuest()).Contains(quest);
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

        public bool? Evaluate(PredicateTypes predicate, List<string> parameters)
        {
            switch (predicate)
            {
                case PredicateTypes.HasQuest:
                    return HasQuest(Quest.GetByName(parameters[0]));
                case PredicateTypes.CompletedQuest:
                    return HasQuest(Quest.GetByName(parameters[0])) && GetQuestStatus(Quest.GetByName(parameters[0])).IsQuestComplete();
            }
            return null;
        }
    }
}
