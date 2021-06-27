﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public interface IPredicateEvaluator 
    {
        bool? Evaluate(PredicateTypes predicate, List<string> parameters);
    }
}
