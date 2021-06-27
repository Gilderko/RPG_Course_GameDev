using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Inventories;

namespace RPG.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "RPG Project/QUest", order = 0)]
    public class Quest : ScriptableObject
    {
        [SerializeField]
        string questTitle;

        [SerializeField]
        List<QuestObjective> objectives = new List<QuestObjective>();

        [SerializeField]
        List<QuestReward> rewards = new List<QuestReward>();
        
        [System.Serializable]
        class QuestReward
        {
            [SerializeField] int stackNumber;
            [SerializeField] InventoryItem item;

            public int GetHowMany()
            {
                return stackNumber;
            }

            public InventoryItem GetItemReward()
            {
                return item;
            }
        }

        [System.Serializable]
        public class QuestObjective
        {
            [SerializeField] string description;
            [SerializeField] string reference;

            public string GetDescription()
            {
                return description;
            }

            public string GetReference()
            {
                return reference;
            }
        }

        public IEnumerable<QuestObjective> GetObjectives()
        {
            return objectives;
        }

        public string GetQuestTitle()
        {
            return questTitle;
        }

        public bool HasObjective(string objective)
        {
            return objectives.Select(obj => obj.GetReference()).Contains(objective);
        }

        public static Quest GetByName(string questName)
        {
            return Resources.LoadAll<Quest>("").First(quest => quest.name == questName);
        }
    }
}
