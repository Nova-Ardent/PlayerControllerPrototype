using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace Objects.UI
{
    public class ButtonCallout : MonoBehaviour
    {
        [SerializeField] TMP_Text text;
        [SerializeField] Image image;

        public void SetText(string text)
        {
            this.text.text = text;
        }

        public void SetCallout(Sprite sprite)
        {
            this.image.sprite = sprite;
        }
    }
}