using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SupportTowerController : MonoBehaviour
{

    public bool isAlive;
    
    public GameObject powerUpTrail;
    private float lastSpawnTimestamp;
    public float freqPowerUpSpawn;
    
    public GameObject[] targets;
    private bool setupBossReference;
    private SensePlayerBoss boss;

    void SetupBossReferenceBrain()
    {
        boss = GameObject.Find("Cannon_SensePlayerBoss").GetComponent<SensePlayerBoss>();
    }

    void Start()
    {
        isAlive = true;
        lastSpawnTimestamp = Time.time;
        setupBossReference = false;
    }

    
    void Update()
    {
        if (isAlive)
        {
            if (Time.time > lastSpawnTimestamp)
            {
                lastSpawnTimestamp = Time.time + freqPowerUpSpawn;

                foreach (var target in targets)
                {
                    if(target.gameObject.tag == "Boss")
                    {
                        if (!setupBossReference)
                        {
                            setupBossReference = true;
                            SetupBossReferenceBrain();
                        }

                        if (boss.npcAlive)
                        {
                            GameObject powerUpTrialObject = Instantiate(powerUpTrail, transform.position, transform.rotation, transform);
                            powerUpTrialObject.name = "PowerUpTrial_target-" + target.name;
                            powerUpTrialObject.GetComponent<PowerUpTrail>().SetTarget(target);
                        }
                    }
                    else if(target.GetComponent<NavMeshAgent>().isActiveAndEnabled)
                    {
                        GameObject powerUpTrialObject = Instantiate(powerUpTrail, transform.position, transform.rotation, transform);
                        powerUpTrialObject.name = "PowerUpTrial_target-" + target.name;
                        powerUpTrialObject.GetComponent<PowerUpTrail>().SetTarget(target);
                        
                        //Destroy(powerUpTrail, 30f);
                    }
                }
            }
        }
    }
}
