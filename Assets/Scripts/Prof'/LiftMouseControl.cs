using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftMouseControl : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Up");
            animator.ResetTrigger("Down");
        }

        if (Input.GetButtonDown("Fire2"))
        {
            animator.ResetTrigger("Up");
            animator.SetTrigger("Down");
        }
    }
}