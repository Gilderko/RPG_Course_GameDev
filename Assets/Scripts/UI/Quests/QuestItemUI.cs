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

        Quest currentQuest;

        public void Setup(Quest quest)
        {
            currentQuest = quest;
            title.text = quest.GetQuestTitle();
            progress.text = $"Objectives completed 0/{quest.GetObjectives().Count()}";
        }

        public Quest GetCurrentQuest()
        {
            return currentQuest;
        }
    }
}
