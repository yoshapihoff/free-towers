using UnityEngine;
using System;

namespace Towers
{
    public class ShootingTower : Tower
    {
        #region PUBLIC VARIABLES

        public Action OnTurnedToEnemy;
        public Action OnEnemyOutOfSight;
        public Action OnMagazineRechargeStarted;
        public Action OnMagazineRechargeFininshed;

        public Transform ShootingPoint;
        public GameObject Bullet;
        public float TimeForTurnToEnemy;
        public string RechargeStateName;

        #endregion

        #region PRIVATE VARIEBLES

        float TurningTime;
        int RechargeCommandHsh = Animator.StringToHash("Recharge");
        AnimatorInStateChecker RechargeStateChecker;

        #endregion

        #region MONOBEHAVIOUR HANDLERS

        protected override void Awake()
        {
            base.Awake();
            RechargeStateChecker = AnimatorInStateChecker.CreateStateChecker(Animator, RechargeStateName, OnMagazineRechargeStarted, OnMagazineRechargeFininshed);
            OnEnemy += new Action(ResetTurningTime);
            OnEnemyLost += new Action(ResetTurningTime);
        }

        protected override void Update()
        {
            base.Update();
            TurnToEnemy();
        }

        #endregion

        #region PROTECTED METHODS

        protected virtual void RechargeMagazine()
        {
            RunRechargeAnimation();
        }

        #endregion

        #region PRIVATE METHODS

        private void RunRechargeAnimation()
        {
            Animator.SetTrigger(RechargeCommandHsh);
        }

        private void TurnToEnemy()
        {
            if (EnemyUnderAttack == null)
            {
                return;
            }
            var directionVector = EnemyUnderAttack.transform.position - ShootingPoint.transform.position;
            var lookRotation = Quaternion.LookRotation(directionVector);
            TurningTime += Time.deltaTime / TimeForTurnToEnemy;
            ShootingPoint.transform.rotation = Quaternion.Lerp(ShootingPoint.transform.rotation, lookRotation, TurningTime);

            if (TurningTime >= 0.24f)
            {
                ResetTurningTime();
                if (OnTurnedToEnemy != null)
                {
                    OnTurnedToEnemy();
                }
            }
            else
            {
                if (OnEnemyOutOfSight != null)
                {
                    OnEnemyOutOfSight();
                }
            }
        }

        private void ResetTurningTime()
        {
            TurningTime = 0f;
        }

        #endregion
    }
}