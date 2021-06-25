using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "RPG Project/QUest", order = 0)]
    public class Quest : ScriptableObject
    {
        [SerializeField]
        string questTitle;

        [SerializeField]
        List<string> objectives = new List<string>();        

        public IEnumerable<string> GetObjectives()
        {
            return objectives;
        }

        public string GetQuestTitle()
        {
            return questTitle;
        }

    }
}
