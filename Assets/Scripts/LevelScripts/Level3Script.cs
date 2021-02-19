using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Script : MonoBehaviour
{
    public enum STAGE
    {
        FASE1,
        FASE2,
        FASE3,
        DEATH
    }
    public STAGE bossStage;
    

    [Header("Barriers")]
    private float timeBarriers;
    public float freqChangeBarriersInit;
    private float freqChangeBarriers;
    public BarrierAutoController[] barriersAuto;


    [Header("SuportTower")]
    private GameObject shieldDome;
    private ParticleSystem particleDomeFX;
    public HPCounterController towerSupport;

    [Header("Bombs")]
    public GameObject launchFX;
    public GameObject bombFX;
    public GameObject bombCheatFX;
    public GameObject markZoneFX;
    private float timeFireBombs;
    public float freqFireBombs;
    public float timeBetweenLaunches;
    public ParticleSystem launch1;
    public ParticleSystem launch2;
    public ParticleSystem launch3;
    public ParticleSystem launch4;
    private bool launched4;
    private bool launched3;
    private bool launched2;
    private bool launched1;
    private GameObject player;

    [Header("Boss")]
    public WeaponController bossWeapon;
    public Weapon_Rifle stage2Weapon;
    public Weapon_Rifle stage3Weapon;
    private ParticleSystem damageFase2FX;
    private ParticleSystem damageFase3FX;
    private ParticleSystem damageFaseChangeFX;
    private ParticleSystem weaponFaseChangeFX;

    [Header("Enemigos")]
    public SceneController sceneController;
    public ParticleSystem HealEffect01;
    public GameObject HealthPickup01;
    public ParticleSystem HealEffect02;
    public GameObject HealthPickup02;
    public ParticleSystem HealEffect03;
    public GameObject HealthPickup03;
    public GameObject droneSmallPrefab;
    public GameObject droneBigPrefab;
    private ParticleSystem spawnPointA;
    private ParticleSystem spawnPointB;
    private ParticleSystem spawnPointC;
    private SensePlayerDrone foeA;
    private SensePlayerDrone foeB;
    private SensePlayerDrone foeC;
    private float timeEnemies;
    public float freqSpawnEnemiesInit;
    private float freqSpawnEnemies;
    public GameObject prefabHeal;
    public GameObject prefabEnemyTurret;
    public GameObject prefabEnemyShotgun;



    private void ChangeBarriers()
    {
        foreach (BarrierAutoController barrier in barriersAuto)
        {
            if(Random.Range(0, 2) == 0)
            {
                barrier.ChangePositionBarrier();
            }
        }
    }

    public void SetupFase2()
    {
        bossStage = STAGE.FASE2;

        SoundManager.Instance.PlaySfx("BossFase2");
        if(SoundManager.Instance.SoundIsPlaying(Utils.AudioType.SFX, "deployDroneBoss"))
        {
            SoundManager.Instance.StopAudioByName(Utils.AudioType.SFX, "deployDroneBoss");
        }
        //SoundManager.Instance.PlaySfx("shieldDomeDownBoss");
        SoundManager.Instance.PlaySfxVolumePercent("shieldDomeDownBoss", volumeUp);

        damageFaseChangeFX.Play();
        weaponFaseChangeFX.Play();

        SoundManager.Instance.PlayRandomSfx(Utils.SFX.EXPLOSION);
        

        freqChangeBarriers = (freqChangeBarriersInit / 3) * 2;
        freqSpawnEnemies = (freqSpawnEnemiesInit / 3) * 2;
        
        //Bajan escudos de la torre
        shieldDome.SetActive(false);
        towerSupport.invencible = false;
        particleDomeFX.Stop();

        //Cambia arma
        bossWeapon.ChangeWeapon(stage2Weapon);

        //FeedbackVisual
        damageFase2FX.Play();
    }

    public void SetupFase3()
    {
        bossStage = STAGE.FASE3;
        SoundManager.Instance.PlaySfx("BossFase3");

        damageFaseChangeFX.Play();
        weaponFaseChangeFX.Play();

        SoundManager.Instance.PlayRandomSfx(Utils.SFX.EXPLOSION);


        freqChangeBarriers = freqChangeBarriersInit / 3;
        freqSpawnEnemies = freqSpawnEnemiesInit / 3;

        //Cambia arma        
        bossWeapon.ChangeWeapon(stage3Weapon);

        //FeedbackVisual
        damageFase3FX.Play();
    }

    public void SetupDeath()
    {
        bossStage = STAGE.DEATH;

        if (SoundManager.Instance.SoundIsPlaying(Utils.AudioType.SFX, "deployDroneBoss"))
        {
            SoundManager.Instance.StopAudioByName(Utils.AudioType.SFX, "deployDroneBoss");
        }
        if (SoundManager.Instance.SoundIsPlaying(Utils.AudioType.SFX, "missileLaunchBoss")) 
        {
            SoundManager.Instance.StopAudioByName(Utils.AudioType.SFX, "missileLaunchBoss");
        }

        //SoundManager.Instance.PlaySfx("bossDeath");
        SoundManager.Instance.PlaySfxVolumePercent("bossDeath", volumeUp);

        damageFaseChangeFX.Play();

        SoundManager.Instance.PlayRandomSfx(Utils.SFX.EXPLOSION);

        if (foeA.npcAlive)
        {
            sceneController.AddCompanion(foeA);
        }

        if (foeB.npcAlive)
        {
            sceneController.AddCompanion(foeB);
        }

        if (foeC.npcAlive)
        {
            sceneController.AddCompanion(foeC);
        }

    }


    private void SpawnEnemyByFase()
    {
        //SoundManager.Instance.PlaySfx("deployDroneBoss");
        SoundManager.Instance.PlaySfxVolumePercent("deployDroneBoss", volumeUp);

        switch (bossStage)
        {
            case STAGE.FASE1:
                SpawnEnemyA(droneSmallPrefab);
                SpawnEnemyB(droneSmallPrefab);
                SpawnEnemyC(droneSmallPrefab);
                break;
            case STAGE.FASE2:
                SpawnEnemyA(droneSmallPrefab);
                SpawnEnemyB(droneSmallPrefab);
                SpawnEnemyC(droneBigPrefab);
                break;
            case STAGE.FASE3:
                SpawnEnemyA(droneBigPrefab);
                SpawnEnemyB(droneBigPrefab);
                SpawnEnemyC(droneSmallPrefab);
                break;
        }
    }

    
    private void SpawnEnemyA(GameObject foePrefab)
    {        
        if (!foeA.npcAlive)
        {
            if (HealthPickup01 == null)
            {
                HealEffect01.Play();
                HealthPickup01 = Instantiate(prefabHeal, HealEffect01.transform.position, Quaternion.identity);
            }

            spawnPointA.Play();
            GameObject newEnemyA = Instantiate(foePrefab, spawnPointA.transform.position, spawnPointA.transform.rotation);
            foeA = newEnemyA.GetComponentInChildren<SensePlayerDrone>();
        }   
    }
    
    private void SpawnEnemyB(GameObject foePrefab)
    {
       
        if (!foeB.npcAlive)
        {
            if (HealthPickup02 == null)
            {
                HealEffect02.Play();
                HealthPickup02 = Instantiate(prefabHeal, HealEffect02.transform.position, Quaternion.identity);
            }

            spawnPointB.Play();
            GameObject newEnemyB = Instantiate(foePrefab, spawnPointB.transform.position, spawnPointB.transform.rotation);
            foeB = newEnemyB.GetComponentInChildren<SensePlayerDrone>();
        }
    }

    
    private void SpawnEnemyC(GameObject foePrefab)
    {
    
        if (!foeC.npcAlive)
        {
            if (HealthPickup03 == null)
            {
                HealEffect03.Play();
                HealthPickup03 = Instantiate(prefabHeal, HealEffect03.transform.position, Quaternion.identity);
            }

            spawnPointC.Play();
            GameObject newEnemyC = Instantiate(foePrefab, spawnPointC.transform.position, spawnPointC.transform.rotation);
            foeC = newEnemyC.GetComponentInChildren<SensePlayerDrone>();
        }
    
    }

    float volumeUp = 2f;
    IEnumerator LaunchBomb(ParticleSystem trail)
    {
        //LAUNCH TRAIL
        SoundManager.Instance.PlaySfx("Trail01");
        trail.Stop();
        trail.Play();
        yield return new WaitForSeconds(3);

        //Mark Explosion
        Vector3 explosionPoint = player.transform.position;
        GameObject explosionZone = Instantiate(markZoneFX, explosionPoint + Vector3.up, Quaternion.identity);
        yield return new WaitForSeconds(1.25f);

        //Drop bomb
        GameObject bombDrop = Instantiate(bombFX, explosionPoint + Vector3.up*18, Quaternion.identity);
        bombDrop.transform.LookAt(explosionZone.transform.position, Vector3.up);
        bombDrop.GetComponent<ParticleSystem>().Play();
        Destroy(explosionZone, 4f);
        Destroy(bombDrop, 4f);
    }

    private void Start()
    {
        freqSpawnEnemies= freqSpawnEnemiesInit;
        freqChangeBarriers = freqChangeBarriersInit;

        timeBarriers = freqChangeBarriers;
        timeEnemies = freqSpawnEnemies;
        timeFireBombs = freqFireBombs;

        launched1 = false;
        launched2 = false;
        launched3 = false;
        launched4 = false;

        spawnPointA = GameObject.Find("spawnPointA").GetComponent<ParticleSystem>();
        spawnPointB = GameObject.Find("spawnPointB").GetComponent<ParticleSystem>();
        spawnPointC = GameObject.Find("spawnPointC").GetComponent<ParticleSystem>();

        damageFase2FX = GameObject.Find("DamageFase2FX").GetComponent<ParticleSystem>();
        damageFase3FX = GameObject.Find("DamageFase3FX").GetComponent<ParticleSystem>();
        damageFaseChangeFX = GameObject.Find("damageFaseChange").GetComponent<ParticleSystem>();
        weaponFaseChangeFX = GameObject.Find("FX_weaponFaseChange").GetComponent<ParticleSystem>();

        shieldDome = GameObject.Find("shieldDome");
        particleDomeFX = GameObject.Find("feedbackDome").GetComponent<ParticleSystem>();

        bossStage = STAGE.FASE1;

        SoundManager.Instance.StopAllAudios();
        //SoundManager.Instance.PlayMusic("Ambient");
        SoundManager.Instance.PlayMusicPercent("Ambient", 0.35f);
        //SoundManager.Instance.PlayMusic("BossFight01");

        spawnPointA.Play();
        GameObject newEnemyA = Instantiate(droneSmallPrefab, spawnPointA.transform.position, spawnPointA.transform.rotation);
        foeA = newEnemyA.GetComponentInChildren<SensePlayerDrone>();

        spawnPointB.Play();
        GameObject newEnemyB = Instantiate(droneSmallPrefab, spawnPointB.transform.position, spawnPointB.transform.rotation);
        foeB = newEnemyB.GetComponentInChildren<SensePlayerDrone>();     

        spawnPointC.Play();
        GameObject newEnemyC = Instantiate(droneSmallPrefab, spawnPointB.transform.position+Vector3.forward*2, spawnPointB.transform.rotation);
        foeC = newEnemyC.GetComponentInChildren<SensePlayerDrone>();

        player = GameObject.Find("Player");

    }



    private void MoveBarriers()
    {
        timeBarriers += Time.deltaTime;

        if (timeBarriers >= freqChangeBarriers)
        {
            timeBarriers = 0f;
            ChangeBarriers();
        }
    }


    private void SpawnEnemies()
    {

        if (!foeA.npcAlive || !foeB.npcAlive || !foeC.npcAlive)
        {
            timeEnemies += Time.deltaTime;

            if (timeEnemies >= freqSpawnEnemies)
            {
                timeEnemies = 0f;
                SpawnEnemyByFase();
            }
        }
    }

    private void LaunchMissiles()
    {
        //LaunchMisilles
        timeFireBombs += Time.deltaTime;

        if (timeFireBombs >= freqFireBombs)
        {
            if(!launched1 && !launched2 && !launched3 && !launched4)
            {
                if (!SoundManager.Instance.SoundIsPlaying(Utils.AudioType.SFX, "deployDroneBoss"))
                {
                    //SoundManager.Instance.PlaySfx("missileLaunchBoss");
                    SoundManager.Instance.PlaySfxVolumePercent("missileLaunchBoss", volumeUp);
                }
            }

            if (timeFireBombs >= freqFireBombs + timeBetweenLaunches * 3 && !launched4)
            {
                launched4 = true;
                StartCoroutine(LaunchBomb(launch4));
                //LaunchBomb(launch4);
            }
            else if (timeFireBombs >= freqFireBombs + timeBetweenLaunches * 2 && !launched3)
            {
                launched3 = true;
                StartCoroutine(LaunchBomb(launch3));
                //LaunchBomb(launch3);
            }
            else if (timeFireBombs >= freqFireBombs + timeBetweenLaunches * 1 && !launched2)
            {
                launched2 = true;
                StartCoroutine(LaunchBomb(launch2));
                //LaunchBomb(launch2);
            }
            else if (timeFireBombs >= freqFireBombs && !launched1)
            {
                launched1 = true;
                StartCoroutine(LaunchBomb(launch1));
                //LaunchBomb(launch1);
            }
            else if (launched1 && launched2 && launched3 && launched4)
            {
                launched1 = false;
                launched2 = false;
                launched3 = false;
                launched4 = false;
                timeFireBombs = 0f;
            }

        }
    }

    private void CheatDamageBoss()
    {
        GameObject bombCheat = Instantiate(bombCheatFX, transform.position + Vector3.up * 18, Quaternion.identity);
        bombCheat.transform.LookAt(transform.position, Vector3.up);
        bombCheat.GetComponent<ParticleSystem>().Play();
        Destroy(bombCheat, 4f);
    }

    private void Update()
    {
        //CHEATS
        if (Input.GetKeyDown(KeyCode.C))
        {
            CheatDamageBoss();
        }

        //MOVIMIENTO ALEATORIO BARRERAS
        MoveBarriers();

        if (bossStage != STAGE.DEATH)
        {
            //SPAWN ENEMIGOS
            SpawnEnemies();

            //LAUNCH MISSILES
            if (bossStage == STAGE.FASE3)
            {
                LaunchMissiles();
            }
        }

        if (!SoundManager.Instance.SoundIsPlaying(Utils.AudioType.MUSIC, "BossFight01"))
        {
            SoundManager.Instance.PlayMusicPercent("BossFight01", 0.35f);
        }
    }
}
