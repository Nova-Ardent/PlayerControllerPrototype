using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Animations;


namespace Creatures
{
    public class CreatureAnimator
    {
        public enum State
        {
            None,
            Idle,
            Walk,
            Jump,
            Falling,
        }

        public static readonly string STATE_NAME = "State";
        public static readonly int STATE_ID = Animator.StringToHash(STATE_NAME);

        public static readonly int IDLE_ID = 0;
        public static readonly int WALK_ID = 1;
        public static readonly int JUMP_ID = 2;
        public static readonly int MOVING_JUMP_ID = 3;
        public static readonly int FALLING_ID = 4;

        private AnimatorControllerParameter[] animatorControllerParameter;
        private Animator animator;

        public Animator Animator
        {
            get
            {
                return animator;
            }
        }

        State currentState;
        public State CurrentState
        {
            get
            {
                return currentState;
            }
        }

        int currentAnimation;
        public int CurrentAnimation
        {
            get
            {
                return currentAnimation;
            }
        }

        public AnimatorControllerParameter[] AnimatorControllerParameters
        {
            get
            {
                return animatorControllerParameter;
            }
        }

        public CreatureAnimator(Animator animator)
        {
            this.animator = animator;
            this.animatorControllerParameter = this.animator.parameters;
        }

        public void SetSpeed(float speed)
        {
            animator.SetFloat("Speed", speed);
        }

        public void SetState(State state, int animation)
        {
            if (currentState != state)
            {
                currentState = state;
                currentAnimation = animation;
                animator.SetInteger(STATE_ID, animation);
            }
        }

        public void SetStateWalking()
        {
            SetState(State.Walk, WALK_ID);
        }

        public void SetStateIdle()
        {
            SetState(State.Idle, IDLE_ID);
        }

        public void SetStateJump()
        {
            SetState(State.Jump, JUMP_ID);
        }

        public void SetStateMovingJump()
        {
            SetState(State.Jump, MOVING_JUMP_ID);
        }
        
        public void SetStateFalling()
        {
            SetState(State.Falling, FALLING_ID);
        }
    }
}
