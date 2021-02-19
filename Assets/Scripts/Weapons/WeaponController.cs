using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    public Weapon currentWeapon;
    public MeshFilter weaponModel;
    public MeshRenderer weaponRenderer;

    public Transform bulletSpawn;
    public GameObject muzzleFlash;

    public bool NPC;

    public void Shoot(bool aimedShoot)
    {
        currentWeapon.Shoot(aimedShoot);
    }

    public void ChangeWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;

        //Setup weapon
        if (!NPC)
            weaponModel = GameObject.Find("Player/Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R/Elbow_R/Hand_R/WeaponEquiped/WeaponModel").GetComponent<MeshFilter>();

        weaponModel.mesh = currentWeapon.weaponMesh;
        currentWeapon.SetBulletSpawn(bulletSpawn);
        currentWeapon.SetInitialShootTimestamp();
        currentWeapon.SetMuzzleFlashFXDuration();

        if(!NPC)
            muzzleFlash = GameObject.Find("Player/Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R/Elbow_R/Hand_R/WeaponEquiped/WeaponModel/MuzzleFlash");

        muzzleFlash.transform.localPosition = currentWeapon.GetMuzzleFlashPosition();


        currentWeapon.SetMuzzleSpawn(muzzleFlash.transform);
    }

    public void ChangeWeaponRender(Material mat)
    {
        if(!NPC)
            weaponRenderer = GameObject.Find("Player/Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R/Elbow_R/Hand_R/WeaponEquiped/WeaponModel").GetComponent<MeshRenderer>(); muzzleFlash = GameObject.Find("Player/Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R/Elbow_R/Hand_R/WeaponEquiped/WeaponModel/MuzzleFlash");
        
        weaponRenderer.material = mat;
    }

    public void SetupWeaponNPC()
    {
        //Setup weapon
        currentWeapon.SetBulletSpawn(bulletSpawn);
        currentWeapon.SetInitialShootTimestamp();
        currentWeapon.SetMuzzleFlashFXDuration();
        muzzleFlash.transform.localPosition = currentWeapon.GetMuzzleFlashPosition();
        currentWeapon.SetMuzzleSpawn(muzzleFlash.transform);
    }

    private void Start()
    {
        if (!NPC)
        {
            weaponModel = GameObject.Find("Player/Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R/Elbow_R/Hand_R/WeaponEquiped/WeaponModel").GetComponent<MeshFilter>();
            weaponRenderer = GameObject.Find("Player/Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R/Elbow_R/Hand_R/WeaponEquiped/WeaponModel").GetComponent<MeshRenderer>();            
            muzzleFlash = GameObject.Find("Player/Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R/Elbow_R/Hand_R/WeaponEquiped/WeaponModel/MuzzleFlash");
            bulletSpawn = GameObject.Find("Player/BulletSpawn").GetComponent<Transform>();
            ChangeWeapon(currentWeapon);
        } else
        {
            SetupWeaponNPC();
        }
        
    }
}
