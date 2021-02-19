using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AttackDroneController : MonoBehaviour
{
    //::::: POWERUP :::::
    public bool tutorialWait;
    [Header("SliderSkillConfig")]
    public Color initFillColor;
    public Color endFillColor;
    public float cooldown;


    [Header("Missile")]
    public ParticleSystem missileFXNotSpawnable;
    public GameObject explosionFX;
    public GameObject bulletSpawn;

    [Header("Bar")]
    //Slider slider;
    //public Image fillSlider;
    

    [Header("Circle")]
    public Image backgroundCircle;
    public Image fillCircle;
    public GameObject skillIcon;
    public GameObject keyR;

    [Header("LightSkill")]
    Light lightDrone;
    private float initLightIntensity;
    public float freqLightDrone;
    private float timeToChangeLight;
    public GameObject bulbLightGreen;
    public GameObject bulbLightYellow;



    //::::: NAVIGATION :::::
    GameObject player;
    NavMeshAgent navMeshAgent;

    public GameObject explosionMarkNotSpawneable;
    //objectTest.transform.position = laserSightHit.point;


    IEnumerator LerpSlider(float endValue, float duration)
    {
        float time = 0;
        float startValue;

        //startValue = slider.value;
        startValue = fillCircle.fillAmount;

        while (time < duration)
        {
            //slider.value = Mathf.Lerp(startValue, endValue, time / duration);
            fillCircle.fillAmount = Mathf.Lerp(startValue, endValue, time / duration);
            //fillSlider.color = Color.Lerp(initFillColor, endFillColor, slider.value);
            fillCircle.color = Color.Lerp(initFillColor, endFillColor, fillCircle.fillAmount);
            time += Time.deltaTime;
            yield return null;
        }
        //slider.value = endValue;
        fillCircle.fillAmount = endValue;
    }

    public void ShowExplosionMark(bool visible)
    {
        explosionMarkNotSpawneable.SetActive(visible);
    }

    public void PlaceExplosionMark(Vector3 newPosition)
    {
        if(fillCircle.fillAmount == 1)
        {
            ShowExplosionMark(true);
        }
        Vector3 pos = 0.5f*Vector3.up + newPosition;
        explosionMarkNotSpawneable.transform.position = pos;
    }

     

    private void LoadingSkillLightFX()
    {
        lightDrone.color = Color.yellow;
        bulbLightYellow.SetActive(true);
        bulbLightGreen.SetActive(false);

        timeToChangeLight += Time.deltaTime;

        if (timeToChangeLight >= freqLightDrone)
        {
            timeToChangeLight = 0;

            if (lightDrone.intensity == initLightIntensity)
            {
                lightDrone.intensity = 0;
            } else
            {
                lightDrone.intensity = initLightIntensity;
            }
        }
    }

    public void StateOfCanvasSkill(bool newState)
    {

        backgroundCircle.enabled = newState;
        fillCircle.enabled = newState;
        keyR.SetActive(newState);

    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        ShowExplosionMark(false);

        rocketReadyPlayed = false;

        //slider = GetComponentInChildren<Slider>();

        lightDrone = GetComponentInChildren<Light>();

        initLightIntensity = lightDrone.intensity;

        if (tutorialWait)
        {
            StateOfCanvasSkill(false);
        }

    }

    void DoMovement()
    {
        navMeshAgent.destination = player.transform.position;
        transform.rotation.SetLookRotation(player.transform.forward, player.transform.up);
    }

    bool rocketReadyPlayed;

    private void Update()
    {
        DoMovement();

        if(tutorialWait)
        {
            return;
        }
        
        

        if (fillCircle.fillAmount == 1)
        {
            
            lightDrone.color = Color.cyan;
            fillCircle.color = Color.green;
            lightDrone.intensity = initLightIntensity;
            bulbLightYellow.SetActive(false);
            bulbLightGreen.SetActive(true);

            if(!rocketReadyPlayed)
            {
                rocketReadyPlayed = true;
                SoundManager.Instance.PlaySfx("RocketReady");
            }
            

            skillIcon.SetActive(false);
            keyR.SetActive(true);
        }

        //Reload skill
        if (fillCircle.fillAmount == 0)
        {
            skillIcon.SetActive(true);
            keyR.SetActive(false);
            StartCoroutine(LerpSlider(1, cooldown));
        }
        if(fillCircle.fillAmount != 1)
        {
            LoadingSkillLightFX();
        }

        //Disparar skill
        if (Input.GetKeyDown(KeyCode.R) && explosionMarkNotSpawneable.activeSelf)
        {
            //Puede disparar
            if (fillCircle.fillAmount == 1)
            {
                //Reset cooldown
                fillCircle.fillAmount = 0;
                rocketReadyPlayed = false;

                //Lanzamos skill              
                missileFXNotSpawnable.transform.position = bulletSpawn.transform.position;
                missileFXNotSpawnable.transform.LookAt(explosionMarkNotSpawneable.transform);
                missileFXNotSpawnable.Play();
                SoundManager.Instance.PlaySfx("Trail01");
            } else
            {
                //No puede lanzar skill
            }
        }
    }
}
