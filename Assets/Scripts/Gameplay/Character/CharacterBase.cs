using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Utilities;

public class CharacterBase : MonoBehaviour
{
    [System.Serializable]
    public struct CharacterData
    {
        public string name;
    }

    [System.Serializable]
    public struct CharacterWorldData
    {
        [SerializeField] public Collider characterController;
        [SerializeField] public GameObject personBase;
        [SerializeField] public float angularRotation;
        [SerializeField] public float fallingSpeed;
    }

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

    [SerializeField] protected Animator animator;
    [SerializeField] GameObject lockOn; 
    [SerializeField] protected CharacterData characterData;

    [SerializeField] protected CharacterWorldData characterWorldData;

    public void FaceDirection(Vector2 direction)
    {
        FaceDirection(Vector2.SignedAngle(direction, Vector2.down));
    }

    public void FaceDirection(float angle)
    {
        characterWorldData.personBase.transform.localRotation = 
            Quaternion.RotateTowards(characterWorldData.personBase.transform.localRotation, Quaternion.Euler(0, angle, 0), UnityEngine.Time.deltaTime  * characterWorldData.angularRotation);
    }

    public void MoveDirection(Vector2 direction, bool running = false)
    {
        MoveDirection(Vector2.SignedAngle(direction, Vector2.down));
    }

    public void MoveDirection(float angle, bool running = false)
    {
        FaceDirection(angle);
        isIdle = false;
        isTraveling = true;
        isRunning = running;

        //characterWorldData.characterController.(new Vector3(Mathf.Sin(angle.DegreesToRads()), -characterWorldData.fallingSpeed, Mathf.Cos(angle.DegreesToRads())));
    }


    public void Idle()
    {
        isIdle = true;
        isTraveling = false;
        isRunning = false;
    }
}
