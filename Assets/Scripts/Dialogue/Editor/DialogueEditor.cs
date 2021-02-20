﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        Dialogue selectedDialogue = null;

        [MenuItem("Window/Dialogue Editor")] // Annotation Callback in editor
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }

        [OnOpenAsset(1)] // This Function should be called when we try to open any asset and the number defines the order
        public static bool OnOpenAsset(int instanceID, int line) 
        {
            Dialogue caller = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
            if (caller != null)
            {
                ShowEditorWindow();
                return true;
            }
            return false;
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnChangeDialogue;
        }

        /*private void OnDisable()
        {
            Selection.selectionChanged -= OnChangeDialogue;
        }*/

        private void OnChangeDialogue()
        {
            Dialogue selectedObject = Selection.activeObject as Dialogue;
            if (selectedObject != null)
            {
                Debug.Log("New Dialogue");
                selectedDialogue = selectedObject;
                Repaint();
            }            
        }

        private void OnGUI() // Hapens while over a GUI triggered by clicking for example (Name callback)
        {
            if (selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No dialogue selected.");
            }
            else
            {
                EditorGUILayout.LabelField(selectedDialogue.name);
            }
        }
    }
}

