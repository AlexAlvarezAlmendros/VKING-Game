using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimalRange : MonoBehaviour
{
    public SensePlayer sensePlayer;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            sensePlayer.npcMovement = false;
            sensePlayer.navMeshAgent.isStopped = true;
            
            Debug.Log(other.gameObject.name + " enters MINIMAL range");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            sensePlayer.npcMovement = true;
            sensePlayer.navMeshAgent.isStopped = false;
            Debug.Log(other.gameObject.name + " exists MINIMAL range");
        }
    }
}
