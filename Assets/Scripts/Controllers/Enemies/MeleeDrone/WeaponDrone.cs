using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDrone : MonoBehaviour
{
    public GameObject bulletSpawn;
    public GameObject bullet;

    private float lastShootTimestamp;
    public float fireRate;

    //public bool npcAlive;

    void Start()
    {
        lastShootTimestamp = Time.time;
        //npcAlive = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if (Time.time > lastShootTimestamp)
            {
                GameObject newBullet = Instantiate(bullet, bulletSpawn.transform.position, bulletSpawn.transform.rotation, bulletSpawn.transform);
                Destroy(newBullet, 0.25f);
                other.GetComponentInChildren<HPCounterController>().ModifyHealth();
                lastShootTimestamp = Time.time + fireRate;
                SoundManager.Instance.PlaySfx("DroneMelee01");

            }
        }
    }
}
