using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMPlayer : MonoBehaviour
{
    private PlayerState currentState;


    // :: INIT VARIABLES ::
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    [Header("INIT Variables")]
    // :: Equipment ::
    public float initLaserSight;
    public float maxLaserSight;
    

    // :: Movement ::
    public float initSpeed;
    
    [Range(0, 1)]
    public float speedReductionAim;     //Limita la velocidad cuando hace AIM. 0 = no modifica, el resto es el % que se resta a su initSpeed

    


    // :: UNITY LOOP FUNCTIONS ::
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    private void Start()
    {
        currentState = new onGroundState(this);
    }

    private void Update()
    {
        currentState.Update();
    }

    private void LateUpdate()
    {
        PlayerState newState = currentState.CheckTransitions();
        if (newState != null)
        {
            currentState = newState;
        }
    }
}
