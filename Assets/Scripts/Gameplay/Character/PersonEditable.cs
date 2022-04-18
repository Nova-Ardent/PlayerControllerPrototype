using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonEditable : CharacterBase
{
    [SerializeField] Localized.GenderSelection _gender;
    public Localized.GenderSelection gender
    {
        get => _gender;
        set
        {
            _gender = value;
            SetGender(value);
        }
    }

    [SerializeField] Color _eyeColor;
    public Color eyeColor
    {
        get => _eyeColor;
        set
        {
            _eyeColor = value;
            SetEyeColor(value);
        }
    }

    [SerializeField] Color _skinTone;
    public Color skinTone
    {
        get => _skinTone;
        set
        {
            _skinTone = value;
            SetSkinTone(value);
        }
    }

    [SerializeField] int _hair;
    public int hair
    {
        get => _hair;
        set
        {
            _hair = value;
            SetHair(_hair);
        }
    }

    [SerializeField] int _facialHair;
    public int facialHair
    {
        get => _facialHair;
        set
        {
            _facialHair = value;
            SetHairColor(_hairColor);
        }
    }

    [SerializeField] Color _hairColor;
    public Color hairColor
    {
        get => _hairColor;
        set
        {
            _hairColor = value;
            SetHairColor(_hairColor);
        }
    }

    [SerializeField] GameObject eyeBrows;
    [SerializeField] GameObject eyes;
    [SerializeField] GameObject male;
    [SerializeField] GameObject currentHair;
    [SerializeField] GameObject currentBeard;
    [SerializeField] GameObject female;
    [SerializeField] Animator animator;

    [Header("equippables stuff")]
    [SerializeField] Equippables equippables;
    [SerializeField] Transform armature;

    Material eyeMaterial;
    Renderer eyeRenderer;

    Material maleMaterial;
    Renderer maleRenderer;

    Material femaleMaterial;
    Renderer femaleRenderer;

    Material eyeBrowsMaterial;
    Renderer eyeBrowsRenderer;

    Material hairMaterial;
    Renderer hairRenderer;

    [Header("Idle")]
    bool _isIdle;
    public bool isIdle
    {
        get => _isIdle;
        set
        {
            animator.SetBool("Idle", value);
            _isIdle = value;
        } 
    }

    [SerializeField] float minArmsCrossedDuration;
    [SerializeField] float maxArmsCrossedDuration;

    private void Start()
    {
        if (male == null || female == null || animator == null)
        {
            Debug.LogError("missing components to Person GO.");
        }

        if (eyes.TryGetComponent<Renderer>(out eyeRenderer))
        {
            eyeMaterial = eyeRenderer.material;
        }
        else
        {
            Debug.LogError("eyes missing renderer");
        }

        if (male.TryGetComponent<Renderer>(out maleRenderer))
        {
            maleMaterial = maleRenderer.material;
        }
        else
        {
            Debug.LogError("male missing renderer");
        }

        if (female.TryGetComponent<Renderer>(out femaleRenderer))
        {
            femaleMaterial = femaleRenderer.material;
        }
        else
        {
            Debug.LogError("female missing renderer");
        }

        if (eyeBrows.TryGetComponent<Renderer>(out eyeBrowsRenderer))
        {
            eyeBrowsMaterial = eyeBrowsRenderer.material;
        }
        else
        {
            Debug.LogError("eyebrows msising renderer");
        }

        SetSkinTone(_skinTone);
        SetGender(_gender);
        SetEyeColor(_eyeColor);
        SetHairColor(_hairColor);
    }

    private void Update()
    {
    }

    public void SetGender(Localized.GenderSelection gender)
    {
        if (gender == Localized.GenderSelection.Male)
        {
            male.SetActive(true);
            female.SetActive(false);
        }
        else
        {
            male.SetActive(false);
            female.SetActive(true);
        }
    }

    public void SetEyeColor(Color color)
    {
        if (eyeMaterial != null)
        {
            eyeMaterial.SetColor("_Red", color);
        }
    }

    public void SetSkinTone(Color color)
    {
        if (maleMaterial != null)
        {
            maleMaterial.SetColor("_Color", color);
        }
        if (femaleMaterial != null)
        {
            femaleMaterial.SetColor("_Color", color);
        }
    }

    public void SetHair(int i)
    {
        var equippable = equippables.GetHairStyles((Equippables.HairStyles)i);

        if (equippable != null)
        {
            Destroy(currentHair);

            var equippableInstance = Instantiate(equippable);
            equippableInstance.transform.SetParent(this.transform);

            if (equippableInstance != null && equippableInstance.TryGetComponent(out ArmatureReassign ar))
            {
                ar.newArmature = armature;
                ar.Reassign();
            }

            currentHair = equippableInstance;
            if (currentHair.transform.childCount > 0 && currentHair.transform.GetChild(0).TryGetComponent(out hairRenderer))
            {
                hairMaterial = hairRenderer.material;
            }
            else
            {
                hairRenderer = null;
                hairMaterial = null;
            }

            SetHairColor(_hairColor);
        }
    }

    public void SetHairColor(Color color)
    {
        if (eyeBrowsMaterial != null)
        {
            eyeBrowsMaterial.SetColor("_Color", color);
        }

        if (hairMaterial != null)
        {
            hairMaterial.SetColor("_Color", color);
        }

        /*if (currentHair != null && currentHair.TryGetComponent<Renderer>(out Renderer hairRenderer))
        {
            hairRenderer.material.SetColor("_Color", color);
        }

        if (currentBeard != null && currentHair.TryGetComponent<Renderer>(out Renderer beardRenderer))
        {
            beardRenderer.material.SetColor("_Color", color);
        }*/
    }
}
