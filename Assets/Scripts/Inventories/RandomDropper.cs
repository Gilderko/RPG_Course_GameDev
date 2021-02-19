using GameDevTV.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Inventories
{
    public class RandomDropper : ItemDropper
    {
        [Tooltip("Scatter radius")]
        [SerializeField] float scatterRadius = 1;
        [SerializeField] InventoryItem[] dropOptions;

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
            int numberOfDrops = Random.Range(1, 4);
            for (int i = 0; i < numberOfDrops; i++)
            {
                int choiceIndex = Random.Range(0, dropOptions.Length);
                InventoryItem item = dropOptions[choiceIndex];
                if (item.IsStackable())
                {
                    int number = Random.Range(1, 5);
                    DropItem(dropOptions[choiceIndex], number);
                }
                else
                {
                    DropItem(dropOptions[choiceIndex]);
                }               
            }           
        }
    }
}


