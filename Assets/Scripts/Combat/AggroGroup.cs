using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class AggroGroup : MonoBehaviour
    {
        [SerializeField]
        List<Fighter> fighters;
        [SerializeField]
        bool onStartStatus = false;

        private void Start()
        {
            Activate(onStartStatus);
        }

        public void Activate(bool activateStatus)
        {
            foreach(var fighter in fighters)
            {
                var target = fighter.GetComponent<CombatTarget>();
                if (target != null)
                {
                    target.enabled = activateStatus;
                }
                fighter.enabled = activateStatus;
            }
        }
    }
}
