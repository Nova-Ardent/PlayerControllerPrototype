using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creatures
{
    public class RagdollController
    {
        private readonly Rigidbody[] ragdollRigidbodies;
        private readonly Collider[] ragdollColliders;
        private readonly Animator animator;
        private readonly Collider mainCollider;

        private bool isRagdollActive = false;
        public bool IsRagdollActive
        {
            get => isRagdollActive;
        }

        public RagdollController(Rigidbody[] ragdollRigidbodies, Collider[] ragdollColliders, Animator animator, Collider mainCollider)
        {
            this.ragdollRigidbodies = ragdollRigidbodies;
            this.ragdollColliders = ragdollColliders;
            this.animator = animator;
            this.mainCollider = mainCollider;

            Deactivate();
        }

        public void Activate(Vector3 activationSpeed)
        {
            isRagdollActive = true;

            this.animator.enabled = false;
            this.mainCollider.enabled = false;
            for (int i = 0; i < this.ragdollRigidbodies.Length; i++)
            {
                this.ragdollRigidbodies[i].isKinematic = false;
                this.ragdollRigidbodies[i].detectCollisions = true;
                this.ragdollRigidbodies[i].velocity = activationSpeed;
            }

            for (int i = 0; i < this.ragdollColliders.Length; i++)
            {
                this.ragdollColliders[i].enabled = true;
            }
        }

        public void Deactivate()
        {
            isRagdollActive = false;

            this.animator.enabled = true;
            this.mainCollider.enabled = true;
            for (int i = 0; i < this.ragdollRigidbodies.Length; i++)
            {
                this.ragdollRigidbodies[i].isKinematic = true;
                this.ragdollRigidbodies[i].detectCollisions = false;
            }

            for (int i = 0; i < this.ragdollColliders.Length; i++)
            {
                this.ragdollColliders[i].enabled = false;
            }
        }
    }
}