using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onGroundState : PlayerState
{

    //:: Get Inputs ::
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private bool move;
    private bool toggleAim;
    private bool attack;


    //:: Movement ::
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private Ray rayMovement;
    private RaycastHit[] hitMovement;

    //:: Attack No Aim ::
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private Ray rayAttackNoAim;
    private RaycastHit[] hitAttackNoAim;
    private GameObject weaponAimTarget;


    // :: UNITY LOOP FUNCTIONS ::
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public onGroundState(SMPlayer mb) :  base(mb)
    {
        navMeshAgent.speed = mb.initSpeed;
        laserSight.SetPosition(1, new Vector3(0, 0, mb.initLaserSight));

        //:: GET REFERENCES ::
        weaponAimTarget = GameObject.Find("Player/WeaponAimTarget");
    }
 
    
    public override PlayerState CheckTransitions()
    {
        if (toggleAim)
        {
            Cursor.visible = false;

            //:: ZOOM OUT ::
            GameManager.Instance.doZoom(GameManager.Instance.zoomOutOffset, 0.5f);
            Vector3 changeDirection = new Vector3(0.25f, 0.25f, 0.25f);
            navMeshAgent.velocity.Scale(changeDirection);
            

            return new onAimingState(mb);
        }

        return null;
    }

    public override void Update()
    {

        //:: PLAYER DEATH ::
        if(!GameManager.Instance.isPlayerAlive)
        {
            return;
        }

        //:: GET INPUTS ::
        move = Input.GetButton("Move");
        toggleAim = Input.GetButton("ToggleAim");
        attack = Input.GetButton("Attack");

        //:: DO MOVEMENT ::

        //Fix rotation to perform moving animation
        if (navMeshAgent.velocity.magnitude > 0)
        {
            if (trans.rotation.y <= 45)
            {
                trans.Rotate(Vector3.up, 1);
            }
        }

        //Do movement
        if (move)
        {
            rayMovement = Camera.main.ScreenPointToRay(Input.mousePosition);
            hitMovement = Physics.RaycastAll(rayMovement.origin, rayMovement.direction, 2000f);

            
            Vector3 changeDirection = new Vector3(0.75f, 0.75f, 0.75f);
            navMeshAgent.velocity.Scale(changeDirection);

            
            for (int i = 0; i < hitMovement.Length; i++)
            {
                if (hitMovement[i].transform.gameObject.tag == "Floor")
                {
                    navMeshAgent.destination = hitMovement[i].point;
                    i = hitMovement.Length+1;
                }
            }

            //OLD
            //if (Physics.Raycast(rayMovement, out hitMovement, 100))
            //{
            //    //Seteo a zero para conseguir movimientos rapidos al cambiar de destino.
            //    navMeshAgent.velocity = Vector3.zero;
            //    navMeshAgent.destination = hitMovement.point;
            //}


            //Do movement feedback FX         
            GameManager.Instance.DoMovementFeedbackFX(navMeshAgent.destination);

        }


        //:: DO ANIMATIONS ::
        animator.SetFloat("SpeedCharacterAnimation", navMeshAgent.velocity.magnitude);

        //:: DO ATTACK ::
        if (attack)
        {

            rayAttackNoAim = Camera.main.ScreenPointToRay(Input.mousePosition);
            hitAttackNoAim = Physics.RaycastAll(rayAttackNoAim.origin, rayAttackNoAim.direction, 2000f);



            //Locate floor to set the aim angle
            
            for (int i = 0; i < hitAttackNoAim.Length; i++)
            {
                
                if (hitAttackNoAim[i].transform.gameObject.tag == "FloorTarget")
                {
                    Vector3 PlayerToTarget = new Vector3(hitAttackNoAim[i].point.x - trans.position.x, hitAttackNoAim[i].point.y - trans.position.y, hitAttackNoAim[i].point.z - trans.position.z);
                    Vector3 newDestinationOfTarget = hitAttackNoAim[i].point;

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

            //Add 45euler grad to fix animation rotations.            
            trans.Rotate(Vector3.up, 45f);



            //Disparar aplicando penalizador
            weaponController.Shoot(false);
            
        }

        


    }
}
