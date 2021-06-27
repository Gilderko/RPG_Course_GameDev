using RPG.Quests;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
    public class QuestToolTipUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] Transform objectiveContainer;
        [SerializeField] GameObject objectivePrefab;
        [SerializeField] GameObject objectiveCompletePrefab;

        public void Setup(QuestStatus questStatus)
        {
            var quest = questStatus.GetQuest();
            title.text = questStatus.GetQuest().GetQuestTitle();
            objectiveContainer.DetachChildren();
            foreach (var objective in quest.GetObjectives())
            {
                print(objective);
                var objectiveUI = questStatus.IsObjectiveComplete(objective.GetReference()) ? Instantiate(objectiveCompletePrefab, objectiveContainer) :
                    Instantiate(objectivePrefab, objectiveContainer);
                var objectiveText = objectiveUI.GetComponentInChildren<TextMeshProUGUI>();
                objectiveText.text = objective.GetDescription();
            }
        }
    }
}

