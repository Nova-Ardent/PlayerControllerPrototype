using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;

public partial class Localized
{
    public enum Title
    {
        X_Colon
    }

    public enum CharacterEditor
    {
        CharacterEditor,
        Gender,
        EyeColor,
        SkinTone,
        Hair,
        FacialHair,
        HairColor,
    }

    public enum GenderSelection
    {
        Male,
        Female,
    }

    public enum SaveCancel
    {
        Cancel,
        Save,
    }
}

public class CharacterEditor : MonoBehaviour
{
    public enum LeftOrRight
    {
        Left,
        Right,
    }

    [System.Serializable]
    public class Selector<T>
    {
        public Image backing;
        public TextMeshProUGUI title;
        public TextMeshProUGUI selection;
        public Image colorSplotch;
        public Button right;
        public Button left;
        public T[] options;
        [NonSerialized] public int selected;

        [NonSerialized] Action<int, T> _indexAction;
        public Action<int, T> indexUpdate
        {
            get => _indexAction;
            set
            {
                _indexAction = value;
                indexUpdate(selected, options[selected]);
            }
        }

        public void UpdateRight()
        {
            if (options == null)
            {
                return;
            }

            if (options.Length == 0)
            {
                return;
            }

            if (selected < options.Length - 1)
            {
                selected++;
                left.interactable = true;

                if (selected == options.Length - 1)
                {
                    right.interactable = false;
                }
                indexUpdate(selected, options[selected]);
            }
        }

        public void UpdateLeft()
        {
            if (options == null)
            {
                return;
            }

            if (options.Length == 0)
            {
                return;
            }

            if (selected > 0)
            {
                selected--;
                right.interactable = true;

                if (selected == 0)
                {
                    left.interactable = false;
                }
                indexUpdate(selected, options[selected]);
            }
        }

        public void SetButtonCalls()
        {
            right.onClick.AddListener(UpdateRight);
            left.onClick.AddListener(UpdateLeft);
        }
    }

    [System.Serializable]
    public class ButtonAction
    {
        public TextMeshProUGUI title;
        public Button button;
    }

    [Header("selectors")]
    public TextMeshProUGUI pageTitle;
    public Selector<Localized.GenderSelection> gender;
    public Selector<Color> eyeColor;
    public Selector<Color> skinTone;
    public Selector<int> hair;
    public Selector<int> facialHair;
    public Selector<Color> hairColor;

    public ButtonAction cancel;
    public ButtonAction save;

    public Localized.GenderSelection currentGender;

    [Header("data")]
    public PersonEditable person;

    private void Start()
    {
        pageTitle.text = Localized.CharacterEditor.CharacterEditor.Localize();
        gender.title.text = Localized.Title.X_Colon.Localize(Localized.CharacterEditor.Gender.Localize());
        eyeColor.title.text = Localized.Title.X_Colon.Localize(Localized.CharacterEditor.EyeColor.Localize());
        skinTone.title.text = Localized.Title.X_Colon.Localize(Localized.CharacterEditor.SkinTone.Localize());
        hair.title.text = Localized.Title.X_Colon.Localize(Localized.CharacterEditor.Hair.Localize());
        facialHair.title.text = Localized.Title.X_Colon.Localize(Localized.CharacterEditor.FacialHair.Localize());
        hairColor.title.text = Localized.Title.X_Colon.Localize(Localized.CharacterEditor.HairColor.Localize());

        gender.indexUpdate = (i, v) =>
        {
            gender.selection.text = v.Localize();
            person.gender = v;
        };

        eyeColor.indexUpdate = (i, v) =>
        {
            eyeColor.colorSplotch.color = v;
            person.eyeColor = v;
        };

        skinTone.indexUpdate = (i, v) =>
        {
            skinTone.colorSplotch.color = v;
            person.skinTone = v;
        };

        hair.indexUpdate = (i, v) =>
        {
            hair.selection.text = i.ToString();
            person.hair = v;
        };

        facialHair.indexUpdate = (i, v) =>
        {
            facialHair.selection.text = i.ToString();
            person.facialHair = v;
        };

        hairColor.indexUpdate = (i, v) =>
        {
            hairColor.colorSplotch.color = v;
            person.hairColor = v;
        };

        gender.SetButtonCalls();
        eyeColor.SetButtonCalls();
        skinTone.SetButtonCalls();
        hair.SetButtonCalls();
        facialHair.SetButtonCalls();
        hairColor.SetButtonCalls();

        cancel.button.onClick.AddListener(BackOut);
        cancel.title.text = Localized.SaveCancel.Cancel.Localize();

        save.button.onClick.AddListener(SaveAndBackOut);
        save.title.text = Localized.SaveCancel.Save.Localize();
    }

    public void BackOut()
    {

    }

    public void SaveAndBackOut()
    {

    }
}
