using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpTest : MonoBehaviour
{
    public float left = -5;
    public float right = 5;

    [Range(0, 1)]
    public float lerp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(left + (right - left) * lerp, 0, 0);
    }
}
