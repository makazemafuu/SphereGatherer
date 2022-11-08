using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public MovePlayer MyController;
    public Transform PrefabProjectile;
    public float ProjectileStartSpeed = 50;
    public float OffsetForwardShoot = 2;
    public float TimeBetweenShots = 0.5f;
    private float TimeShoot = 0;

    // Update is called once per frame
    void Update()
    {
        TimeShoot -= Time.deltaTime;

        if (MyController.WantsToShoot && TimeShoot <= 0)
        {
            TimeShoot = TimeBetweenShots;

            //Création du projetctile au bon endroit
            Transform proj = GameObject.Instantiate<Transform>(PrefabProjectile,
                transform.position + transform.forward * OffsetForwardShoot, transform.rotation);
            //Ajout d une impulsion de départ
            proj.GetComponent<Rigidbody>().AddForce(transform.forward * ProjectileStartSpeed, ForceMode.Impulse);
        }
    }
}