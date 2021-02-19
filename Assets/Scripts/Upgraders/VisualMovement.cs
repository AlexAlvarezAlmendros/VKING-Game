using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualMovement : MonoBehaviour
{

    private Vector3 initPosition;
    private Vector3 maxPosition;
    private Vector3 minPosition;

    [Range(0, 0.5f)]
    public float yVariation;

    public float timeYAnimation;
    float timeY = 0;
    public float rotationAngle = 1f;

    private bool goingUp;

    public bool doMovementAnimation;




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


    // Start is called before the first frame update
    void Start()
    {
        doMovementAnimation = true;
        initPosition = transform.localPosition;

        maxPosition = initPosition;
        maxPosition.y += yVariation;

        minPosition = initPosition;
        minPosition.y -= yVariation;

        goingUp = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (doMovementAnimation)
        {
            VerticalAnimation();
            transform.Rotate(Vector3.up, rotationAngle, Space.World);
        }
    }


    


}
