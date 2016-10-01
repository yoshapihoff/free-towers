using UnityEngine;
using System;

namespace Towers
{
    public class Bullet : MonoBehaviour
    {
        #region PUBLIC VARIABLES

        public Action OnHit;

        public float DamageRange;
        public float Speed;
        public int DamageHits;
        public GameObject OnHitEffectPrefab;

        #endregion

        #region PROTECTED VARIABLES

        protected Vector3 StartPosition;
        protected Vector3 FinishPosition;

        #endregion

        #region PRIVATE VARIABLES

        float DistanceToEnemy;
        float LerpTime = 1f;
        float CurrentLerpTime = 0f;
        float FlightTime;
        SphereCollider DamageCollider;

        #endregion

        #region MONOBEHAVIOUR HANDLERS

        protected virtual void Awake ()
        {
            DamageCollider = GetComponentInChildren<SphereCollider>();
        }

        protected virtual void Update ()
        {
            CurrentLerpTime += Time.deltaTime;
            if (CurrentLerpTime > LerpTime)
            {
                CurrentLerpTime = LerpTime;
            }
            FlightTime = CurrentLerpTime / LerpTime * Speed;

            transform.position = Vector3.Lerp(StartPosition, FinishPosition, FlightTime);
        }

        #endregion

        #region PUBLIC METHODS

        public void Initialize(Vector3 startPosition, Vector3 finishPosition)
        {
            StartPosition = startPosition;
            FinishPosition = finishPosition;
            DamageCollider.radius = DamageRange;
        }

        #endregion

        #region PRIVATE VARIABLES

        #endregion

        #region PROTECTED METHODS

        #endregion
    }
}
