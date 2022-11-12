using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class CNAMSManager : MonoBehaviour
{
    private static CNAMSManager instance = null;
    public static CNAMSManager sharedInstance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<CNAMSManager>();
            }
            return instance;
        }
    }

    public CNAMS prefabCNAM;
    public float nbCNAMS = 100;
    public float startSpeed = 1;
    public float startSpread = 10;

    public float maxDistCNAMS = 30;

    public float periodRetargetCNAMS = 6;
    public float periodNoTargetCNAMS = 3;
    private float timerRetargetCNAMS = 0;
    private bool setTargetToCNAMS = true;

    public GameObject target;

    private List<CNAMS> cnams = new List<CNAMS>();
    public ReadOnlyCollection<CNAMS> roCNAMS
    {
        get { return new ReadOnlyCollection<CNAMS>(cnams); }
    }

    void Start()
    {
        for (int i = 0; i < nbCNAMS; i++)
        {
            CNAMS b = GameObject.Instantiate<CNAMS>(prefabCNAM);
            Vector3 positionCNAM = Random.insideUnitSphere * startSpread + transform.position;
            positionCNAM.y = Mathf.Abs(positionCNAM.y); //Ne pas créer des oiseaux sous 0, on imagine que ce sera le sol.
            b.player = target;
            b.transform.position = positionCNAM;
            b.velocity = (positionCNAM - transform.position).normalized * startSpeed;
            b.transform.parent = this.transform;
            b.maxSpeed *= Random.Range(0.95f, 1.05f);
            cnams.Add(b);
        }
    }

    void Update()
    {
        //Décrémente la temporisation
        timerRetargetCNAMS -= Time.deltaTime;
        if (timerRetargetCNAMS <= 0)
        {
            if (!setTargetToCNAMS)
                timerRetargetCNAMS = periodNoTargetCNAMS;
            else
                timerRetargetCNAMS = periodRetargetCNAMS;

            Vector3 target = Random.insideUnitSphere * maxDistCNAMS;
            target.y = Mathf.Max(Mathf.Abs(target.y), 10);
            foreach (CNAMS b in cnams)
            {
                b.goToTarget = false;
                if (setTargetToCNAMS && Random.Range(0.0f, 1.0f) < 0.5f)
                {
                    b.target = target;
                    b.goToTarget = true;
                }
            }

            setTargetToCNAMS = !setTargetToCNAMS;
        }
    }
}