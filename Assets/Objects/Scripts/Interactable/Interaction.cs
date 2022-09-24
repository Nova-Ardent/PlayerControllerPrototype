using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utilities.Controller;
using Utilities.Localization;

namespace Objects.Interactable
{
    public class Interaction
    {
        public Controller.Controls control;
        public Enum action;
        public string actionString => action.Localize();
        public Sprite callout;
    }
}
