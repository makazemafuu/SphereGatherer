using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reverbation : MonoBehaviour
{
    public Vector3 centreZone = Vector3.zero;
    public float radiusStartEffect = 10;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.TransformPoint(centreZone), radiusStartEffect);
    }
}