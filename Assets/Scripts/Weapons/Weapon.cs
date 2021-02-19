using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon: ScriptableObject
{
    [Header("Weapon Variables")]
    // :: WEAPON VARIABLES ::
    public float damage;

    public float fireRate;

    protected float lastShootTimestamp;

    // :: WEAPON BULLET CONTROLLER ::
    protected Transform bulletSpawn;   //WeaponController se lo asigna.

    public float shootForce;

    // :: WEAPON MUZZLE FLASH ::
    [Header("Muzzle FX Configuration")]
    public GameObject muzzleFlashFX;
    protected float muzzleFlashFXDuration;
    public Vector3 muzzleFlashPositionOffset;
    protected Transform muzzleFlashPosition;

    [Header("Weapon Model")]
    // :: WEAPON MODEL ::
    public Mesh weaponMesh;

        
    // :: WEAPON FUNCTIONS ::
    
    public Vector3 GetMuzzleFlashPosition()
    {
        return muzzleFlashPositionOffset;
    }
    public void SetMuzzleSpawn(Transform mfp)
    {
        muzzleFlashPosition = mfp;
    }
    public void SetMuzzleFlashFXDuration()
    {
        muzzleFlashFXDuration = muzzleFlashFX.GetComponent<ParticleSystem>().main.duration;
    }
    public void SetBulletSpawn(Transform bs)
    {
        bulletSpawn = bs;
    }
    public void SetInitialShootTimestamp()
    {
        lastShootTimestamp = Time.time;
    }


    private void MuzzleFlash()
    {
        //:: DO MUZZLE FLASH FX ::
        if (muzzleFlashFX != null)
        {
            GameObject flashEffect = Instantiate(muzzleFlashFX, muzzleFlashPosition.position, muzzleFlashPosition.rotation, muzzleFlashPosition.transform);
            Destroy(flashEffect, 1f);
        }
    }
    public abstract void Attack(bool aimedShoot);

    public void Shoot(bool aimedShoot)
    {
        if (Time.time > lastShootTimestamp)
        {
            lastShootTimestamp = Time.time + fireRate;
            MuzzleFlash();
            Attack(aimedShoot);
        }
    }

    
}
