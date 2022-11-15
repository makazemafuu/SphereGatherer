using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CNAMS : MonoBehaviour
{
    public float avoidanceZone = 5;
    public float alignmentZone = 9;
    public float cohesionZone = 50;

    public float avoidanceForce = 15;
    public float alignmentForce = 3;
    public float cohesionForce = 20;

    public Vector3 target = new Vector3();
    public float targetForce = 9;
    public bool goToTarget = false;

    public Vector3 velocity = new Vector3();
    public float maxSpeed = 20;
    public float minSpeed = 10;

    public bool drawGizmos = true;
    public bool drawLines = true;
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        Vector3 sumForces = new Vector3();
        Color colorDebugForce = Color.black;
        float nbForcesApplied = 0;

        foreach (CNAMS otherCNAM in CNAMSManager.sharedInstance.roCNAMS)
        {
            Vector3 vecToOtherCNAM = otherCNAM.transform.position - transform.position;

            Vector3 forceToApply = new Vector3();

            //Si on doit prendre en compte cet autre boid (plus grande zone de perception)
            if (vecToOtherCNAM.sqrMagnitude < cohesionZone * cohesionZone)
            {
                //Si on est entre attraction et alignement
                if (vecToOtherCNAM.sqrMagnitude > alignmentZone * alignmentZone)
                {
                    //On est dans la zone d'attraction uniquement
                    forceToApply = vecToOtherCNAM.normalized * cohesionForce;
                    float distToOtherCNAM = vecToOtherCNAM.magnitude;
                    float normalizedDistanceToNextZone = ((distToOtherCNAM - alignmentZone) / (cohesionZone - alignmentZone));
                    float boostForce = (4 * normalizedDistanceToNextZone);
                    if (!goToTarget) //Encore plus de cohésion si pas de target
                        boostForce *= boostForce;
                    forceToApply = vecToOtherCNAM.normalized * cohesionForce * boostForce;
                    colorDebugForce += Color.green;
                }
                else
                {
                    //On est dans alignement, mais est on hors de répulsion ?
                    if (vecToOtherCNAM.sqrMagnitude > avoidanceZone * avoidanceZone)
                    {
                        //On est dans la zone d'alignement uniquement
                        forceToApply = otherCNAM.velocity.normalized * alignmentForce;
                        colorDebugForce += Color.blue;
                    }
                    else
                    {
                        //On est dans la zone de repulsion
                        float distToOtherCNAM = vecToOtherCNAM.magnitude;
                        float normalizedDistanceToPreviousZone = 1 - (distToOtherCNAM / avoidanceZone);
                        float boostForce = (4 * normalizedDistanceToPreviousZone);
                        forceToApply = vecToOtherCNAM.normalized * -1 * (avoidanceForce * boostForce);
                        colorDebugForce += Color.red;

                    }
                }

                sumForces += forceToApply;
                nbForcesApplied++;
            }
        }

        //On fait la moyenne des forces, ce qui nous rend indépendant du nombre de boids
        sumForces /= nbForcesApplied;

        target = player.GetComponent<Transform>().position;

        //Si on a une target, on l'ajoute
        if (goToTarget)
        {
            Vector3 vecToTarget = target - transform.position;
            if (vecToTarget.sqrMagnitude < 1)
                goToTarget = false;
            else
            {
                Vector3 forceToTarget = vecToTarget.normalized * targetForce;
                sumForces += forceToTarget;
                colorDebugForce += Color.magenta;
                nbForcesApplied++;
                if (drawLines)
                    Debug.DrawLine(transform.position, target, Color.magenta);
            }
        }

        //Debug
        if (drawLines)
            Debug.DrawLine(transform.position, transform.position + sumForces, colorDebugForce / nbForcesApplied);

        //On freine
        velocity += -velocity * 10 * Vector3.Angle(sumForces, velocity) / 180.0f * Time.deltaTime;

        //on applique les forces
        velocity += sumForces * Time.deltaTime;

        //On limite la vitesse
        if (velocity.sqrMagnitude > maxSpeed * maxSpeed)
            velocity = velocity.normalized * maxSpeed;
        if (velocity.sqrMagnitude < minSpeed * minSpeed)
            velocity = velocity.normalized * minSpeed;

        //On regarde dans la bonne direction        
        if (velocity.sqrMagnitude > 0)
            transform.LookAt(transform.position + velocity);

        //Debug
        if (drawLines)
            Debug.DrawLine(transform.position, transform.position + velocity, Color.blue);


        //Deplacement du boid
        transform.position += velocity * Time.deltaTime;

    }

    void OnDrawGizmosSelected()
    {
        if (drawGizmos)
        {
            // Répulsion
            Gizmos.color = new Color(1, 0, 0, 1.0f);
            Gizmos.DrawWireSphere(transform.position, avoidanceZone);
            // Alignement
            Gizmos.color = new Color(0, 1, 0, 1.0f);
            Gizmos.DrawWireSphere(transform.position, alignmentZone);
            // Attraction
            Gizmos.color = new Color(0, 0, 1, 1.0f);
            Gizmos.DrawWireSphere(transform.position, cohesionZone);
        }
    }
}