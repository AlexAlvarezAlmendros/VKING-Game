using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    // ::: GET INPUTS :::
    // ----------------------------------------------------------------------------------------------------------------------------------
    // ----------------------------------------------------------------------------------------------------------------------------------
    //private bool attack;

    // ----------------------------------------------------------------------------------------------------------------------------------


    // ::: PLAYER VARIABLES :::
    // ----------------------------------------------------------------------------------------------------------------------------------
    // ----------------------------------------------------------------------------------------------------------------------------------
    //public float maxLaserSight;
    //public float initLaserSight;
    //[Range(0,1)]
    //public float speedReductionAim;     //Limita la velocidad cuando hace AIM. 0 = no modifica, el resto es el % que se resta a su initSpeed

    // ----------------------------------------------------------------------------------------------------------------------------------


    // ::: WEAPON VARIABLES :::
    // ----------------------------------------------------------------------------------------------------------------------------------
    // ----------------------------------------------------------------------------------------------------------------------------------

    public float weaponFireRate;                  //Valor en segundos
    public float weaponRange;
    public GameObject bulletTracer;

    //private GameObject bulletSpawn;
    //private float lastShootTimestamp;
    private RaycastHit hitShoot;
    // ----------------------------------------------------------------------------------------------------------------------------------


    // ::: MOVEMENT :::
    // ----------------------------------------------------------------------------------------------------------------------------------
    // ----------------------------------------------------------------------------------------------------------------------------------
    //private Ray rayMovement;
    //private RaycastHit hitMovement;

    //private float initSpeed;   

    //private NavMeshAgent navMeshAgent;

    //private GameObject weaponEquiped;
    //private LineRenderer laserSight;

    //private Ray rayAim;
    //private RaycastHit[] hitsRayAim;

    // ----------------------------------------------------------------------------------------------------------------------------------



    // ::: ANIMATIONS :::
    // ----------------------------------------------------------------------------------------------------------------------------------
    // ----------------------------------------------------------------------------------------------------------------------------------

    //private Animator animator;
    //private GameObject weaponAimTarget;

    // ----------------------------------------------------------------------------------------------------------------------------------


    /*
    private void Shoot()
    {


        if(Physics.Raycast(bulletSpawn.transform.position, bulletSpawn.transform.forward, out hitShoot, weaponRange))
        { 
            GameObject bullet = GameObject.Instantiate(bulletTracer, bulletSpawn.transform.position, bulletSpawn.transform.rotation) as GameObject;
            bullet.GetComponent<ShotBehavior>().setTarget(hitShoot.point);
            GameObject.Destroy(bullet, 30f);
        }

    }
    */
    //private void RotatePlayer()
    //{
    //    //Apuntado del arma
    //    // -------------------------------------------------------
    //    // -------------------------------------------------------
    //    rayAim = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    hitsRayAim = Physics.RaycastAll(rayAim.origin, rayAim.direction, 2000f);

    //    //Localizar suelo para mover el target
    //    for (int i = 0; i < hitsRayAim.Length; i++)
    //    {
    //        if (hitsRayAim[i].transform.gameObject.tag == "Floor")
    //        {
    //            Vector3 PlayerToTarget = new Vector3(hitsRayAim[i].point.x - transform.position.x, hitsRayAim[i].point.y - transform.position.y, hitsRayAim[i].point.z - transform.position.z);
    //            Vector3 newDestinationOfTarget = hitsRayAim[i].point;

    //            //Create a security zone arround player to avoid fast turnings if mouse is over player
    //            //Method1: Clunky but functional
    //            if (PlayerToTarget.magnitude < 1)
    //            {
    //                newDestinationOfTarget.x += 1f;
    //                newDestinationOfTarget.z += 1f;
    //            }
    //            weaponAimTarget.transform.position = newDestinationOfTarget;

    //            //Method2: Smooth, but problems with minrange!!!
    //            //target.transform.position = transform.position + Vector3.ClampMagnitude(PlayerToTarget, 1f);                   

    //        }
    //    }

    //    //Rotate to target, no modifizamos la Y para evitar YAW
    //    Vector3 lookAtVector = new Vector3(weaponAimTarget.transform.position.x, transform.position.y, weaponAimTarget.transform.position.z);
    //    transform.LookAt(lookAtVector);
    //    //Añadimos 45grados para corregir la rotacion de las animaciones.
    //    transform.Rotate(Vector3.up, 45f);
    //}


    private void Awake()
    {
        //navMeshAgent = GetComponent<NavMeshAgent>();
        //animator = GetComponent<Animator>();
        //weaponEquiped = GameObject.Find("Player/Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R/Elbow_R/Hand_R/WeaponEquiped");
        //laserSight = weaponEquiped.GetComponent<LineRenderer>();
        //initSpeed = navMeshAgent.speed;
        //bulletSpawn = GameObject.Find("Player/BulletSpawn");

        //DESACTIVAR EN BUILD FINAL
        //weaponAimTarget = GameObject.Find("Player/WeaponAimTarget");
    }

    private void Start()
    {
        //laserSight.SetPosition(1, new Vector3(0, 0, initLaserSight));

        //weaponAimTarget.GetComponent<MeshRenderer>().enabled = false;
    }

    void Update()
    {
        // ::: GET INPUTS :::
        // ----------------------------------------------------------------------------------------------------------------------------------
        // ----------------------------------------------------------------------------------------------------------------------------------
        //if (Input.GetButton("Move"))
        //{
        //    move = true;
        //} else
        //{
        //    move = false;
        //}

        //if (Input.GetButton("ToggleAim"))
        //{
        //    toggleAim = true;
        //} else
        //{
        //    toggleAim = false;
        //}

        //if (Input.GetButton("Attack"))
        //{
        //    attack = true;
        //}
        //else
        //{
        //    attack = false;
        //}

        // ----------------------------------------------------------------------------------------------------------------------------------

        // ::: MOVEMENT :::
        // ----------------------------------------------------------------------------------------------------------------------------------
        // ----------------------------------------------------------------------------------------------------------------------------------
        //if(navMeshAgent.velocity.magnitude > 0)
        //{
        //    if (transform.rotation.y <= 45)
        //    {
        //        transform.Rotate(Vector3.up, 1);
        //    }
        //}
        
        //if (move)
        //{

        //    rayMovement = Camera.main.ScreenPointToRay(Input.mousePosition);
            
        //    if (Physics.Raycast(rayMovement, out hitMovement, 100))
        //    {
        //        //Seteo a zero para conseguir movimientos rapidos al cambiar de destino.
        //        navMeshAgent.velocity = Vector3.zero;
        //        navMeshAgent.destination = hitMovement.point;
        //    }
        //}

        /*
        if(attack)
        {
            //Rotate player
            // -------------------------------------------------------
            RotatePlayer();
            if(Time.time > lastShootTimestamp)
            {
                lastShootTimestamp = Time.time + weaponFireRate;
                Shoot();
            }
        }
        */

        //if(toggleAim)
        //{
        //    //Control de velocidad maxima
        //    // -------------------------------------------------------
        //    navMeshAgent.speed = initSpeed - (initSpeed*speedReductionAim);

        //    //Rotate player
        //    // -------------------------------------------------------
        //    RotatePlayer();

        //    //Laser displayed
        //    // -------------------------------------------------------
        //    RaycastHit laserSightHit;
        //    if (Physics.Raycast(weaponEquiped.transform.position, weaponEquiped.transform.forward, out laserSightHit))
        //    {
        //        if (laserSightHit.collider.tag != "PlayerWeaponEquiped" && laserSightHit.distance<maxLaserSight)
        //        {
        //            laserSight.SetPosition(1, new Vector3(0, 0, laserSightHit.distance));
        //        } else {
        //            laserSight.SetPosition(1, new Vector3(0, 0, maxLaserSight));
        //        } 
        //    }
        //    else
        //    {
        //        laserSight.SetPosition(1, new Vector3(0, 0, maxLaserSight));
        //    }
            
            
        //}
        //else
        //{
        //    navMeshAgent.speed = initSpeed;
        //    laserSight.SetPosition(1, new Vector3(0, 0, initLaserSight));

        //}

        // ----------------------------------------------------------------------------------------------------------------------------------

        // ::: ANIMATIONS :::
        // ----------------------------------------------------------------------------------------------------------------------------------
        // ----------------------------------------------------------------------------------------------------------------------------------
        //if (toggleAim)
        //{
        //    animator.SetBool("ToggleAimAnimation", true);
        //} else
        //{
        //    animator.SetBool("ToggleAimAnimation", false);
        //}


        
        
        
        //animator.SetFloat("SpeedCharacterAnimation", navMeshAgent.velocity.magnitude);
        // ----------------------------------------------------------------------------------------------------------------------------------
    }
}
