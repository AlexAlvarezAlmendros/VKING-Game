using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

public abstract class PlayerState
{
    protected SMPlayer mb;          //Reference to MonoBehaviour
    protected Transform trans;      //Reference to transform GameObject.
    protected Animator animator;

    

    //:: Equipments ::
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    protected GameObject weaponEquiped;
    protected WeaponController weaponController;
    protected LineRenderer laserSight;

    //:: Movement ::
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    protected NavMeshAgent navMeshAgent;
    



    public PlayerState(SMPlayer monoBehaviour) {
        
        mb = monoBehaviour;
        animator = mb.GetComponent<Animator>();
        navMeshAgent = mb.GetComponent<NavMeshAgent>();
        trans = mb.transform;
        weaponEquiped = GameObject.Find("Player/Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R/Elbow_R/Hand_R/WeaponEquiped");
        laserSight = weaponEquiped.GetComponent<LineRenderer>();
        weaponController = weaponEquiped.GetComponent<WeaponController>();
        
    }

    public abstract PlayerState CheckTransitions();
    public abstract void Update();

}
