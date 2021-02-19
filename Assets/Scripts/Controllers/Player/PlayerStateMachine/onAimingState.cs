using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onAimingState : PlayerState
{
    //:: GET INPUTS ::
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private bool move;
    private bool toggleAim;
    private bool attack;

    //:: MOVEMENT ::
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private Ray rayMovement;
    private RaycastHit hitMovement;

    //:: EQUIPMENT ::
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    RaycastHit laserSightHit;

    // :: CUSTOM FUNCTIONS ::
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    private Ray rayAim;
    private RaycastHit[] hitsRayAim;
    private GameObject weaponAimTarget;

    private void RotatePlayer()
    {
        //Aim with the weapon
        rayAim = Camera.main.ScreenPointToRay(Input.mousePosition);
        hitsRayAim = Physics.RaycastAll(rayAim.origin, rayAim.direction, 2000f);

        //Locate floor to set the aim angle
        for (int i = 0; i < hitsRayAim.Length; i++)
        {
            if (hitsRayAim[i].transform.gameObject.tag == "FloorTarget")
            {
                Vector3 PlayerToTarget = new Vector3(hitsRayAim[i].point.x - trans.position.x, hitsRayAim[i].point.y - trans.position.y, hitsRayAim[i].point.z - trans.position.z);
                Vector3 newDestinationOfTarget = hitsRayAim[i].point;

                //Create a security zone arround player to avoid fast turnings if mouse is over player. Clunky but functional.
                if (PlayerToTarget.magnitude < 1)
                {
                    newDestinationOfTarget.x += 1f;
                    newDestinationOfTarget.z += 1f;
                }
                weaponAimTarget.transform.position = newDestinationOfTarget;
            }
        }

        //Rotate to target,  we let Y(YAW) unedited
        Vector3 lookAtVector = new Vector3(weaponAimTarget.transform.position.x, trans.position.y, weaponAimTarget.transform.position.z);
        trans.LookAt(lookAtVector, Vector3.up);

        //Feedback Visual
        GameManager.Instance.AimDrone(lookAtVector);
        
        
        //Add 45euler grad to fix animation rotations.
        trans.Rotate(Vector3.up, 45f);
    }


    // :: UNITY LOOP FUNCTIONS ::
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public onAimingState(SMPlayer mb):base(mb)
    {

        navMeshAgent.speed = mb.initSpeed - (mb.initSpeed * mb.speedReductionAim);

        //:: DO ANIMATIONS ::
        animator.SetBool("ToggleAimAnimation", true);

        //:: GET REFERENCES ::
        weaponAimTarget = GameObject.Find("Player/WeaponAimTarget");

    }

    public override PlayerState CheckTransitions()
    {
        if (!toggleAim)
        {

            Cursor.visible = true;

            //:: RESET LASER ATTACK DRONE ::
            GameManager.Instance.ResetLaserAttackDrone();

            //:: ZOOM IN ::
            GameManager.Instance.doZoom(GameManager.Instance.zoomInOffset, 1f);

            //:: DO ANIMATIONS ::
            animator.SetBool("ToggleAimAnimation", false);

            return new onGroundState(mb);
        }

        return null;
    }

    public override void Update()
    {

        //:: PLAYER DEATH ::
        if (!GameManager.Instance.isPlayerAlive)
        {
            return;
        }

        //:: GET INPUTS ::
        toggleAim = Input.GetButton("ToggleAim");
        move = Input.GetButton("Move");
        attack = Input.GetButton("Attack");


        //:: DO MOVEMENT ::
        if (move)
        {
            rayMovement = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(rayMovement, out hitMovement, 100))
            {
                //Seteo a zero para conseguir movimientos rapidos al cambiar de destino.
                navMeshAgent.velocity = Vector3.zero;
                navMeshAgent.destination = hitMovement.point;
            }
        }

        //:: DO ANIMATIONS ::
        animator.SetFloat("SpeedCharacterAnimation", navMeshAgent.velocity.magnitude);


        //:: DO AIMING ::
        //Rotate player
        RotatePlayer();

        //Laser displayed
        if (Physics.Raycast(weaponEquiped.transform.position, weaponEquiped.transform.forward, out laserSightHit))
        {
            if (laserSightHit.collider.tag != "PlayerWeaponEquiped" && laserSightHit.distance < mb.maxLaserSight)
            {
                laserSight.SetPosition(1, new Vector3(0, 0, laserSightHit.distance));
            }
            else
            {
                laserSight.SetPosition(1, new Vector3(0, 0, mb.maxLaserSight));
            }
        }
        else
        {
            laserSight.SetPosition(1, new Vector3(0, 0, mb.maxLaserSight));
        }


        //:: DO ATTACK ::
        if(attack)
        {
            weaponController.Shoot(true);
        }

    }
}
