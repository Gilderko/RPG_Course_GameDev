using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "RPG/Dialogue")]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();
        Dictionary<string, DialogueNode> lookUpNodesCache = new Dictionary<string, DialogueNode>();

#if UNITY_EDITOR // Doesnt run while playing the game
        private void Awake() // When ScriptableObject is Loaded
        {
            if (nodes.Count == 0)
            {
                Debug.Log("Loading Dialogue");
                DialogueNode defaultNode = new DialogueNode();
                nodes.Add(defaultNode);
            }
        }
#endif
        private void OnValidate()
        {
            lookUpNodesCache.Clear();
            foreach (DialogueNode dialogueNode in GetAllNodes())
            {
                lookUpNodesCache[dialogueNode.uniqueID] = dialogueNode;
            }
        }

        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {
            foreach (string childID in parentNode.dialogueChildren)
            {
                if (!lookUpNodesCache.ContainsKey(childID)) { yield return null; }
                yield return lookUpNodesCache[childID];
            }
        }
    }
}

