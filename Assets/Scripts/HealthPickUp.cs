using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    public GameObject healFX;
    private HPCounterController playerHPController;

    private void Start()
    {
        playerHPController = GameObject.Find("Player").GetComponentInChildren<HPCounterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(playerHPController.actualDamage > 0)
            {
                //HEAL
                playerHPController.ModifHealthHeal();

                //HEAL FX
                GameObject newHealFX = Instantiate(healFX, other.transform.position, other.transform.rotation, other.transform);
                Destroy(newHealFX, 2f);
                SoundManager.Instance.PlaySfx("Healing");

                //HEAL DESTROY
                Destroy(gameObject);
            }
        }
    }
}
