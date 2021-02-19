using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierAutoController : MonoBehaviour
{
    public GameObject movingWall;
    private Vector3 initWallPosition;
    private Vector3 lowWallPosition;

    public Light[] lights;

    public float durationAnimation;
    private bool wallMoving;
    private bool wallUp;

    private float timeLight;
    public float freqLight;    


    void TurnLights()
    {
        foreach (var light in lights)
        {
            if(light.intensity == 0)
            {
                light.intensity = 1;
            } else
            {
                light.intensity = 0;
            }
        }
    }


    public void ChangePositionBarrier()
    {
        if (!wallMoving)
        {
            if (wallUp)
            {
                foreach (Light light in lights)
                {
                    light.color = Color.red;
                }
                StartCoroutine(LerpPosition(initWallPosition, lowWallPosition));
            } else
            {
                foreach (Light light in lights)
                {
                    light.color = Color.yellow;
                }
                StartCoroutine(LerpPosition(lowWallPosition, initWallPosition));
            }
        }
    }

    private IEnumerator LerpPosition(Vector3 startPosition, Vector3 targetPosition)
    {
        float time = 0;
        wallMoving = true;

        while (time < durationAnimation)
        {
            movingWall.transform.position = Vector3.Lerp(startPosition, targetPosition, time / durationAnimation);
            time += Time.deltaTime;
            yield return null;
        }

        movingWall.transform.position = targetPosition;
        wallMoving = false;
        wallUp = !wallUp;


    }

    public Vector3 GetLowWallPosition()
    {
        return lowWallPosition;
    }

    public Vector3 GetInitWallPosition()
    {
        return initWallPosition;
    }

    void Start()
    {
        initWallPosition = movingWall.transform.position;
        lowWallPosition = initWallPosition + new Vector3(0f, -2.25f, 0f);

        wallMoving = false;
        wallUp = true;

        timeLight = 0;

        StartCoroutine(LerpPosition(initWallPosition, lowWallPosition));
    }

    private void Update()
    {
        if (wallMoving)
        {
            timeLight += Time.deltaTime;

           if(timeLight >= freqLight)
            {
                timeLight = 0;
                TurnLights();
            }
            
        } else
        {
            foreach (var light in lights)
            {
                light.intensity = 1;
            }
        }
    }

}
