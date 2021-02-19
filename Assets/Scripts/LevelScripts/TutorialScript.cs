using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    private GameObject enemyIngame;
    private SensePlayerDrone npcBrain;
    public GameObject enemyPrefab;
    public GameObject enemyPrefab2;
    public GameObject enemySpawnPoint;
    public GameObject magicSpawnPrefab;

    public GameObject fireCanvas;
    public GameObject fireInfoCanvas;

    public GameObject aimCanvas;
    public GameObject aimInfoCanvas;

    public GameObject rocketInfoCanvas;

    public GameObject exitZone;
    public GameObject exitTextCanvas;
    public GameObject exitInfoCanvas;

    private int movementCount;
    private int movementMax;

    public GameObject sprayFX;

    private Image fillImage;
    private AttackDroneController attackDroneController;

    private bool movementTuto;
    private bool fireTuto;
    private bool aimTuto;
    private bool rocketTuto;
    private bool endlessSpawn;

    private bool waitForEnemy;


    private void Start()
    {
        
        movementCount = 0;
        movementMax = 2;

        fillImage = GameObject.Find("FillCircle").GetComponent<Image>();
        attackDroneController = GameObject.Find("AttackDrone").GetComponent<AttackDroneController>();

        movementTuto = true;

        fireTuto = false;
        aimTuto = false;
        rocketTuto = false;
        endlessSpawn = false;
        waitForEnemy = false;

        exitZone.SetActive(false);
        
        SoundManager.Instance.StopAllAudios();
        SoundManager.Instance.PlayMusic("Ambient");

    }

    void Update()
    {
        waitForEnemy = false;

        //Tutorial MOVIMIENTO >>>> FIRE
        if(movementTuto)
        {
            if (Input.GetButtonDown("Move"))
            {
                movementCount++;
            }

            if (movementCount >= movementMax)
            {
                movementTuto = false;
                fireTuto = true;

                fireCanvas.SetActive(true);
                fireInfoCanvas.SetActive(true);
                SoundManager.Instance.PlaySfx("DroneTalk01");

                GameObject newSpray = Instantiate(sprayFX, fireCanvas.transform.position, fireCanvas.transform.rotation);
                Destroy(newSpray, 2f);
                SoundManager.Instance.PlaySfx("SprayPaint");

                SpawnEnemy(enemyPrefab2);
                waitForEnemy = true;
            }
        }

        //Tutorial FIRE >>>> AIM
        if(fireTuto)
        {
            if(!waitForEnemy && !npcBrain.npcAlive)
            {
                fireTuto = false;
                aimTuto = true;

                fireInfoCanvas.SetActive(false);

                aimCanvas.SetActive(true);
                aimInfoCanvas.SetActive(true);
                SoundManager.Instance.PlaySfx("DroneTalk02");

                GameObject newSpray = Instantiate(sprayFX, aimCanvas.transform.position, aimCanvas.transform.rotation);
                Destroy(newSpray, 2f);
                SoundManager.Instance.PlaySfx("SprayPaint");

                SpawnEnemy(enemyPrefab2);
                waitForEnemy = true;
            }
        }

        //Tutorial AIM >>>>> ROCKET
        if(aimTuto)
        {
            if (!waitForEnemy && !npcBrain.npcAlive)
            {
                aimTuto = false;
                rocketTuto = true;

                aimInfoCanvas.SetActive(false);

                rocketInfoCanvas.SetActive(true);
                SoundManager.Instance.PlaySfx("DroneTalk01");

                SpawnEnemy(enemyPrefab2);
                SpawnEnemy(enemyPrefab2);
                SpawnEnemy(enemyPrefab);
                waitForEnemy = true;
            }
        }


        //Tutorial ROCKET >>>> EXIT
        if(rocketTuto)
        {
            if(attackDroneController.tutorialWait)
            {
                attackDroneController.tutorialWait = false;
                attackDroneController.StateOfCanvasSkill(true);
                fillImage.fillAmount = 1f;
            }


            if (!waitForEnemy && !npcBrain.npcAlive)
            {
                rocketTuto = false;
                endlessSpawn = true;

                rocketInfoCanvas.SetActive(false);
                exitZone.SetActive(true);
                exitZone.GetComponent<ParticleSystem>().Play();
                exitTextCanvas.SetActive(true);
                exitInfoCanvas.SetActive(true);
                SoundManager.Instance.PlaySfx("DroneTalk02");

                GameObject newSpray = Instantiate(sprayFX, exitTextCanvas.transform.position, exitTextCanvas.transform.rotation);
                Destroy(newSpray, 2f);
                SoundManager.Instance.PlaySfx("SprayPaint");
            }
        }

        if(endlessSpawn)
        {
            if (!waitForEnemy && !npcBrain.npcAlive)
            {
                SpawnEnemy(enemyPrefab);
            }
        }
    }

    

    void SpawnEnemy(GameObject foe)
    {
        if(endlessSpawn)
        {
            Destroy(enemyIngame, 3f);
        }

        GameObject newEnemy = Instantiate(foe, enemySpawnPoint.transform.position, enemySpawnPoint.transform.rotation);
        enemyIngame = newEnemy;
        npcBrain = enemyIngame.GetComponentInChildren<SensePlayerDrone>();

        GameObject newSpawn = Instantiate(magicSpawnPrefab, enemySpawnPoint.transform.position, enemySpawnPoint.transform.rotation);
        Destroy(newSpawn, 2f);
    }

}
