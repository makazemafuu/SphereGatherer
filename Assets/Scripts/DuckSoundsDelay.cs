using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckSoundsDelay : MonoBehaviour
{
    public float MinSoundDelay = 1;
    public float RandomDelay = 5;
    private float NextSoundDelay = 0;
    private AudioSource aSource;

    private void computeNextSoundDelay()
    {
        NextSoundDelay = MinSoundDelay + Random.Range(0, RandomDelay);
    }

    void Start()
    {
        aSource = GetComponent<AudioSource>();
        computeNextSoundDelay();
    }

    void Update()
    {
        NextSoundDelay -= Time.deltaTime;
        if (NextSoundDelay <= 0)
        {
            aSource.Play();
            computeNextSoundDelay();
        }
    }
}