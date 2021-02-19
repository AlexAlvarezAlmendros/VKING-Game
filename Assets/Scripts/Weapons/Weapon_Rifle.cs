using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rifle", menuName = "Data Models/Weapons/Rifle")]
public class Weapon_Rifle : Weapon
{
    [Header("Bullet Configuration")]
    public GameObject bullet;

    public float minSpread;
    public float maxSpread;
    public float xSpread;
    public float ySpread;

    public override void Attack(bool aimedShoot)
    {
        //:: SHOOT BULLETS ::
        Vector3 shootDirection = bulletSpawn.forward;

        if (!aimedShoot) 
        {
            shootDirection.x += Random.Range(-minSpread * xSpread, maxSpread * xSpread);
            shootDirection.y += Random.Range(-minSpread * ySpread, maxSpread * ySpread);
        }

        GameObject newBullet = Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
        newBullet.GetComponent<Rigidbody>().AddForce(shootDirection.normalized * shootForce, ForceMode.Impulse);
        Destroy(newBullet, 30f);
        SoundManager.Instance.PlayRandomSfx(Utils.SFX.BASICRIFLE);
    }
    
    
}
