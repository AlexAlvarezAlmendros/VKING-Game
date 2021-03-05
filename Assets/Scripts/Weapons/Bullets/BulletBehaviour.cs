﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public GameObject hitParticle;
    private void OnTriggerEnter(Collider other)
    {
        if (other.name != "BulletSpawn" && other.tag != "BulletPlayer" && other.tag != "Trigger")
        {
            //:: BULLETS ENEMIGAS ATRAVIESAN ENEMIGOS ::
            if(transform.tag == "BulletEnemy" && (other.tag=="Boss" || other.tag == "Enemy"))
            {
                return;
            }

            if(transform.tag == "BulletPlayer" && other.gameObject.tag == "BossFieldForce")
            {
                //:: DO HIT FX ::
                GameObject explosionF = Instantiate(hitParticle, transform.position, transform.rotation);
                Destroy(gameObject);
                Destroy(explosionF, 0.5f);
            }

            //:: DO HIT FX ::
            GameObject explosion = Instantiate(hitParticle, transform.position, transform.rotation);

            //Debug.Log("BulletBehaviour - other.tag: " + other.tag);
            //Debug.Log("BulletBehaviour - transform.tag: " + transform.tag);

            //:: DESTROY GAME OBJECTS ::
            if (other.tag == "Player" && transform.tag == "BulletEnemy")
            {
                StartCoroutine(other.GetComponentInChildren<HPCounterController>().ModifyHealthFire(100f));
            }

            if ((other.tag == "Boss" || other.tag == "Enemy" ) && transform.tag == "BulletPlayer")
            {
                StartCoroutine(other.GetComponentInChildren<HPCounterController>().ModifyHealthFire(100f)) ;
            }

            //:: DESTROY GAME OBJECTS ::
            if (other.tag != "BulletEnemy")
            {
                Destroy(gameObject);
            }
            Destroy(explosion, 0.5f);


        }
    }
}
