using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public PlayerMove MyController;
    public Transform PrefabProjectile;
    public float ProjectileStartSpeed = 50;
    public float OffsetForwardShoot = 2;

    // Update is called once per frame
    void Update()
    {
        if (MyController.WantsToShoot)
        {
            //Création du projetctile au bon endroit
            Transform proj = GameObject.Instantiate<Transform>(PrefabProjectile,
                transform.position + transform.forward * OffsetForwardShoot, transform.rotation);
            //Ajout d une impulsion de départ
            proj.GetComponent<Rigidbody>().AddForce(transform.forward * ProjectileStartSpeed, ForceMode.Impulse);
        }
    }
}
