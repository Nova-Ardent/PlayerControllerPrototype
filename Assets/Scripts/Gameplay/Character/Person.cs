using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : CharacterBase
{
    public enum Gender
    {
        None,
        Male,
        Female,
    }

    [SerializeField] Gender _gender;
    public Gender gender
    {
        get => _gender;
        set
        {
            _gender = value;
            SetGender(value);
        }
    }

    [SerializeField] GameObject male;
    [SerializeField] GameObject female;
    [SerializeField] Animator animator;
    
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

        SetGender(_gender);
    }

    private void Update()
    {
    }

    void SetGender(Gender gender)
    {
        male.SetActive(false);
        female.SetActive(false);

        if (gender == Gender.Male)
        {
            male.SetActive(true);
        }
        else
        {
            female.SetActive(true);
        }
    }
}
