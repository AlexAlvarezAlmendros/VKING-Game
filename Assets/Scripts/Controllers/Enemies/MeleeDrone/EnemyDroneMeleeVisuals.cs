using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDroneMeleeVisuals : MonoBehaviour
{
    private Vector3 initPosition;
    private Vector3 maxPosition;
    private Vector3 minPosition;

    private Vector3 newDestination;

    [Range(0, 0.5f)]
    public float yVariation;

    [Range(0, 0.5f)]
    public float xVariation;

    public float timeYAnimation;
    float timeY = 0;

    public float timeXAnimation;
    float timeX = 0;
    private float initXValue;
    private float minXValue;
    private float maxXValue;

    private bool goingUp;
    private bool goingLeft;


    public bool doMovementAnimation;
    public GameObject fxTrail;
    public GameObject fxDamage;
    public CapsuleCollider mainCollider;
    public BoxCollider corpseCollider;

    void Start()
    {
        
        initPosition = transform.localPosition;
        newDestination = initPosition;

        maxPosition = initPosition;
        maxPosition.y += yVariation;

        minPosition = initPosition;
        minPosition.y -= yVariation;


        initXValue = transform.localPosition.x;
        maxXValue = initXValue + xVariation;
        minXValue = initXValue - xVariation;
        

        goingUp = true;
        goingLeft = true;
        doMovementAnimation = true;

        mainCollider.enabled = true;
        corpseCollider.enabled = false;
        fxDamage.SetActive(false);
    }

    
    void VerticalAnimation()
    {
        if (goingUp)
        {
            if (timeY < timeYAnimation)
            {
                transform.localPosition = Vector3.Lerp(minPosition, maxPosition, timeY / timeYAnimation);
                timeY += Time.deltaTime;
            }
            else
            {
                timeY = 0;
                goingUp = !goingUp;
            }
        }
        else
        {
            if (timeY < timeYAnimation)
            {
                transform.localPosition = Vector3.Lerp(maxPosition, minPosition, timeY / timeYAnimation);
                timeY += Time.deltaTime;
            }
            else
            {
                timeY = 0;
                goingUp = !goingUp;
            }
        }
    }

    void HorizontalAnimation()
    {
        if (goingLeft)
        {
            if (timeX < timeXAnimation)
            {
                Vector3 newPosition = Vector3.zero;
                newPosition.x = Mathf.Lerp(maxXValue, minXValue, timeX / timeXAnimation);
                transform.localPosition += newPosition;

                timeX += Time.deltaTime;
            }
            else
            {
                newDestination = transform.localPosition;
                timeX = 0;
                goingLeft = !goingLeft;
            }
        }
        else
        {
            if (timeX < timeXAnimation)
            {
                Vector3 newPosition = Vector3.zero;
                newPosition.x = Mathf.Lerp(minXValue, maxXValue, timeX / timeXAnimation);
                transform.localPosition += newPosition;

                timeX += Time.deltaTime;
            }
            else
            {
                newDestination = transform.localPosition;
                timeX = 0;
                goingLeft = !goingLeft;
            }
        }
    }

    public void PlayDeathAnimation()
    {
        mainCollider.enabled = false;
        corpseCollider.enabled = true;
        fxTrail.SetActive(false);
        fxDamage.SetActive(true);
    }

    void Update()
    {
        if(doMovementAnimation)
        {
            VerticalAnimation();
            HorizontalAnimation();
        }

    }
}
