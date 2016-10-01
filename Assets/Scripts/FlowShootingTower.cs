using UnityEngine;
using System;

namespace Towers
{
    public class FlowShootingTower : ShootingTower
    {
        #region PUBLIC VARIABLES

        public Action OnStartAttack;
        public Action OnFinishAttack;

        public float RechargeTime;
        public float AttackTime;

        #endregion

        #region PRIVATE VARIABLES

        float MagazineSize = 1f;
        float InnerRechargeTime;
        bool Attacking;
        bool Recharging;

        #endregion

        #region MONOBEHAVIOUR HANDLERS

        protected override void Awake()
        {
            base.Awake();
            OnTurnedToEnemy += new Action(Attack);
            OnEnemyLost += new Action(StopAttck);
        }

        protected override void Update()
        {
            base.Update();
            if (Attacking)
            {
                MagazineSize -= Time.deltaTime / AttackTime;
                if (MagazineSize <= 0f)
                {
                    Recharge();
                }
            }
            if (Recharging)
            {
                InnerRechargeTime -= Time.deltaTime;
                if (InnerRechargeTime <= 0f)
                {
                    Recharging = false;
                    MagazineSize = 1f;

                    if (OnMagazineRechargeFininshed != null)
                    {
                        OnMagazineRechargeFininshed();
                    }
                }
            }
        }

        #endregion

        #region PRIVATE METHODS

        private void Attack()
        {
            Recharging = false;
            Attacking = true;

            if (OnStartAttack != null)
            {
                OnStartAttack();
            }
        }

        private void StopAttck()
        {
            Attacking = false;

            if (OnFinishAttack != null)
            {
                OnFinishAttack();
            }
        }

        private void Recharge()
        {
            InnerRechargeTime = RechargeTime;
            Recharging = true;
            Attacking = false;

            if (OnMagazineRechargeStarted != null)
            {
                OnMagazineRechargeStarted();
            }
        }

        #endregion
    }
}