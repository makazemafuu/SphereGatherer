using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    public bool ShowDebug = true;
    public float _Speed;
    public bool _HasATarget;

    [SerializeField] protected NavMeshAgent _NavMeshAgent;

    public virtual void Start()
    {
        _NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void FindPathTo(Vector3 dest)
    {
        _NavMeshAgent.SetDestination(dest);
    }

    public void Stop(bool _IsStoped)
    {
        _NavMeshAgent.isStopped = _IsStoped;
    }

    public void OnDrawGizmos()
    {
        if (!ShowDebug)
            return;

        float height = GetComponent<NavMeshAgent>().height;
        if (GetComponent<NavMeshAgent>().hasPath)
        {
            Vector3[] corners = GetComponent<NavMeshAgent>().path.corners;
            if (corners.Length >= 2)
            {
                Gizmos.color = Color.red;
                for (int i = 1; i < corners.Length; i++)
                {
                    Gizmos.DrawLine(corners[i - 1] + Vector3.up * height / 2, corners[i] + Vector3.up * height / 2);
                }
            }
        }

        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -0.5f, 0));
    }
}