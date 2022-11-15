using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SightStimulus
{
    public Vector3 position;
    public float accuracy;
}

public class AISenseSight : AISense<SightStimulus>
{
    public float SightAngle = 60;
    public float MaxDistance = 10f;

    public float ConeAccuracy = 0.5f;
    public float BoundsMargin = 0.2f; //On ne teste pas la largeur totale du mesh, on lui laisse une marge 

    private List<Vector3> checkedPoints = new List<Vector3>();

    protected override void resetSense()
    {
        base.resetSense();
        if (ShowDebug)
            checkedPoints.Clear();
    }

    private Bounds getObjectBounds(Transform t)
    {
        Renderer[] rends = t.GetComponentsInChildren<Renderer>();
        Bounds b = new Bounds();
        foreach (Renderer r in rends)
        {
            if (b.size.sqrMagnitude == 0)
                b = new Bounds(r.bounds.center, r.bounds.size);
            else
                b.Encapsulate(r.bounds);
        }
        return b;
    }

    private bool canSeePoint(Vector3 point, Transform parent)
    {
        Vector3 dirToPoint = point - SenseTransform.position;
        if (Vector3.Angle(SenseTransform.forward, dirToPoint) < SightAngle / 2.0f)
        {
            float distance = dirToPoint.magnitude;
            if (distance < MaxDistance)
            {
                RaycastHit hitInfo;
                if (Physics.Raycast(SenseTransform.position, dirToPoint, out hitInfo, distance))
                {
                    if (hitInfo.collider.transform.IsChildOf(parent) || hitInfo.collider.transform == parent)
                        return true;
                }
                else
                {
                    return true;
                }

            }
        }
        return false;
    }

    protected override bool doSense(Transform obj, ref SightStimulus sti)
    {
        //Si l'objet est trop loin, on ne fait rien
        //C'est un peu faux pour les gros objets mais c'est une optimisation utile pour ne pas checker précisément les objets lointains
        if ((obj.position - SenseTransform.position).sqrMagnitude > MaxDistance * MaxDistance)
            return false;

        //On récupère la taille de l'objet
        Bounds b = getObjectBounds(obj);
        float width = Mathf.Max(b.extents.x, b.extents.z) * (1 - BoundsMargin);
        float height = b.extents.y * (1 - BoundsMargin);

        //On teste 7 points, les coins et au centre
        Vector3[] pointsToCheck = {
            b.center + obj.right * width ,
            b.center - obj.right * width ,
            b.center + obj.right * width + obj.up * height,
            b.center - obj.right * width + obj.up * height,
            b.center + obj.right * width - obj.up * height,
            b.center - obj.right * width - obj.up * height,
            b.center
        };

        checkedPoints.Clear();
        foreach (Vector3 point in pointsToCheck)
        {
            if (ShowDebug)
                checkedPoints.Add(point);

            sti.position = obj.position;

            if (canSeePoint(point, obj))
            {
                return true;
            }
        }
        return false;
    }

    public new void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (!ShowDebug)
            return;

        Vector3 pointMax = new Vector3(0, 0, MaxDistance);
        Gizmos.color = Color.red;
        Vector3 posWorld = SenseTransform.TransformPoint(pointMax);
        Gizmos.DrawLine(SenseTransform.position, posWorld);
        pointMax = Quaternion.AngleAxis(SightAngle / 2, Vector3.right) * pointMax;
        float nbRays = 100 * ConeAccuracy;
        Quaternion rCone = Quaternion.AngleAxis(360 / nbRays, Vector3.forward);
        for (float i = 0; i < nbRays; i++)
        {
            pointMax = rCone * pointMax;
            posWorld = SenseTransform.TransformPoint(pointMax);
            Gizmos.color = Color.white;
            Gizmos.DrawLine(SenseTransform.position, posWorld);
        }

        //On affiche les points checkés
        foreach (Vector3 p in checkedPoints)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(SenseTransform.position, p);
        }
    }
}