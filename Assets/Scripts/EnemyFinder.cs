using UnityEngine;
using System;
using System.Collections;

namespace Towers
{
    public class EnemyFinder : MonoBehaviour
    {
        #region PUBLIC VARIABLES

        public Action<Enemy> OnEnemyFound;
        public Action<Enemy> OnEnemyLost;
        public SphereCollider Collider;
        #endregion

        #region PRIVATE VARIABLES

        Tower Tower;
        bool Search = false;

        #endregion

        #region MONOBEHAVIOUR HANDLERS

        void Awake()
        {
            this.Tower = GetComponentInParent<Tower>();

            this.Tower.OnBuildFinished += () => { Search = true; };
            this.Tower.OnDestroyStarted += () => { Search = false; };

            Search = true;
        }

        void OnTriggerStay(Collider other)
        {
            if (Search)
            {
                var enemyController = other.GetComponent<Enemy>();
                if (enemyController)
                {
                    if (OnEnemyFound != null)
                    {
                        OnEnemyFound(enemyController);
                    }
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            var enemyController = other.GetComponent<Enemy>();
            if (enemyController)
            {
                if (OnEnemyFound != null)
                {
                    OnEnemyLost(enemyController);
                }
            }
        }

        #endregion

        #region PUBLIC METHODS

        public void SetSearchRange (float attackRange)
        {
            Collider.radius = attackRange * 2f;
        }

        #endregion
    }
}