using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDroneVisuals : MonoBehaviour
{
    private Vector3 initPosition;
    private Vector3 maxPosition;
    private Vector3 minPosition;

    [Range(0, 0.5f)]
    public float yVariation;

    public float timeYAnimation;
    float timeY = 0;

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

    public AttackDroneController attackDroneController;

    [Header("LaserConfig")]
    RaycastHit laserSightHit;
    public float maxLaserSight;
    public LineRenderer laserSight;
    

    public void ShowLaserAim()
    {
        doMovementAnimation = false;
        if (Physics.Raycast(transform.position, transform.forward, out laserSightHit))
        {
            if (laserSightHit.collider.tag != "AttackDrone" && laserSightHit.distance < maxLaserSight)
            {
                laserSight.SetPosition(1, new Vector3(0, 0, laserSightHit.distance));
            }
            else
            {
                laserSight.SetPosition(1, new Vector3(0, 0, maxLaserSight));
            }
        }
        else
        {
            laserSight.SetPosition(1, new Vector3(0, 0, maxLaserSight));
        }
        attackDroneController.PlaceExplosionMark(laserSightHit.point);
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

        laserSight = GetComponent<LineRenderer>();
        laserSight.SetPosition(1, Vector3.zero);

    }

    // Update is called once per frame
    void Update()
    {
        if (doMovementAnimation)
        {
            VerticalAnimation();
        }   
    }
}
