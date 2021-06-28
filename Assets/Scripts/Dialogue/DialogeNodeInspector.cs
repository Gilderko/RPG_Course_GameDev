using GameDevTV.Inventories;
using RPG.Core;
using RPG.Quests;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{    
    [CustomEditor(typeof(DialogueNode))]    
    [ExecuteInEditMode]
    public class DialogeNodeInspector : Editor
    { 
        public override void OnInspectorGUI()
        {
            Debug.Log("running");
            var targetNode = (DialogueNode) target;

            targetNode.SetPlayerSpeaking(EditorGUILayout.Toggle("IsPlayerSpeaking",targetNode.IsPlayerSpeaking()));
            targetNode.SetText(EditorGUILayout.TextField("Dialogue Text", targetNode.GetText()));

            targetNode.SetPosition(EditorGUILayout.RectField("Rect", targetNode.GetRect()).position);            

            targetNode.SetEnterAction(EditorGUILayout.TextField("Enter Action", targetNode.GetOnEnterAction()));
            targetNode.SetExitAction(EditorGUILayout.TextField("Exit Action", targetNode.GetOnExitAction()));

            var condition = targetNode.GetCondition();

            condition.SetPredicateName((PredicateTypes) EditorGUILayout.EnumPopup("Predicate", targetNode.GetCondition().GetPredicateName()));
            
            var predicate = condition.GetPredicateName();
            if (predicate == PredicateTypes.HasQuest || predicate == PredicateTypes.CompletedQuest)
            {
                var questNames = Resources.LoadAll<Quest>("").ToList();
                var condParameters = condition.GetParameteres();

                int selectedIndex = 0;
                if (condParameters.Count != 0)
                {
                    selectedIndex = questNames.Select(quest => quest.name).ToList().IndexOf(condition.GetParameteres()[0]);
                    selectedIndex = selectedIndex == -1 ? 0 : selectedIndex;
                }

                var selectedOption = (Quest) EditorGUILayout.ObjectField("Quest", questNames[selectedIndex], typeof(Quest), false);
                condition.SetHasQuest(selectedOption.name);
                EditorUtility.SetDirty(targetNode);
            }
            
            if (predicate == PredicateTypes.HasInventoryItem)
            {
                var items = Resources.LoadAll<InventoryItem>("").ToList();
                var itemIDs = items.Select(item => item.GetItemID()).ToList();
                var condParameters = condition.GetParameteres();

                int selectedIndex = 0;
                if (condParameters.Count != 0)
                {
                    selectedIndex = itemIDs.IndexOf(condition.GetParameteres()[0]);
                    selectedIndex = selectedIndex == -1 ? 0 : selectedIndex;
                }

                EditorGUILayout.LabelField("Item name:", items[selectedIndex].GetDisplayName());
                var selectedItem = (InventoryItem) EditorGUILayout.ObjectField("Item required:", items[selectedIndex], typeof(InventoryItem), false);                
                condition.SetHasInventoryItem(selectedItem.GetItemID());
                EditorUtility.SetDirty(targetNode);
            }            
        }
    }
}
