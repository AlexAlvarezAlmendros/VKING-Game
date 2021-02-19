using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SensePlayerDrone : MonoBehaviour
{
    bool playerInsight;
    public bool npcMovement;

    public bool npcAlive;


    GameObject player;
    public GameObject myself;


    public NavMeshAgent navMeshAgent;
    public Vector3 destination;


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
        //myself.transform.Rotate(Vector3.up, 45f);
        //if (!sinTexto)
        //{
        //    textPost.text = "textPost: " + myself.transform.rotation.eulerAngles;
        //}
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
            Gizmos.DrawWireCube(bulletSpawn.transform.position + bulletSpawn.transform.forward * hitPlayerInsight.distance, new Vector3(0.1f, 0.1f, 0.1f));
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            Gizmos.color = Color.red;
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(bulletSpawn.transform.position, bulletSpawn.transform.forward * shootDistance);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(bulletSpawn.transform.position + bulletSpawn.transform.forward * shootDistance, new Vector3(0.1f, 0.1f, 0.1f));
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
    }


    private void DoShooting()
    {
        //Look at player
        //myself.transform.LookAt(player.transform.position, Vector3.up);
        Orientacion();
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
            }
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInsight = true;
            //Debug.Log(other.gameObject.name + " enters range");
            navMeshAgent.speed = engageSpeed;
        }

    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInsight = false;
            //Debug.Log(other.gameObject.name + " exists range");
            navMeshAgent.speed = normalSpeed;
        }

    }
}
