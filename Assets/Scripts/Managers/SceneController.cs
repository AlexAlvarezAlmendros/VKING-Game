using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.AI;

public class SceneController : MonoBehaviour
{

    //:: NextLevel ::
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    private GameObject exitZone;
    public bool tutorial_store_Level;
    private bool allEnemiesKilled;

    public SensePlayer[] enemyHumans;
    public SensePlayerDrone[] enemyDrones;
    public SupportTowerController[] enemyTowers;
    public bool bossLevel;
    public SensePlayerBoss[] enemyBoss;
    private List<SensePlayerDrone> enemyDronesCompanions = new List<SensePlayerDrone>();

    private GameObject exitTrailSpawnPoint;
    public GameObject exitTrailPrefab;

    float lastSpawnTimestamp;
    float freqHintSpawn;
    private GameObject player;

    private void VerifyEnemies()
    {
        foreach (var human in enemyHumans)
        {
            if (human.npcAlive)
            {
                allEnemiesKilled = false;
            }
        }

        if (allEnemiesKilled)
        {
            foreach (var drone in enemyDrones)
            {
                if (drone.npcAlive)
                {
                    allEnemiesKilled = false;
                }
            }
        }

        if (allEnemiesKilled)
        {
            foreach (var tower in enemyTowers)
            {
                if (tower.isAlive)
                {
                    allEnemiesKilled = false;
                }
            }
        }

        if(bossLevel)
        {
            if (allEnemiesKilled)
            {
                foreach (var boss in enemyBoss)
                {
                    if (boss.npcAlive)
                    {
                        allEnemiesKilled = false;
                    }
                }

            }

            if (allEnemiesKilled)
            {
                foreach (var companion in enemyDronesCompanions)
                {
                    if (companion.npcAlive)
                    {
                        allEnemiesKilled = false;
                    }
                }
            }
        }

    }

    public void AddCompanion(SensePlayerDrone companion)
    {
        enemyDronesCompanions.Add(companion);
    }
    

    void ActivateExitZone()
    {
        exitZone.SetActive(true);

        if (!tutorial_store_Level)
        {
            if (Time.time > lastSpawnTimestamp)
            {
                lastSpawnTimestamp = Time.time + freqHintSpawn;


                if (player.GetComponent<NavMeshAgent>().isActiveAndEnabled)
                {
                    GameObject newHintTrail = Instantiate(exitTrailPrefab, exitTrailSpawnPoint.transform.position, exitTrailSpawnPoint.transform.rotation);
                    newHintTrail.GetComponent<HintTrailController>().SetTarget(exitZone);

                }
            }
        }
    }

    //:: Camera ::
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private CinemachineVirtualCamera vcam;
    private CinemachineTransposer transposer;
    private Vector3 sceneZoomOut;
    private Vector3 sceneZoomIn;

    void Start()
    {
        GameManager.Instance.SetSceneVariables();
        vcam = GameObject.Find("CMvcam").GetComponent<CinemachineVirtualCamera>();
        transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();

        sceneZoomIn = transposer.m_FollowOffset;
        sceneZoomOut = transposer.m_FollowOffset * 1.25f;

        GameManager.Instance.zoomOutOffset = sceneZoomOut;
        GameManager.Instance.zoomInOffset = sceneZoomIn;

        allEnemiesKilled = false;

        freqHintSpawn = 1.75f;
        lastSpawnTimestamp = Time.time;

        exitTrailSpawnPoint = GameObject.Find("exitTrailSpawnPoint");
        player = GameObject.Find("Player");

        exitZone = GameObject.Find("FX_ExitPoint");
        exitZone.SetActive(false);
    }


    public GameObject healingPrefab;

    private void Update()
    {

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(healingPrefab, player.transform.position, player.transform.rotation);
        }

        if (!tutorial_store_Level)
        {
            allEnemiesKilled = true;
            VerifyEnemies();
            
            if(allEnemiesKilled)
            {
                ActivateExitZone();
            }
        }
    }
}


