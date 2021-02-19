using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SensePlayer : MonoBehaviour
{
    bool playerInsight;
    public bool npcMovement;

    public bool npcAlive;


    GameObject player;
    public GameObject myself;

    public GameObject weaponEquiped;
    public WeaponController weaponController;

    public Animator animator;

    public NavMeshAgent navMeshAgent;
    public Vector3 destination;   

    private Ray rayPlayerInsight;
    bool m_HitDetect;
    private RaycastHit hitPlayerInsight;
    public GameObject bulletSpawn;

    [Header("Configuration")]
    public float shootDistance;
    public float normalSpeed;
    public float engageSpeed;

    // Start is called before the first frame update
    void Start()
    {
        playerInsight = false;
        npcMovement = true;
        npcAlive = true;


        player = GameObject.FindGameObjectWithTag("Player");

        GetComponent<SphereCollider>().radius = shootDistance;
        navMeshAgent.stoppingDistance = shootDistance / 2;

        navMeshAgent.speed = normalSpeed;
        navMeshAgent.updateRotation = false;
    }


    void OnDrawGizmos()
    {
        //Check if there has been a hit yet
        if (m_HitDetect)
        {
            Gizmos.color = Color.green;
            //Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(bulletSpawn.transform.position, bulletSpawn.transform.forward * hitPlayerInsight.distance);
            //Draw a cube that extends to where the hit exists
            Gizmos.DrawWireCube(bulletSpawn.transform.position + bulletSpawn.transform.forward * hitPlayerInsight.distance, new Vector3(0.25f, 0.25f, 0.25f));
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            Gizmos.color = Color.red;
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(bulletSpawn.transform.position, bulletSpawn.transform.forward * shootDistance);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(bulletSpawn.transform.position + bulletSpawn.transform.forward * shootDistance, new Vector3(0.25f, 0.25f, 0.25f));
        }
    }

    private void DoMovement()
    {
        //Set destination
        //destination = player.transform.position;

        //Look at player
        //myself.transform.LookAt(player.transform.position, Vector3.up);
        Orientacion();

        //Add 45euler grad to fix animation rotations.
        //myself.transform.Rotate(Vector3.up, 45f);

        //:: MOVE ENEMY ::
        navMeshAgent.destination = destination;

        //:: DO ANIMATIONS ::
        animator.SetFloat("SpeedCharacterAnimation", navMeshAgent.velocity.magnitude);
    }


    private void DoShooting()
    {
        //Look at player
        //myself.transform.LookAt(player.transform.position, Vector3.up);
        Orientacion();

        //Add 45euler grad to fix animation rotations.
        //myself.transform.Rotate(Vector3.up, 45f);

        m_HitDetect = Physics.BoxCast(bulletSpawn.transform.position, new Vector3(0.25f, 0.25f, 0.25f), bulletSpawn.transform.forward, out hitPlayerInsight, Quaternion.identity, shootDistance);

        if (m_HitDetect)
        {
            if (hitPlayerInsight.transform.tag == "Player")
            {
                weaponController.Shoot(false);
            }
        }


        //hitPlayerInsight = Physics.BoxCastAll(bulletSpawn.transform.position, new Vector3(0.5f, 0.5f, 0.5f), bulletSpawn.transform.forward, Quaternion.identity, shootDistance);
        //hitPlayerInsight = Physics.RaycastAll(myself.transform.position, bulletSpawn.transform.forward, shootDistance);


        //for (int i=0; i < hitPlayerInsight.Length; i++)
        //{
        //    Debug.Log(i + ":" + hitPlayerInsight[i].transform.name);
        //    if (i== hitPlayerInsight.Length-1 && hitPlayerInsight[i].transform.tag == "Player")
        //    {

        //        weaponController.Shoot(false);
        //    }
        //}
    }

    float timeCount = 0.3f;
    float actualTime;
    void SetDestination()
    {
        destination = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (npcAlive)
        {
            if (GameManager.Instance.isPlayerAlive)
            {
                actualTime += Time.deltaTime;
                if (actualTime > timeCount)
                {
                    actualTime = 0;
                    SetDestination();
                }

                
                if (npcMovement)
                {
                    DoMovement();
                }

                if (playerInsight)
                {
                    DoShooting();
                }
                
            }
            else
            {
                navMeshAgent.velocity = Vector3.zero;
                navMeshAgent.SetDestination(transform.position);

                //:: DO ANIMATIONS ::
                animator.SetFloat("SpeedCharacterAnimation", navMeshAgent.velocity.magnitude);
            }
        }


       

    }

    float yValue;
    //public Text textPre;
    //public Text textPost;
    //public bool sinTexto = true;
    void Orientacion()
    {
        //if (!sinTexto)
        //{
        //    textPre.text = "textPre: " + myself.transform.rotation.eulerAngles;
        //}
        //myself.transform.rotation = Quaternion.Euler(Vector3.zero);
        myself.transform.LookAt(player.transform.position, Vector3.up);
        yValue = myself.transform.rotation.eulerAngles.y;
        myself.transform.rotation = Quaternion.Euler(new Vector3(0, yValue, 0));
        myself.transform.Rotate(Vector3.up, 45f);
        //if (!sinTexto)
        //{
        //    textPost.text = "textPost: " + myself.transform.rotation.eulerAngles;
        //}
    }

    

    private void LateUpdate()
    {
        //if (!sinTexto)
        //{
        //    textPre.text = "textPre: " + myself.transform.rotation.ToString();
        //}
        //myself.transform.rotation = Quaternion.identity;
        //myself.transform.LookAt(player.transform.position, Vector3.up);
        //myself.transform.Rotate(Vector3.up, 45f);
        //if(!sinTexto)
        //{
        //    textPost.text = "textPost: " + myself.transform.rotation.ToString();
        //}
        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerInsight = true;
            
            navMeshAgent.speed = engageSpeed;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerInsight = false;
            
            navMeshAgent.speed = normalSpeed;
        }
        
    }
}
