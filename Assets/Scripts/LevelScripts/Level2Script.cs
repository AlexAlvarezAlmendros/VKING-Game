using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Level2Script : MonoBehaviour
{
    float limitX;
    bool camRotada;
    private GameObject player;

    private CinemachineVirtualCamera vcam;
    private CinemachineTransposer transposer;

    public float timeChange;

    public float newCamPositionX;
    private Vector3 initCamPosition;

    float cooldownCam;
    float changeReadyAt;

    // Start is called before the first frame update
    void Start()
    {
        changeReadyAt = Time.time;
        cooldownCam = 1.5f;
        limitX = 5.5f;
        camRotada = false;
        player = GameObject.Find("Player");

        vcam = GameObject.Find("CMvcam").GetComponent<CinemachineVirtualCamera>();
        transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();

        initCamPosition = transposer.m_FollowOffset;

        SoundManager.Instance.StopAllAudios();
        SoundManager.Instance.PlayMusic("Ambient");
        //SoundManager.Instance.PlayMusic("Combat");
    }



    IEnumerator LerpRotarCam(float endValue, float duration)
    {
        float time = 0;
        float startValue;

        //startValue = slider.value;
        startValue = transposer.m_FollowOffset.x;

        while (time < duration)
        {

            float newValueOffsetX = Mathf.Lerp(startValue, endValue, time / duration);

            transposer.m_FollowOffset.x = newValueOffsetX;
            GameManager.Instance.zoomInOffset.x = newValueOffsetX;
            GameManager.Instance.zoomOutOffset.x = newValueOffsetX * 1.25f;
            time += Time.deltaTime;
            yield return null;
        }

        //GameManager.Instance.zoomOutOffset.z = endValue;
    }

    

    // Update is called once per frame
    void Update()
    {

        if (changeReadyAt < Time.time)
        {
            
            if (player.transform.position.x < limitX && !camRotada)
            {
                changeReadyAt = Time.time + cooldownCam;
                
                camRotada = true;
                StartCoroutine(LerpRotarCam(newCamPositionX, timeChange));
            }
            else if (player.transform.position.x >= limitX && camRotada)
            {
                changeReadyAt = Time.time + cooldownCam;
                
                camRotada = false;
                StartCoroutine(LerpRotarCam(initCamPosition.x, timeChange));
            }
        }

        if (!SoundManager.Instance.SoundIsPlaying(Utils.AudioType.MUSIC, "Combat"))
        {
            SoundManager.Instance.PlayMusic("Combat");
        }


    }
}
