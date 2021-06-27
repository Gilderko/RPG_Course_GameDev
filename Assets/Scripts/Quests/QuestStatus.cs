using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Quests
{
    [System.Serializable]
    public class QuestStatus
    {
        [SerializeField] Quest quest;
        [SerializeField] List<string> completedObjectives = new List<string>();

        [System.Serializable]
        class QuestStatusRecord
        {
            string questName;
            List<string> completedObjectives = new List<string>();
            
            public QuestStatusRecord(string qName, List<string> qCompObj)
            {
                questName = qName;
                completedObjectives = qCompObj;
            }

            public string GetName()
            {
                return questName;
            }

            public List<string> GetCompletedObjectives()
            {
                return completedObjectives;
            }
        }

        public QuestStatus(Quest quest)
        {
            this.quest = quest;
        }

        public QuestStatus(object objectState)
        {
            var loadRecord = objectState as QuestStatusRecord;
            quest = Quest.GetByName(loadRecord.GetName());
            completedObjectives = loadRecord.GetCompletedObjectives();
        }

        public bool IsQuestComplete()
        {
            Debug.Log(quest.GetObjectives().Count());
            Debug.Log(completedObjectives.Count());
            return quest.GetObjectives().Count() == completedObjectives.Count();
        }

        public Quest GetQuest()
        {
            return quest;
        }

        public int GetCompletedCount()
        {
            return completedObjectives.Count;
        }

        public bool IsObjectiveComplete(string objective)
        {
            Debug.Log(objective);
            return completedObjectives.Contains(objective);
        }

        public void CompleteObjective(string objective)
        {
            if (quest.HasObjective(objective))
            {
                completedObjectives.Add(objective);
            }            
        }

        public object CaptureState()
        {
            var recordToSave = new QuestStatusRecord(quest.name,completedObjectives);
            return recordToSave;
        }
    }
}
