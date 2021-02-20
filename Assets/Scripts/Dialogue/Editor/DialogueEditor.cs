using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        [MenuItem("Window/Dialogue Editor")] // Callback in editor
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }

        [OnOpenAssetAttribute(1)] // This Function should be called when we try to open any asset and the number defines the order
        public static bool OnOpenAsset(int instanceID, int line) 
        {
            Dialogue caller = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
            if (caller == null) { return false; }

            ShowEditorWindow();
            return true;
        }

        private void OnGUI() // Hapens while over a GUI triggered by clicking for example
        {
            EditorGUILayout.LabelField("Hello World");
            EditorGUILayout.LabelField("Hello there");
            EditorGUILayout.LabelField("Hello ou");
            EditorGUILayout.LabelField("Hello");
        }

    }
}

