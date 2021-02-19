using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamageArea : MonoBehaviour
{
    public int missileDamage;
    public bool damagePlayer;
    
    void Start()
    {
        float radius = GetComponent<SphereCollider>().radius;
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var other in colliders)
        {
            if (damagePlayer)
            {
                if (other.gameObject.tag == "Player")
                {
                    other.gameObject.GetComponentInChildren<HPCounterController>().TakeMultipleDamage(missileDamage);
                }
                
            }            
            else if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Boss")
            {
                other.gameObject.GetComponentInChildren<HPCounterController>().TakeMultipleDamage(missileDamage);   
            }

            
        }
    }


}
