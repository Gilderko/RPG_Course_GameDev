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
        Disjunction[] AndDisjunctions;
        public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
        {
            foreach (Disjunction dis in AndDisjunctions)
            {
                if (!dis.Check(evaluators))
                {
                    return false;
                }
            }
            return true;
        }

        [System.Serializable]
        class Disjunction
        {
            [SerializeField]
            MyPredicate[] OrPredicates;

            public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach (var pred in OrPredicates)
                {
                    if (pred.Check(evaluators))
                    {
                        return true;
                    }
                }
                return false;
            }
        }


        [System.Serializable]
        public class MyPredicate
        {
            public PredicateTypes predicate = PredicateTypes.None;
            public List<string> paramaters = new List<string>();
            public bool shouldNegate = false;

            public bool Check(IEnumerable<IPredicateEvaluator> predicateEvaluators)
            {
                foreach (var evaluator in predicateEvaluators)
                {
                    bool? result = evaluator.Evaluate(predicate, paramaters.ToList());
                    if (!result.HasValue)
                    {
                        continue;
                    }

                    if (result == shouldNegate) return false;
                }
                return true;
            }
        }
        
    }
}
