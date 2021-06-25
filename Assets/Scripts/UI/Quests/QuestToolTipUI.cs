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

        public void Setup(Quest quest)
        {
            title.text = quest.GetQuestTitle();
            objectiveContainer.DetachChildren();
            foreach (var objective in quest.GetObjectives())
            {
                var objectiveUI = Instantiate(objectivePrefab, objectiveContainer);
                var objectiveText = objectiveUI.GetComponentInChildren<TextMeshProUGUI>();
                objectiveText.text = objective;
            }
        }
    }
}

