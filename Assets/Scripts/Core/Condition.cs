using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    [System.Serializable]
    public class Condition 
    {
        [SerializeField]
        string predicate;

        [SerializeField]
        List<string> paramaters;

        public bool Check(IEnumerable<IPredicateEvaluator> predicateEvaluators)
        {
            foreach (var evaluator in predicateEvaluators)
            {
                bool? result = evaluator.Evaluate(predicate, paramaters);
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
    }
}
