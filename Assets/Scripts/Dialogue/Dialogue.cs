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
        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }

        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }
    }
}

