using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        Dialogue selectedDialogue = null;
        GUIStyle nodeStyle;
        
        DialogueNode dragginNode = null;
        Vector2 mouseDragOffset = new Vector2();

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
            OnChangeDialogue();

            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            nodeStyle.padding = new RectOffset(15, 25, 10, 20);
            nodeStyle.border = new RectOffset(12, 12, 12, 12);
        }

        private void OnDisable()
        {
            Selection.selectionChanged -= OnChangeDialogue;
        }

        private void OnChangeDialogue()
        {
            Dialogue selectedObject = Selection.activeObject as Dialogue;
            if (selectedObject != null)
            {                
                selectedDialogue = selectedObject;
                Repaint();
            }            
        }

        private void OnGUI() // Hapens while over a GUI triggered by clicking for example (Name callback)
        {
            Debug.Log("on gui");            
            if (selectedDialogue == null)
            {
                EditorGUILayout.TextField("No dialogue selected.");
            }
            else
            {
                ProcessEvents();
                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    OnGUINode(node);
                }
            }            
        }

        private void ProcessEvents()
        {
            EditorGUI.BeginChangeCheck();            
            if (Event.current.type == EventType.MouseDown && (dragginNode == null))
            {
                dragginNode = GetOverMouseNode(Event.current.mousePosition); 
                if (dragginNode != null)
                {
                    mouseDragOffset = Event.current.mousePosition - dragginNode.inEditorPosition.position;
                }
            }
            else if (Event.current.type == EventType.MouseDrag && dragginNode != null)
            {
                Undo.RecordObject(selectedDialogue, "Move Dialog");
                dragginNode.inEditorPosition.position = Event.current.mousePosition - mouseDragOffset;
                GUI.changed = true; // This Triggers OnGUI 
            }
            else if (Event.current.type == EventType.MouseUp && dragginNode != null)
            {
                dragginNode = null;                                
            }            
        }

        private void OnGUINode(DialogueNode node)
        {
            GUILayout.BeginArea(node.inEditorPosition,nodeStyle); // All the field bellow will go inside the area
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField("Node:",EditorStyles.whiteLabel);
            string newText = EditorGUILayout.TextField(node.text); // OnGUI gets called twice, first return text inside and second changes it
            string newID = EditorGUILayout.TextField(node.uniqueID);

            if (EditorGUI.EndChangeCheck()) // Return true if something changed what is encompassed but BeginChangeCheck
            {
                Undo.RecordObject(selectedDialogue, "Update Dialogue Text");
                node.text = newText;
                node.uniqueID = newID;
            }
            GUILayout.EndArea();
        }

        public DialogueNode GetOverMouseNode(Vector2 mousePosition)
        {
            DialogueNode foundNode = null;
            foreach (DialogueNode dialogueNode in selectedDialogue.GetAllNodes())
            {
                if (dialogueNode.inEditorPosition.Contains(mousePosition))
                {
                    foundNode = dialogueNode;
                }
            }
            return foundNode;
        }
    }
}

