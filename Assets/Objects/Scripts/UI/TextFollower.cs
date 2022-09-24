using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Objects.UI
{
    public class TextFollower : MonoBehaviour
    {
        [SerializeField] Transform follow;

        // Start is called before the first frame update
        void Start()
        {
            if (follow == null)
            {
                Debug.LogError($"{this.gameObject.name} is missing a follow target, destroying componenet");
                Destroy(this);
                return;
            }
        }

        private void Update()
        {
            transform.LookAt(follow);
        }
    }

}