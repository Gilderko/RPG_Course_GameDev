using RPG.Quests;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestItemUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] TextMeshProUGUI progress;

        QuestStatus currentQuestStatus;      

        public void Setup(QuestStatus questStatus)
        {
            currentQuestStatus = questStatus;
            title.text = questStatus.GetQuest().GetQuestTitle();
            progress.text = $"Objectives completed {questStatus.GetCompletedCount()}/{questStatus.GetQuest().GetObjectives().Count()}";
        }

        public QuestStatus GetCurrentQuestStatus()
        {
            return currentQuestStatus;
        }
    }
}
