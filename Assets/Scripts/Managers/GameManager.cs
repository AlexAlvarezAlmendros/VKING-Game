using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using System;

public class GameManager : MonoBehaviour
{
    

    /*index
      ########################
      #                      #
      #  CHARACTER POWERUPS  #
      #                      #
      ########################
    */

    private GameObject attackDrone;
    private GameObject attackDronePR1N7;
    
    public void ResetLaserAttackDrone()
    {
        attackDrone.GetComponentInChildren<AttackDroneVisuals>().laserSight.SetPosition(1, Vector3.zero);
        attackDrone.GetComponentInChildren<AttackDroneVisuals>().doMovementAnimation = true;
        attackDrone.GetComponentInChildren<AttackDroneController>().ShowExplosionMark(false);
    }


    public void AimDrone(Vector3 lookAtVector)
    {
        attackDrone.transform.LookAt(lookAtVector);
        attackDrone.GetComponentInChildren<AttackDroneVisuals>().ShowLaserAim();
    }


    public bool vikingShield = false;
    public bool pr1n7Module = false;

    public bool basicRifle = true;
    public Weapon basicRifleWeapon;
    public bool partyShotgun = false;
    public Weapon partyShotgunWeapon;
    public bool pewpewRifle = false;
    public Weapon pewpewRifleWeapon;

    public void resetCharacter()
    {
        vikingShield = false;
        pr1n7Module = false;

        basicRifle = true;
        partyShotgun = false;
        pewpewRifle = false;
    }

    public Material attackDroneBlack;
    public Material attackDroneWhite;
    public Material attackDroneGold;
    public Material pewpewRifleMat;
    public Material partyShotgunMat;


    public void loadCharacterPowerups()
    {

        isPlayerAlive = true;
        if (vikingShield)
        {
            GameObject.Find("Item_Shield").GetComponent<MeshRenderer>().enabled = true;
            initPlayerHP = 12;
        }
        else
        {
            GameObject.Find("Item_Shield").GetComponent<MeshRenderer>().enabled = false;
            initPlayerHP = 8;
        }

        if(pr1n7Module)
        {
            attackDrone = GameObject.Find("AttackDrone");
            attackDrone.GetComponent<AttackDroneController>().cooldown = 4f;
            GameObject.Find("AttackDroneModel").GetComponent<MeshRenderer>().material = attackDroneBlack;
        } else
        {
            attackDrone = GameObject.Find("AttackDrone");
            attackDrone.GetComponent<AttackDroneController>().cooldown = 8f;
            GameObject.Find("AttackDroneModel").GetComponent<MeshRenderer>().material = attackDroneWhite;
        }

        WeaponController weaponEquiped = GameObject.Find("Player").GetComponentInChildren<WeaponController>();
        if (partyShotgun)
        {
            weaponEquiped.ChangeWeapon(partyShotgunWeapon);
            weaponEquiped.ChangeWeaponRender(partyShotgunMat);
        } else if (pewpewRifle)
        {
            weaponEquiped.ChangeWeapon(pewpewRifleWeapon);
            weaponEquiped.ChangeWeaponRender(pewpewRifleMat);
        } else
        {
            weaponEquiped.ChangeWeapon(basicRifleWeapon);
            weaponEquiped.ChangeWeaponRender(attackDroneWhite);
        }
    }



    /*index
      #############################
      #                           #
      #  CHARACTER CONFIGURATION  #
      #                           #
      #############################
    */

    

    public int initPlayerHP;
    public bool isPlayerAlive;

    public GameObject movementFeedbackFX;


    GameObject movementEffect;

    public void DoMovementFeedbackFX(Vector3 spawnPosition)
    {
        if(movementEffect == null)
        {
            spawnPosition.y += 0.5f;
            movementEffect = Instantiate(movementFeedbackFX, spawnPosition, Quaternion.identity);
            Destroy(movementEffect, 0.5f);
        }
        /*
        if (actualTime >= timeLimit)
        {
            actualTime = 0;
            spawnPosition.y += 0.5f;
            movementEffect = Instantiate(movementFeedbackFX, spawnPosition, Quaternion.identity);
            Destroy(movementEffect, 1f);
        }*/
        
    }

    /*index
      ##########################
      #                        #
      #  CAMERA CONFIGURATION  #
      #                        #
      ##########################
    */


    private PostProcessVolume actualPPVolume;
    private FloatParameter vignetteDamageIntensity;
    public float minIntensity = 0.39f;
    public float maxIntensity = 0.65f;
    public float durationDamage = 0.5f;

    
    public void SetActualPPVolume()
    {
        actualPPVolume = GameObject.Find("Main Camera").GetComponent<PostProcessVolume>();
        vignetteDamageIntensity = actualPPVolume.profile.GetSetting<Vignette>().intensity;
    }

    public void SetDamageIndicator()
    {
        
        StartCoroutine(LerpDamage(minIntensity, durationDamage));
        
    }

    IEnumerator LerpDamage(float endValue, float duration)
    {
        float time = 0;
        float startValue;

        startValue = maxIntensity;

        while (time < duration)
        {
            vignetteDamageIntensity.value = Mathf.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        vignetteDamageIntensity.value = endValue;
    }

    public void DeathLoadMainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void PlayerKilled()
    {
        isPlayerAlive = false;
        Invoke("DeathLoadMainMenu", 2f);
    }

    private CameraIngameController cmvcam;
    public Vector3 zoomOutOffset;
    public Vector3 zoomInOffset;

    public void SetZoomOffset(Vector3 zoomOut, Vector3 zoomIn)
    {
        zoomOutOffset = zoomOut;
        zoomInOffset = zoomIn;
    }
    

    public void SetSceneVariables()
    {
        cmvcam = GameObject.Find("CMvcam").GetComponent<CameraIngameController>();
        attackDrone = GameObject.Find("AttackDrone");
        SetActualPPVolume();
    }

    public void doZoom(Vector3 destination, float time)
    {
        StartCoroutine(cmvcam.LerpPosition(destination, time));
    }


    /*index
      ########################
      #                      #
      #  FUNCIONES DE UNITY  #
      #                      #
      ########################
    */


    // Instanciar GameManager
    public static GameManager Instance { get; private set; }

    private void Awake()
    {

        
        if (Instance == null)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 50;

            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.Log("Warning: multiple " + this + " in scene!");
        }
    }

    public Texture2D cursorCombat;
     


    private void Start()
    {
        Cursor.SetCursor(cursorCombat, new Vector2(24f, 24f), CursorMode.ForceSoftware);

        isPlayerAlive = true;
        

        
    }

    public void LoadLevelById(int idMap)
    {
        SceneManager.LoadScene(idMap);
    }
    

    private int idMap;
    private void Update()
    {
        //CHEATS
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //GoToMainMenu
            SceneManager.LoadScene(1);
        } 
        else if (Input.GetKeyDown(KeyCode.Alpha1)){
            SceneManager.LoadScene(3);
        } 
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SceneManager.LoadScene(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SceneManager.LoadScene(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SceneManager.LoadScene(7);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SceneManager.LoadScene(8);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            idMap = SceneManager.GetActiveScene().buildIndex;

            if (idMap != 0 && idMap != 1 && idMap !=8)
            {
                attackDrone.GetComponent<AttackDroneController>().cooldown = 1f;
                GameObject.Find("AttackDroneModel").GetComponent<MeshRenderer>().material = attackDroneGold;
            }
        }



    }
}
