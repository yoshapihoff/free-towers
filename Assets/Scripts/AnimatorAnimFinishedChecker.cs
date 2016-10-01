using UnityEngine;
using System;

namespace Towers
{
    public class AnimatorInStateChecker : ScriptableObject
    {
        public Action OnComeToState;
        public Action OnOutFromState;

        public bool InState
        {
            get;
            private set;
        }

        bool Error;
        string PreviousStateName = string.Empty;
        string CurrentStateName;
        Animator Animator;
        string StateName;
        int Layer;

        public void Update()
        {
            if (!Error)
            {
                if (!Animator ||
                    string.IsNullOrEmpty(StateName) ||
                    Layer < 0 ||
                    Layer > Animator.layerCount - 1)
                {
                    Debug.LogError("Параметры заданы неверно");
                    Error = true;
                    return;
                }

                InState = Animator.GetCurrentAnimatorStateInfo(Layer).IsName(StateName);

                if (InState && StateName != PreviousStateName)
                {
                    PreviousStateName = StateName;
                    if (OnComeToState != null)
                    {
                        OnComeToState();
                    }
                    return;
                }

                if (!InState)
                {
                    if (OnOutFromState != null)
                    {
                        OnOutFromState();
                    }
                    return;
                }
            }
        }

        public static AnimatorInStateChecker CreateStateChecker(Animator animator,
            string stateName,
            Action onComeToState,
            Action OnOutFormState,
            int layer = 0)
        {
            var stateChecker = CreateInstance<AnimatorInStateChecker>();

            stateChecker.Layer = layer;
            stateChecker.Animator = animator;
            stateChecker.StateName = stateName;
            stateChecker.OnComeToState += () =>
            {
                if (onComeToState != null)
                {
                    onComeToState();
                }
            };
            stateChecker.OnOutFromState += () =>
            {
                if (OnOutFormState != null)
                {
                    OnOutFormState();
                }
            };
            return stateChecker;
        }
    }
}