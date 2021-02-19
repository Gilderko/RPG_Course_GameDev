using GameDevTV.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Stats;

namespace RPG.Inventories
{
    public class RandomDropper : ItemDropper
    {
        [Tooltip("Scatter radius")]
        [SerializeField] float scatterRadius = 1;
        [SerializeField] DropLibrary dropOptions;

        const int ATTEMPTS = 30;

        protected override Vector3 GetDropLocation()
        {
            for (int i = 0; i < ATTEMPTS; i++)
            {
                Vector3 randomPoint = transform.position + Random.insideUnitSphere * scatterRadius;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 0.1f, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }
            return transform.position;
        }

        public void RandomDrop()
        {
            BaseStats baseStats = GetComponent<BaseStats>();
            IEnumerable<DropLibrary.Dropped> drops = dropOptions.GetRandomDrops(baseStats.GetLevel());
            foreach (DropLibrary.Dropped drop in drops)
            {
                DropItem(drop.item, drop.number);
            }
        }
    }
}


