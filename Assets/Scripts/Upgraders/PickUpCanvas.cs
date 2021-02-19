using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpCanvas : MonoBehaviour
{
    public GameObject itemSwap;


    public void AutoDestroy()
    {
        Destroy(gameObject);
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            itemSwap.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            itemSwap.SetActive(false);
        }
    }
}