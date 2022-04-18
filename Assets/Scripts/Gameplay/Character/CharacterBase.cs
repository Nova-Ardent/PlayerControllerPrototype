using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Network;

public class CharacterBase : MonoBehaviour
{
    bool _isIdle;
    public bool isIdle
    {
        get => _isIdle;
        set
        {
            if (_isIdle == value)
            {
                return;
            }

            _isIdle = value;
            animator.SetBool("idle", value);
        }
    }

    bool _isTraveling;
    public bool isTraveling
    {
        get => _isTraveling;
        private set
        {
            if (_isTraveling == value)
            {
                return;
            }

            _isTraveling = value;
            animator.SetBool("traveling", value);
        }
    }

    bool _isRunning;
    public bool isRunning
    {
        get => _isRunning;
        set
        {
            if (_isRunning == value)
            {
                return;
            }

            _isRunning = value;
            animator.SetBool("running", value);
        }
    }

    [SerializeField] Animator animator;

    void Start()
    {
    }

    void Update()
    {
    }
}
