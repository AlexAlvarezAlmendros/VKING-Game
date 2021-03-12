using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class HPCounterController : MonoBehaviour
{

    public bool invencible;
    public GameObject healthPointPrefab;
    public int initHealthPoints;
    public int actualDamage;
    List<GameObject> healthPointsList = new List<GameObject>();

    public GameObject rootBody;
    public Rigidbody rb;
    public GameObject parentObject;
    public Collider mainCollider;
    public Collider mainColliderRagdoll;
    private Collider[] allColliders;
    private Rigidbody[] allRigidbodies;

    public Animator animator;
    public NavMeshAgent navMeshAgent;
    public Canvas canvasCharacter;

    public SHIELDCounterController shieldController;

    [Header("PlayerSpecific")]
    public bool isPlayer;

    [Header("EnemySpecific")]
    public SensePlayer enemyController;

    [Header("DroneSpecific")]
    public bool isDrone;
    public EnemyDroneMeleeVisuals droneVisuals;
    public SensePlayerDrone enemyDroneController;
    public GameObject weaponDrone;

    [Header("TowerSpecific")]
    public bool isTower;
    public SupportTowerVisuals supportTowerVisuals;
    public SupportTowerController supportTowerController;

    [Header("BossSpecific")]
    public bool isBoss;
    public SensePlayerBoss enemyBossController;
    public Level3Script levelBossController;
    


    //Llamar usando: StartCoroutine(SoundManager.Instance.PlayAudioAfterSeconds(Utils.SFX.xxxx,1.5f));
    public IEnumerator ActivateRagdollColliders(bool isRagdolled, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        foreach (var collider in allColliders)
        {
            collider.enabled = isRagdolled;
        }

    }

    private void ActivateRagdoll(bool isRagdolled)
    {    
        //Colliders

        //Desactivo capsula
        mainCollider.enabled = !isRagdolled;

        //Activo el resto de colliders
        foreach (var collider in allColliders)
        {
            collider.enabled = isRagdolled;
        }


        //Aplico fuerza
        if (isRagdolled)
        {
            parentObject.transform.position = parentObject.transform.position + Vector3.up * 0.15f;

            //if (rb.velocity.magnitude > 0)
            //{
            //    rb.velocity = Vector3.zero;
            //}

            rb.AddForce(parentObject.transform.up * 10, ForceMode.VelocityChange);
            rb.AddForce(parentObject.transform.forward * -4, ForceMode.VelocityChange);

        }


    }

    void Start()
    {
        if (isPlayer)
        {
            GameManager.Instance.loadCharacterPowerups();
            initHealthPoints = GameManager.Instance.initPlayerHP;
        }

        actualDamage = 0;
        
        int id = 0;
        for (int i = 0; i < initHealthPoints; i++)
        {
            GameObject newHealthPoint = Instantiate(healthPointPrefab, transform);
            id = i + 1;
            newHealthPoint.name = "HealthPoint_" + id;
            healthPointsList.Add(newHealthPoint);
        }

        if(!isDrone && !isTower && !isBoss)
        {
            allColliders = rootBody.GetComponentsInChildren<Collider>();
            
            //Colliders
            ActivateRagdoll(false);
        }      
        
    }


    public void ModifHealthHeal()
    {
        actualDamage--;
        int id = actualDamage;

        Image healthBar = healthPointsList[id].GetComponent<Image>();

        Color actualColor = healthBar.color;
        actualColor.a = 1f;

        healthBar.color = actualColor;
    }

    private void ModifyHealthAttack()
    {
        int id = actualDamage;
        actualDamage++;

        //MUERTEEEEEEEEEEEEEEEEEEEEEEEEE
        if (actualDamage == initHealthPoints)
        {
            //Colliders
            if (!isDrone && !isTower && !isBoss)
            {
                ActivateRagdoll(true);
                SoundManager.Instance.PlayRandomSfx(Utils.SFX.DEATHHUMAN);
            }
            else if (isDrone)
            {
                rb.AddForce(parentObject.transform.up * 10, ForceMode.VelocityChange);
                rb.AddForce(parentObject.transform.forward * -4, ForceMode.VelocityChange);
                SoundManager.Instance.PlayRandomSfx(Utils.SFX.DEATHDRONE);
            }

            //Componentes
            if (!isDrone && !isTower && !isBoss)
            {
                animator.enabled = false;
            }
            else
            {
                if (isDrone)
                {
                    droneVisuals.doMovementAnimation = false;
                }
                else if (isTower)
                {
                    supportTowerVisuals.doMovementAnimation = false;
                }
            }

            if (!isTower)
            {
                navMeshAgent.enabled = false;
            }
            canvasCharacter.enabled = false;

            //Registrar muerte
            if (isPlayer)
            {
                GameManager.Instance.PlayerKilled();
                GameManager.Instance.isPlayerAlive = false;
            }
            else
            {
                if (isDrone)
                {
                    enemyDroneController.npcAlive = false;
                    weaponDrone.SetActive(false);
                    droneVisuals.PlayDeathAnimation();
                }
                else if (isTower)
                {
                    supportTowerVisuals.DeactivateParticlesFX();
                    supportTowerVisuals.DoExplosionFX();
                    supportTowerVisuals.ActivateSmokeFX();
                    supportTowerController.isAlive = false;
                }
                else if(isBoss)
                {
                    enemyBossController.npcAlive = false;
                    levelBossController.SetupDeath();
                }
                else
                {
                    enemyController.npcAlive = false;
                }

            }

            return;
        }


        // DAÑO SUFRIDO

        //control de seguridad
        if (actualDamage > initHealthPoints)
        {
            return;
        }

        if (isTower)
        {
            if (actualDamage == 3)
            {
                supportTowerVisuals.ActivateDamageFX();
            }
            else if (actualDamage == 5)
            {
                supportTowerVisuals.ActivateBigDamageFX();
            }
        }


        if(isBoss)
        {
            if(actualDamage == 10)
            {
                levelBossController.SetupFase2();
            }
            else if (actualDamage == 20)
            {
                levelBossController.SetupFase3();
            }
        }


        if (isPlayer)
        {
            GameManager.Instance.SetDamageIndicator();
        }



        Image healthBar = healthPointsList[id].GetComponent<Image>();

        Color actualColor = healthBar.color;
        actualColor.a = 0.33f;

        healthBar.color = actualColor;
    }

    public void TakeMultipleDamage(int numberDamage)
    {
        for (int i = 0; i < numberDamage; i++)
        {
            if (actualDamage == initHealthPoints)
            {
                return;
            }
            ModifyHealth();
        }
        
    }

    public void ModifyHealth()
    {
        if (invencible)
        {
            return;
        }

        // ATTACK SHIELD ::::::::::::::::::::::::::::::::::::::::::::::::::::::::::: 
        if (!isTower && !isPlayer)
        {
            if(shieldController.hasShield)
            {
                SoundManager.Instance.PlaySfx("ShieldDamage01");
                shieldController.RemoveShieldPoint();
                return;
            }
        }

        // ATTACK HEALTH ::::::::::::::::::::::::::::::::::::::::::::::::::::::::::: 
        if (isBoss)
        {
            SoundManager.Instance.PlayRandomSfx(Utils.SFX.DAMAGEBOSS);
        }
        else if (!isTower && !isDrone)
        {
            SoundManager.Instance.PlayRandomSfx(Utils.SFX.DAMAGEHUMAN);
        }
        
        ModifyHealthAttack();
        
    }


    public IEnumerator ModifyHealthFire(int duration) 
    {
        int counter = 0;
        while (counter <= duration)
        {
            yield return new WaitForSeconds(1);
            ModifyHealth();
            counter++;
        }

    }

    public IEnumerator ModifyHealthWind(int pushback)
    {


        
        yield return null;
    }
    
}
