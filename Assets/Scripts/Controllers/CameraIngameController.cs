using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraIngameController : MonoBehaviour
{

    //:: Camera ::
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private CinemachineVirtualCamera vcam;
    private CinemachineTransposer transposer;

    public IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transposer.m_FollowOffset;

        while (time < duration)
        {
            transposer.m_FollowOffset = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transposer.m_FollowOffset = targetPosition;
    }

    
    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();     
    }
   
}
