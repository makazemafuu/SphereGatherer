using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseGravity : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = new Vector3(0, -15, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

