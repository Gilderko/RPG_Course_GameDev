using GameDevTV.Inventories;
using RPG.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
    [CreateAssetMenu(menuName =("RPG/Inventory/Equipable Item"))]
    public class StatsEquipableItem : EquipableItem, IModifierProvider
    {
        [SerializeField]
        protected Modifier[] additiveModifiers;

        [SerializeField]
        protected Modifier[] percentageModifiers;

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            Debug.Log("Calling into EquipableItem");
            foreach(Modifier addMod in additiveModifiers)
            {
                if (addMod.stat == stat)
                {
                    yield return addMod.value;
                }
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            Debug.Log("Calling into EquipableItem");
            foreach (Modifier percMod in percentageModifiers)
            {
                if (percMod.stat == stat)
                {
                    yield return percMod.value;
                }
            }
        }

        [System.Serializable]
        protected struct Modifier
        {
            public Stat stat;
            public float value;
        }        
    }
}


