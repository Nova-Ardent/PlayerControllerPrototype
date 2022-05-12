using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DebugMenu
{
    public class DebugOptionAction : DebugOption
    {
        public Action onActivation { get; private set; }

        public DebugOptionAction(string name, string description, Action onActivation)
            : base(name, description)
        {
            this.onActivation = onActivation;
        }

        public DebugOptionAction(string name, Func<string> description, Action onActivation)
            : base(name, description)
        {
            this.onActivation = onActivation;
        }
    }

    public class DebugOptionActionUI : DebugOptionUI
    {
        DebugOptionAction optionAction;
        public Color onPress;

        public override void UpdateData()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (highlighted && Controller.GetKeyDown(Controller.Controls.DebugEnter))
            {
                backing.color = onPress;
                optionAction.onActivation();
            }
            else if (Controller.GetKeyUp(Controller.Controls.DebugEnter))
            {
                backing.color = highlighted ? backingHighlighted : backingRegular;
            }
#endif
            base.UpdateData();
        }

        public void ApplyData(DebugOptionAction debugOption)
        {
            optionAction = debugOption;
            base.ApplyData(debugOption);
        }
    }
}