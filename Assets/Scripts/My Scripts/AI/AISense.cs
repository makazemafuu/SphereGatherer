using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class AISense<Stimulus> : MonoBehaviour
{
    public enum Status
    {
        Enter,
        Stay,
        Leave
    };

    public Transform SenseTransform;

    public float updateInterval = 0.3f;
    private float updateTime = 0.0f;

    public bool ShowDebug = true;
    protected List<Transform> trackedObjects = new List<Transform>();
    protected List<Transform> sensedObjects = new List<Transform>();

    public delegate void SenseEventHandler(Stimulus sti, Status sta);
    private event SenseEventHandler CallSenseEvent;


    //On appelle de manière recurrente la gestion du sens.
    //Si perception, on lance un event
    void Update()
    {
        Stimulus stimulus;
        Status sta = Status.Stay;

        updateTime += Time.deltaTime;
        if (updateTime > updateInterval)
        {
            resetSense();

            foreach (Transform t in trackedObjects)
            {
                stimulus = default(Stimulus);

                if (doSense(t, ref stimulus))
                {
                    sta = Status.Stay;
                    if (!sensedObjects.Contains(t))
                    {
                        sensedObjects.Add(t);
                        sta = Status.Enter;
                    }
                    CallSenseEvent(stimulus, sta);
                }
                else
                {
                    if (sensedObjects.Contains(t))
                    {
                        sta = Status.Leave;
                        CallSenseEvent(stimulus, sta);
                        sensedObjects.Remove(t);
                    }
                }
            }
            updateTime = 0;
        }
    }

    //A redefinir pour créer un nouveau sens
    //Détermine si on a percu quelquechose, et dans ce cas, en donne les paramètres
    protected abstract bool doSense(Transform obj, ref Stimulus sti);

    //Appelée juste avant de checker tous les objets
    //Peut etre redéfinie
    protected virtual void resetSense()
    {

    }

    //A appeler depuis n'importe quel script qui veut s'abonner à cet event
    public void AddSenseHandler(SenseEventHandler handler)
    {
        CallSenseEvent += handler;
    }


    //A appeler pour ajouter un objet à tracker
    public void AddObjectToTrack(Transform t)
    {
        trackedObjects.Add(t);
    }

    public void OnDrawGizmos()
    {
        if (!ShowDebug)
            return;

        Gizmos.color = Color.red;
        foreach (Transform t in sensedObjects)
        {
            Gizmos.DrawLine(SenseTransform.position, t.position);
        }
    }
}