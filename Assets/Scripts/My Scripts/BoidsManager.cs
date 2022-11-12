using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class BoidsManager : MonoBehaviour
{
    private static BoidsManager instance = null;
    public static BoidsManager sharedInstance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<BoidsManager>();
            }
            return instance;
        }
    }

    public Boids prefabBoid;
    public float nbBoids = 100;
    public float startSpeed = 1;
    public float startSpread = 10;

    public float maxDistBoids = 30;

    public float periodRetargetBoids = 6;
    public float periodNoTargetBoids = 3;
    private float timerRetargetBoids = 0;
    private bool setTargetToBoids = true;

    public GameObject target;

    private List<Boids> boids = new List<Boids>();
    public ReadOnlyCollection<Boids> roBoids
    {
        get { return new ReadOnlyCollection<Boids>(boids); }
    }

    void Start()
    {
        for (int i = 0; i < nbBoids; i++)
        {
            Boids b = GameObject.Instantiate<Boids>(prefabBoid);
            Vector3 positionBoid = Random.insideUnitSphere * startSpread + transform.position;
            positionBoid.y = Mathf.Abs(positionBoid.y); //Ne pas créer des oiseaux sous 0, on imagine que ce sera le sol.
            b.player = target;
            b.transform.position = positionBoid;
            b.velocity = (positionBoid - transform.position).normalized * startSpeed;
            b.transform.parent = this.transform;
            b.maxSpeed *= Random.Range(0.95f, 1.05f);
            boids.Add(b);
        }
    }

    void Update()
    {
        //Décrémente la temporisation
        timerRetargetBoids -= Time.deltaTime;
        if (timerRetargetBoids <= 0)
        {
            if (!setTargetToBoids)
                timerRetargetBoids = periodNoTargetBoids;
            else
                timerRetargetBoids = periodRetargetBoids;

            Vector3 target = Random.insideUnitSphere * maxDistBoids;
            target.y = Mathf.Max(Mathf.Abs(target.y), 10);
            foreach (Boids b in boids)
            {
                b.goToTarget = false;
                if (setTargetToBoids && Random.Range(0.0f, 1.0f) < 0.5f)
                {
                    b.target = target;
                    b.goToTarget = true;
                }
            }

            setTargetToBoids = !setTargetToBoids;
        }
    }
}