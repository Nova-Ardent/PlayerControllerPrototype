using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Objects.Interactable;
using System.Linq;
using Creatures.Lockable;

namespace Objects.UI
{
    public class CalloutController : ILockable
    {
        private readonly GameObject calloutBase;
        private readonly ButtonCallout[] callouts;

        private readonly Transform calloutTransformBase;
        private readonly Transform target;
        private readonly Transform camera;

        private InteractableObject lockedBy;

        public bool IsLocked
        {
            get => lockedBy != null;
        }

        public CalloutController(GameObject calloutBase, ButtonCallout[] callouts, Transform target, Transform camera)
        {
            this.calloutBase = calloutBase;
            this.calloutTransformBase = calloutBase.transform;
            this.callouts = callouts;
            this.target = target;
            this.camera = camera;

            ClearCallouts();
        }

        public void Update()
        {
            if (target != null)
            {
                this.calloutTransformBase.position = target.position;
                this.calloutTransformBase.LookAt(camera);
            }
        }

        public void SetCallouts(params Interaction[] interactions)
        {
            SetCallouts(interactions.Select<Interaction, (string, Sprite)>(x => (x.actionString, x.callout)).ToArray());
        }

        public void SetCallouts(params (string, Sprite)[] callouts)
        {
            for (int i = 0; i < callouts.Length; i++)
            {
                if (i < callouts.Length)
                {
                    var callout = this.callouts[i];
                    callout.gameObject.SetActive(true);
                    callout.SetText(callouts[i].Item1);
                    callout.SetCallout(callouts[i].Item2);
                }
                else
                {
                    var callout = this.callouts[i];
                    callout.gameObject.SetActive(false);
                }
            }
        }

        public void ClearCallouts()
        {
            for (int i = 0; i < callouts.Length; i++)
            {
                var callout = this.callouts[i];
                callout.gameObject.SetActive(false);
            }
        }

        public void UnclearCallouts()
        {
            for (int i = 0; i < callouts.Length; i++)
            {
                var callout = this.callouts[i];
                callout.gameObject.SetActive(true);
            }
        }

        public void Unlock(InteractableObject unlockedBy)
        {
            if (unlockedBy == this.lockedBy)
            {
                this.lockedBy = null;
                UnclearCallouts();
            }
        }

        public void Lock(InteractableObject lockBy)
        {
            if (lockBy is ICanControlCallouts)
            {
                this.lockedBy = lockBy;
                ClearCallouts();
            }
        }
    }
}