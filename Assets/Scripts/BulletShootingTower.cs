using UnityEngine;
using System;

namespace Towers
{
    public class BulletShootingTower : ShootingTower
    {
        #region PUBLIC VARIABLES

        public Action OnShoot;

        public float ShootingInterval;
        public int MagazineSize;
        public float RechargeTime;

        #endregion

        #region PRIVATE VARIABLES

        float LastShootTime;
        bool EnemyInSight;

        #endregion

        #region MONOBEHAVIOUR HANDLERS

        protected override void Awake()
        {
            base.Awake();

            OnTurnedToEnemy += () => EnemyInSight = true;
            OnEnemyOutOfSight += () => EnemyInSight = false;
        }

        protected override void Update()
        {
            base.Update();
            if (EnemyInSight)
            {
                Shoot();
            }
        }

        #endregion

        #region PRIVATE METHODS

        private void Shoot()
        {
            if (Time.time > LastShootTime + ShootingInterval)
            {
                LastShootTime = Time.time;

                Debug.Log(LastShootTime);
                Debug.Log(LastShootTime + ShootingInterval);

                GameObject bullet = Instantiate<GameObject>(this.Bullet);
                bullet.transform.parent = ShootingPoint;
                bullet.transform.localPosition = Vector3.zero;
                Bullet controller = bullet.GetComponent<Bullet>();
                controller.Initialize(ShootingPoint.position, EnemyUnderAttack.transform.position);

                if (OnShoot != null)
                {
                    OnShoot();
                }
            }
        }

        private void Recharge()
        {

            if (OnMagazineRechargeStarted != null)
            {
                OnMagazineRechargeStarted();
            }
        }

        #endregion
    }
}