using RPG.Quests;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RPG.Core
{
    [System.Serializable]
    public class Condition 
    {
        [SerializeField]
        PredicateTypes predicate = PredicateTypes.None;

        [SerializeField]
        List<string> paramaters = new List<string>();

        public bool Check(IEnumerable<IPredicateEvaluator> predicateEvaluators)
        {
            foreach (var evaluator in predicateEvaluators)
            {
                bool? result = evaluator.Evaluate(predicate, paramaters.ToList());
                if (!result.HasValue)
                {
                    continue;
                }

                bool resultValue = result.Value;

                if (!resultValue)
                {
                    return resultValue;
                }
            }
            return true;
        }

        public PredicateTypes GetPredicateName()
        {
            return predicate;
        }

        public void SetHasQuest(string questName)
        {
            paramaters = new List<string> { questName };
        }
        
        public void SetHasInventoryItem(string itemID)
        {
            paramaters = new List<string> { itemID };
        }

        public void SetCompletedQuest(string questName)
        {
            paramaters = new List<string> { questName };            
        }

        public void SetPredicateName(PredicateTypes name)
        {
            predicate = name;
        }

        public List<string> GetParameteres()
        {
            return paramaters;
        }
    }
}
