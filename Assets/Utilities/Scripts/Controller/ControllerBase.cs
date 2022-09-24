using System;
using System.Collections.Generic;
using UnityEngine;
using static Utilities.Controller.Controller;

namespace Utilities.Controller
{
    public abstract class ControllerBase
    {
        protected Dictionary<Controls, Func<bool>> downControls = new Dictionary<Controls, Func<bool>>();
        protected Dictionary<Controls, Func<bool>> upControls = new Dictionary<Controls, Func<bool>>();
        protected Dictionary<Controls, Func<bool>> heldControls = new Dictionary<Controls, Func<bool>>();
        protected Dictionary<Controls, Func<float>> axis = new Dictionary<Controls, Func<float>>();
        public abstract ControllerType controllerType { get; }

        public void SetupButtonAxis(InputAlias alias, Controls control)
        {
            var aliasValue = Utilities.GetAttribute<InputAliasAttribute>(alias)?.alias ?? "";
            if (!String.IsNullOrWhiteSpace(aliasValue))
            {
                SetupButtonAxis(aliasValue, control);
            }
        }

        public virtual void SetupButtonAxis(string key, Controls control)
        {
            axis[control] = () =>
            {
                if (currentControllerType == controllerType)
                {
                    return Input.GetAxis(key);
                }
                return 0;
            };
        }

        public virtual void SetupButtonAxis(KeyCode key, KeyCode key2, Controls control)
        {
            axis[control] = () =>
            {
                if (currentControllerType == controllerType)
                {
                    if (Input.GetKey(key))
                    {
                        return 1;
                    }
                    else if (Input.GetKey(key2))
                    {
                        return -1;
                    }
                }
                return 0;
            };
        }

        public virtual void SetupButtonAxis(KeyCode key, int pressValue, Controls control)
        {
            axis[control] = () =>
            {
                if (currentControllerType == controllerType && Input.GetKey(key))
                {
                    return pressValue;
                }
                return 0;
            };
        }

        public virtual void SetupButtonHeld(KeyCode key, Controls control)
        {
            heldControls[control] = () =>
            {
                if (currentControllerType == controllerType)
                {
                    return Input.GetKey(key);
                }
                return false;
            };
        }

        public virtual void SetupButtonDown(KeyCode key, Controls control)
        {
            downControls[control] = () =>
            {
                if (currentControllerType == controllerType)
                {
                    return Input.GetKeyDown(key);
                }
                return false;
            };
        }

        public virtual void SetupButtonUp(KeyCode key, Controls control)
        {
            upControls[control] = () =>
            {
                if (currentControllerType == controllerType)
                {
                    return Input.GetKeyUp(key);
                }
                return false;
            };
        }

        public virtual void SetupButtonHeld(string key, Controls control)
        {
            heldControls[control] = () =>
            {
                if (currentControllerType == controllerType)
                {
                    return Input.GetKey(key);
                }
                return false;
            };
        }

        public virtual void SetupButtonDown(string key, Controls control)
        {
            downControls[control] = () =>
            {
                if (currentControllerType == controllerType)
                {
                    return Input.GetKeyDown(key);
                }
                return false;
            };
        }

        public virtual void SetupButtonUp(string key, Controls control)
        {
            upControls[control] = () =>
            {
                if (currentControllerType == controllerType)
                {
                    return Input.GetKeyUp(key);
                }
                return false;
            };
        }

        public virtual void SetupButtonHeld(InputAlias key, Controls control)
        {
            var aliasValue = Utilities.GetAttribute<InputAliasAttribute>(key)?.alias ?? "";

            heldControls[control] = () =>
            {
                if (currentControllerType == controllerType)
                {
                    return Input.GetKey(aliasValue);
                }
                return false;
            };
        }

        public virtual void SetupButtonDown(InputAlias key, Controls control)
        {
            var aliasValue = Utilities.GetAttribute<InputAliasAttribute>(key)?.alias ?? "";

            downControls[control] = () =>
            {
                if (currentControllerType == controllerType)
                {
                    return Input.GetKeyDown(aliasValue);
                }
                return false;
            };
        }

        public virtual void SetupButtonUp(InputAlias key, Controls control)
        {
            var aliasValue = Utilities.GetAttribute<InputAliasAttribute>(key)?.alias ?? "";

            upControls[control] = () =>
            {
                if (currentControllerType == controllerType)
                {
                    return Input.GetKeyUp(aliasValue);
                }
                return false;
            };
        }

        public virtual void SetupMouseButtonHeld(int button, Controls control)
        {
            heldControls[control] = () =>
            {
                if (currentControllerType == controllerType)
                {
                    return Input.GetMouseButton(button);
                }
                return false;
            };
        }

        public virtual void SetupMouseButtonDown(int button, Controls control)
        {
            downControls[control] = () =>
            {
                if (currentControllerType == controllerType)
                {
                    return Input.GetMouseButtonDown(button);
                }
                return false;
            };
        }

        public virtual void SetupMouseButtonUp(int button, Controls control)
        {
            upControls[control] = () =>
            {
                if (currentControllerType == controllerType)
                {
                    return Input.GetMouseButtonUp(button);
                }
                return false;
            };
        }

        public bool GetKey(Controls control)
        {
            return heldControls.ContainsKey(control) ? heldControls[control].Invoke() : false;
        }

        public bool GetKeyUp(Controls control)
        {
            return upControls.ContainsKey(control) ? upControls[control].Invoke() : false;
        }

        public bool GetKeyDown(Controls control)
        {
            return downControls.ContainsKey(control) ? downControls[control].Invoke() : false;
        }

        public float GetAxis(Controls control)
        {
            return axis.ContainsKey(control) ? axis[control].Invoke() : 0.0f;
        }
    }
}