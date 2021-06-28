using GameDevTV.Inventories;
using RPG.Quests;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static RPG.Core.Condition;

namespace RPG.Core
{    
    [CustomPropertyDrawer(typeof(MyPredicate))]    
    public class MyPredicateEditor : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 100;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {            
            EditorGUI.BeginProperty(position, label, property);
            EditorGUIUtility.labelWidth = 75;
            position = EditorGUI.PrefixLabel(position, label);

            Debug.Log(position.height);

            // Setup Predicate Type
            Rect predicateRect = new Rect(position.x, position.y + 20, position.width, 20f);

            SerializedProperty predicateName = property.FindPropertyRelative("predicate");
            
            EditorGUI.PropertyField(predicateRect, predicateName, new GUIContent("Type"), false);

            PredicateTypes predicateValue = (PredicateTypes)predicateName.enumValueIndex;
            
            // Setup Predicate Paramaters
            Rect parametersPosition = new Rect(position.x, position.y + 40, position.width, 20f);

            // Setup NonePredicate
            SerializedProperty paramaters = property.FindPropertyRelative("paramaters");
            if (predicateValue == PredicateTypes.None)
            {
                paramaters.ClearArray();
            }

            // Setup HasQuestPredicate, CompletedQuestPredicate
            if (predicateValue == PredicateTypes.HasQuest || predicateValue == PredicateTypes.CompletedQuest)
            {
                var questNames = Resources.LoadAll<Quest>("").ToList();                

                int selectedIndex = 0;
                if (paramaters.arraySize != 0)
                {
                    var firstQuest = paramaters.GetArrayElementAtIndex(0).GetValue() as string;
                    selectedIndex = questNames.Select(quest => quest.name).ToList().IndexOf(firstQuest);
                    selectedIndex = selectedIndex == -1 ? 0 : selectedIndex;    
                }

                var selectedOption = (Quest)EditorGUI.ObjectField(parametersPosition, "Quest" ,questNames[selectedIndex], typeof(Quest), false);
                if (selectedOption == null)
                {
                    return;
                }
                paramaters.SetValue(new List<string>() { selectedOption.name });
            }

            // Set HasItemPredicate
            if (predicateValue == PredicateTypes.HasInventoryItem)
            {
                var items = Resources.LoadAll<InventoryItem>("").ToList();             

                int selectedIndex = 0;
                if (paramaters.arraySize != 0)
                {
                    var firstItem = paramaters.GetArrayElementAtIndex(0).GetValue() as string;
                    selectedIndex = items.Select(item => item.GetItemID()).ToList().IndexOf(firstItem);
                    selectedIndex = selectedIndex == -1 ? 0 : selectedIndex;
                }

                var selectedItem = (InventoryItem)EditorGUI.ObjectField(parametersPosition, "Item", items[selectedIndex], typeof(InventoryItem), false);
                if (selectedItem == null)
                {
                    return;
                }
                paramaters.SetValue(new List<string>() { selectedItem.GetItemID() });              
            }            
            EditorGUI.EndProperty();
        }
    }
}
