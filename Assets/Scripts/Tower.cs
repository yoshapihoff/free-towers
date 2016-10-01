using UnityEngine;
using System;

namespace Towers
{
    public class Tower : MonoBehaviour
    {
        #region PUBLIC VARIABLES

        public Action OnBuildStarted;
        public Action OnBuildFinished;
        public Action OnUpgrade;
        public Action OnHitPointsLowerThanQuarter;
        public Action OnDestroyStarted;
        public Action OnDestroyFinished;
        public Action OnEnemy;
        public Action OnEnemyLost;

        public int ConstructionPrice;
        public int MaxHitPoints;
        public TowerUpgrade[] AvailableUpgrades;
        public float AttackRange;
        public EnemyFinder Finder;
        public string BuildStateName;
        public string DestroyStateName;

        public int Level
        {
            get;
            protected set;
        }

        #endregion

        #region PROTECTED VARIABLES

        protected Animator Animator;
        protected Enemy EnemyUnderAttack
        {
            get
            {
                if (EnemiesInAttackRange.Length == 0)
                {
                    return null;
                }
                else
                {
                    return EnemiesInAttackRange[0];
                }
            }
        }

        #endregion

        #region PRIVATE VARIABLES

        Enemy[] EnemiesInAttackRange;
        int BuildCommandHash = Animator.StringToHash("Build");
        int DestroyCommandHsh = Animator.StringToHash("Destroy");
        
        AnimatorInStateChecker BuildStateChecker;
        AnimatorInStateChecker DestroyStateChecker;

        int _HitPoints;
        int HitPoints
        {
            get
            {
                return _HitPoints;
            }
            set
            {
                _HitPoints = value;
                if (_HitPoints < MaxHitPoints / 4)
                {
                    if (OnHitPointsLowerThanQuarter != null)
                    {
                        OnHitPointsLowerThanQuarter();
                    }
                }
            }
        }

        #endregion

        #region MONOBEHAVIOUR HANDLERS

        protected virtual void Awake()
        {
            EnemiesInAttackRange = new Enemy[0];

            Animator = GetComponentInChildren<Animator>();

            BuildStateChecker = AnimatorInStateChecker.CreateStateChecker(Animator, BuildStateName, OnBuildStarted, OnBuildFinished);
            DestroyStateChecker = AnimatorInStateChecker.CreateStateChecker(Animator, DestroyStateName, OnDestroyStarted, OnDestroyFinished);

            OnDestroyFinished += () =>
            {
                Destroy(gameObject);
            };

            Finder.OnEnemyFound += (Enemy enemy) =>
            {
                AddEnemy(enemy);
            };
            Finder.OnEnemyLost += (Enemy enemy) =>
            {
                RemoveEnemy(enemy);
            };

            SetAttackRange(AttackRange);
        }

        protected virtual void Update()
        {
            BuildStateChecker.Update();
            DestroyStateChecker.Update();
        }

        #endregion

        #region PROTECTED METHODS

        protected virtual void Build()
        {
            RunBuildAnimation();
        }

        protected virtual void Upgrade()
        {
            ++Level;
            // TODO: Предложить выбор исходя из доступных апгрейтов
            // TODO: Запомнить выбор
            RunDestroyAnimation();
            if (OnUpgrade != null)
            {
                OnUpgrade();
            }
            // TODO: Создать объект следующей башни
            // TODO: Запустить постройку новой башни
        }

        protected virtual void Destroy()
        {
            // TODO: Указать в событии, какой враг последним ударил по башне 
            RunDestroyAnimation();
        }

        #endregion

        #region PRIVATE METHODS

        private void RunBuildAnimation()
        {
            Animator.SetTrigger(BuildCommandHash);
        }

        private void RunDestroyAnimation()
        {
            Animator.SetTrigger(DestroyCommandHsh);
        }

        private bool EnemyExists(Enemy enemy)
        {
            for (int i = 0; i < EnemiesInAttackRange.Length; ++i)
            {
                if (enemy == EnemiesInAttackRange[i])
                {
                    return true;
                }
            }
            return false;
        }

        private void AddEnemy(Enemy enemy)
        {
            int length = EnemiesInAttackRange.Length;
            if (length == 0)
            {
                if (OnEnemy != null)
                {
                    OnEnemy();
                }
            }
            if (!EnemyExists(enemy))
            {
                Enemy[] newEnemiesInAttackRange = new Enemy[length + 1];
                for (int i = 0; i < length; ++i)
                {
                    newEnemiesInAttackRange[i] = EnemiesInAttackRange[i];
                }
                newEnemiesInAttackRange[length] = enemy;
                EnemiesInAttackRange = newEnemiesInAttackRange;
            }
        }

        private void RemoveEnemy(Enemy enemy)
        {
            int length = EnemiesInAttackRange.Length;
            if (length == 0)
            {
                if (OnEnemyLost != null)
                {
                    OnEnemyLost();
                }
                return;
            }
            if (EnemyExists(enemy))
            {
                Enemy[] newEnemiesInAttackRange = new Enemy[length - 1];
                int resultIndex = 0;
                for (int i = 0; i < length; ++i)
                {
                    var currentEnemy = EnemiesInAttackRange[i];
                    if (enemy != currentEnemy)
                    {
                        newEnemiesInAttackRange[resultIndex] = currentEnemy;
                        ++resultIndex;
                    }
                }
                EnemiesInAttackRange = newEnemiesInAttackRange;
            }
        }

        private void SetAttackRange(float newAttackRange)
        {
            AttackRange = newAttackRange;
            Finder.SetSearchRange(AttackRange);
        }
        #endregion
    }
}