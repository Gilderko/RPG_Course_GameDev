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
        [NonSerialized] GUIStyle nodeStyle;
        [NonSerialized] DialogueNode dragginNode = null;
        [NonSerialized] Vector2 mouseDragOffset = new Vector2();

        [NonSerialized] DialogueNode creatingNode = null; // Used as a signal for creating a new node 
        [NonSerialized] DialogueNode deletingNode = null; // Used as a signal for deleting a new node 
        [NonSerialized] DialogueNode linkingParentNode = null; // -||- linking node

        [NonSerialized] bool draggingCanvas = false;
        [NonSerialized] Vector2 draggingCanvasOffset;

        Texture2D backgroundTexture;
        Vector2 scrollPosition;
        const int canvasWidth = 4000;
        const int canvasHeight = 4000;
        const int backroundSize = 50;

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
            backgroundTexture = Resources.Load("background") as Texture2D;
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

                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition); // Only for auto layed out components

                Rect canvas = GUILayoutUtility.GetRect(canvasWidth, canvasHeight);                
                
                Rect textureCoords = new Rect(0, 0, canvasWidth/backroundSize, canvasHeight / backroundSize); // Allows to scale background texture
                GUI.DrawTextureWithTexCoords(canvas, backgroundTexture, textureCoords); // Draws the background

                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawNode(node);                    
                }
                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawConnections(node);
                }
                EditorGUILayout.EndScrollView();

                if (creatingNode != null)
                {
                    Undo.RecordObject(selectedDialogue, "Added Dialogue Node");
                    selectedDialogue.CreateNode(creatingNode);
                    creatingNode = null;
                }
                if (deletingNode != null)
                {
                    Undo.RecordObject(selectedDialogue, "Removed Dialogue Node");
                    selectedDialogue.DeleteNode(deletingNode);
                    deletingNode = null;
                }
            }            
        }

        private void ProcessEvents() // Used for Draging nodes around
        {
            EditorGUI.BeginChangeCheck();
            if (Event.current.type == EventType.MouseDown && (dragginNode == null))
            {
                dragginNode = GetOverMouseNode(Event.current.mousePosition + scrollPosition);
                if (dragginNode != null)
                {
                    mouseDragOffset = Event.current.mousePosition - dragginNode.inEditorPosition.position;
                }
                else
                {
                    draggingCanvas = true;
                    draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
                }
            }
            else if (Event.current.type == EventType.MouseDrag && dragginNode != null)
            {
                Undo.RecordObject(selectedDialogue, "Move Dialog");
                dragginNode.inEditorPosition.position = Event.current.mousePosition - mouseDragOffset;
                GUI.changed = true; // This Triggers OnGUI 
            }
            else if (Event.current.type == EventType.MouseDrag && draggingCanvas)
            {
                scrollPosition = draggingCanvasOffset - Event.current.mousePosition;
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseUp && dragginNode != null)
            {
                dragginNode = null;                                
            }
            else if (Event.current.type == EventType.MouseUp && draggingCanvas)
            {
                draggingCanvas = false;
            }
        }

        private void DrawNode(DialogueNode currentNode)
        {
            GUILayout.BeginArea(currentNode.inEditorPosition, nodeStyle); // All the field bellow will go inside the area, not auto layed out
            EditorGUI.BeginChangeCheck();

            string newText = EditorGUILayout.TextField(currentNode.text); // OnGUI gets called twice, first return text inside and second changes it

            if (EditorGUI.EndChangeCheck()) // Return true if something changed what is encompassed but BeginChangeCheck
            {
                Undo.RecordObject(selectedDialogue, "Update Dialogue Text");
                currentNode.text = newText;
            }

            GUILayout.BeginHorizontal(); // Start area which is layed Horizontaly
            if (GUILayout.Button("Create Node"))
            {
                creatingNode = currentNode;
            }            
            if (GUILayout.Button("Delete Node"))
            {
                deletingNode = currentNode;
            }
            GUILayout.EndHorizontal();
            DrawLinkButton(currentNode);

            GUILayout.EndArea();
        }

        private void DrawLinkButton(DialogueNode currentNode)
        {
            if (linkingParentNode == null)
            {
                if (GUILayout.Button("Link Node"))
                {
                    linkingParentNode = currentNode;
                }
            }
            else if (linkingParentNode == currentNode)
            {
                if (GUILayout.Button("Cancel Linking"))
                {
                    linkingParentNode = null;
                }
            }
            else if (linkingParentNode.dialogueChildren.Contains(currentNode.uniqueID))
            {
                if (GUILayout.Button("Remove Child"))
                {
                    Undo.RecordObject(selectedDialogue, "Add Dialogue Link");
                    linkingParentNode.dialogueChildren.Remove(currentNode.uniqueID);
                    linkingParentNode = null;
                }
            }
            else
            {
                if (GUILayout.Button("Add Child"))
                {
                    Undo.RecordObject(selectedDialogue, "Add Dialogue Link");
                    linkingParentNode.dialogueChildren.Add(currentNode.uniqueID);
                    linkingParentNode = null;
                }
            }
        }

        private void DrawConnections(DialogueNode parentNode)
        {
            Vector3 startPosition = new Vector2(parentNode.inEditorPosition.xMax,parentNode.inEditorPosition.center.y);
            foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(parentNode))
            {
                Vector3 endPosition = new Vector2(childNode.inEditorPosition.xMin, childNode.inEditorPosition.center.y);
                Vector3 tangetOffset = new Vector2(100, 0);

                Handles.DrawBezier(startPosition,endPosition,startPosition + tangetOffset, endPosition - tangetOffset,Color.magenta,null,4f);
            }
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

