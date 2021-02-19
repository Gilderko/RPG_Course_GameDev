using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;
using RPG.Attributes;
using GameDevTV.Utils;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] float aggroCooldownTime = 3f;
        [SerializeField] float nearbyAggroDistance = 4f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 3f;
        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;

        Fighter fighter;
        Health health;
        Mover mover;
        GameObject player;

        LazyValue<Vector3> guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        float timeSinceAggraveted = Mathf.Infinity;
        int currentWaypointIndex = 0;
        bool inCombat = false;

        private void Awake() {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");

            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
        }

        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }

        private void Start() {
            guardPosition.ForceInit();
        }

        private void Update()
        {
            if (health.IsDead()) return;

            if (IsAggrevated() && fighter.CanAttack(player))
            {                
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();                
            }
            UpdateTimers();
        }

        public void Aggrevate()
        {
            if (!inCombat)
            {
                inCombat = true;
                timeSinceAggraveted = 0;
                AggrevateNearbyEnemies();
            }            
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeSinceAggraveted += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition.value;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);                
            }
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void SuspicionBehaviour()
        {
            inCombat = false;
            GetComponent<ActionScheduler>().CancelCurrentAction();            
        }

        private void AttackBehaviour()
        {   
            if (!inCombat)
            {
                AggrevateNearbyEnemies();
                inCombat = true;
            }
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);            
        }

        private void AggrevateNearbyEnemies()
        {
            RaycastHit[] nearbyHits = Physics.SphereCastAll(transform.position, nearbyAggroDistance, Vector3.up, 0);
            foreach (RaycastHit hit in nearbyHits)
            {
                print(hit.transform.name);
                AIController potentialEnemy = hit.collider.GetComponent<AIController>();
                if (potentialEnemy != null)
                {
                    print("Found enemy");
                    potentialEnemy.Aggrevate();
                }
            }
        }

        private bool IsAggrevated()
        {           
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            //print(distanceToPlayer);
            return distanceToPlayer < chaseDistance || timeSinceAggraveted < aggroCooldownTime;
        }

        // Called by Unity
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}