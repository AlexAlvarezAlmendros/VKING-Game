using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shootgun", menuName = "Data Models/Weapons/Shootgun")]
public class Weapon_Shootgun : Weapon
{
    [Header("Bullet Configuration")]
    public GameObject[] bullets;
    public float minSpread;
    public float maxSpread;
    public float xSpread;
    public float ySpread;
    public bool isPartyShotgun;
    public override void Attack(bool aimedShoot)
    {
        
        //:: SHOOT BULLETS ::
        for (int i = 0; i < bullets.Length; i++)
        {
            Vector3 shootDirection = bulletSpawn.forward;
            
            //First bullet always goes forward
            if (i != 0)
            {
                shootDirection.x += Random.Range(-minSpread*xSpread, maxSpread* xSpread);
                shootDirection.y += Random.Range(-minSpread* ySpread, maxSpread* ySpread);
            }
            
            GameObject newBullet = Instantiate(bullets[i], bulletSpawn.position, bulletSpawn.rotation);
            newBullet.GetComponent<Rigidbody>().AddForce(shootDirection.normalized * shootForce, ForceMode.Impulse);
            Destroy(newBullet, 30f);
            if(isPartyShotgun)
            {
                SoundManager.Instance.PlaySfx("PartyShotgunShoot01");
            }
            
            SoundManager.Instance.PlayRandomSfx(Utils.SFX.ENEMYSHOTGUN);
            
            
        }        
        
    }
}
