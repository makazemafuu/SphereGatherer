using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmolCube : MonoBehaviour
{
    public Transform PetitCubeAsset;
    public Vector3 OffsetCreation;

    public void CreatePetitCube()
    {
        GameObject.Instantiate(PetitCubeAsset, transform.position + OffsetCreation, Quaternion.identity);
    }

    //Pour savoir ou on crée notre cube en fonction de l'offset, debug visuel
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + OffsetCreation, 0.1f);
    }
}