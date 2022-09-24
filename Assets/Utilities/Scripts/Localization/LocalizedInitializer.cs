using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities.Localization
{
    public class LocalizedInitializer : MonoBehaviour
    {
        private void Awake()
        {
#if UNITY_EDITOR
            Localized.Instance.ValidateAndCreateLanguages();
#endif
            Localized.Instance.SetLanguage(Localized.Languages.English_NA);
        }
    }
}