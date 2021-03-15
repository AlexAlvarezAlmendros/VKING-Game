using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public GameObject hitParticle;
    public Utils.ElementType elementType;
    //private Renderer renderer;
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
                //renderer = gameObject.GetComponent<Renderer>();
                //renderer.enabled = false;
                Destroy(gameObject, 5);
                Destroy(explosionF, 0.5f);
            }

            //:: DO HIT FX ::
            GameObject explosion = Instantiate(hitParticle, transform.position, transform.rotation);

            //Debug.Log("BulletBehaviour - other.tag: " + other.tag);
            //Debug.Log("BulletBehaviour - transform.tag: " + transform.tag);

            //:: DESTROY GAME OBJECTS ::
            switch (elementType)
            {
                case Utils.ElementType.BASIC:
                    {
                        if (other.tag == "Player" && transform.tag == "BulletEnemy")
                        {
                            other.GetComponentInChildren<HPCounterController>().ModifyHealth();
                        }
                        if ((other.tag == "Boss" || other.tag == "Enemy") && transform.tag == "BulletPlayer")
                        {
                            other.GetComponentInChildren<HPCounterController>().ModifyHealth();
                        }
                    }
                    break;
                case Utils.ElementType.DARKNESS:
                    {

                    }
                    break;
                case Utils.ElementType.FIRE:
                    {
                        if (other.tag == "Player" && transform.tag == "BulletEnemy")
                        {
                            StartCoroutine(other.GetComponentInChildren<HPCounterController>().ModifyHealthFire(2,1));
                        }
                        if ((other.tag == "Boss" || other.tag == "Enemy") && transform.tag == "BulletPlayer")
                        {
                            StartCoroutine(other.GetComponentInChildren<HPCounterController>().ModifyHealthFire(2,1));
                        }
                    }
                    break;
                case Utils.ElementType.WIND:
                    {
                        {
                            if (other.tag == "Player" && transform.tag == "BulletEnemy")
                            {
                                StartCoroutine(other.GetComponentInChildren<HPCounterController>().ModifyHealthWind(1,1));
                            }
                            if ((other.tag == "Boss" || other.tag == "Enemy") && transform.tag == "BulletPlayer")
                            {
                                StartCoroutine(other.GetComponentInChildren<HPCounterController>().ModifyHealthWind(1,1));
                            }
                        }
                    }
                    break;
                case Utils.ElementType.LIGHTNING:
                    {

                    }
                    break;
                case Utils.ElementType.LIFE:
                    {

                    }
                    break;
                default:
                    break;
            }


            //:: DESTROY GAME OBJECTS ::
            if (other.tag != "BulletEnemy")
            {
                //gameObject.SetActive(false);
                Destroy(gameObject, 5);
            }
            Destroy(explosion, 0.5f);


        }
    }
}
