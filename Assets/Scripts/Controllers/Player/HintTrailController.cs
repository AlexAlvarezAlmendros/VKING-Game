using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HintTrailController : MonoBehaviour
{
    private GameObject target;
    private NavMeshAgent navMeshAgent;

    private Vector3 destination;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void DoMovement()
    {
        //Set destination
        destination = target.transform.position;

        //:: MOVE TRAIL ::
        navMeshAgent.destination = destination;
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    void Update()
    {
        DoMovement();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "NextLevel")
        {
            Destroy(gameObject);
        }
    }
}
