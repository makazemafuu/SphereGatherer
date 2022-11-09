using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSpeed : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] bool CanMove = true;

    private float timeSpan = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (CanMove)
        {
            timeSpan += Time.deltaTime * 2;
            transform.position -= transform.right * (speed / 10) * timeSpan;
        }
    }
    public void stop()
    {
        CanMove = false;
    }
}
