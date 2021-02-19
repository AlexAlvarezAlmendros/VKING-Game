using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PowerUpTrail : MonoBehaviour
{
    public GameObject target;
    private NavMeshAgent navMeshAgent;

    private Vector3 destination;
    public GameObject shieldFX;

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
        if(target.gameObject.tag == "Boss")
        {
            DoMovement();
        }
        else if(target.GetComponent<NavMeshAgent>().isActiveAndEnabled)
        {
            DoMovement();
        } else
        {
            Destroy(gameObject);
        }
        
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("I'am: " + gameObject.name + " and I touch: " + other.gameObject.name);
        if (other.tag == "Boss")
        {
            GameObject objectShieldFX = Instantiate(shieldFX, other.transform.position, other.transform.rotation, other.transform);
            objectShieldFX.transform.localPosition += Vector3.up*3;
            target.GetComponentInChildren<SHIELDCounterController>().AddShieldPoint();
            Destroy(objectShieldFX, 4f);
            Destroy(gameObject);
        } else if (other.gameObject == target)
        {
            GameObject objectShieldFX = Instantiate(shieldFX, other.transform.position, other.transform.rotation, other.transform);
            objectShieldFX.transform.localPosition += Vector3.up;
            target.GetComponentInChildren<SHIELDCounterController>().AddShieldPoint();
            Destroy(objectShieldFX, 4f);
            Destroy(gameObject);
        }
    }
}
