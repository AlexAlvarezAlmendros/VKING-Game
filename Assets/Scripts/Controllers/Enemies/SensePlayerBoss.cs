using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensePlayerBoss : MonoBehaviour
{
    bool playerInsight;
    public bool npcAlive;


    GameObject player;
    public GameObject myself;

    public GameObject weaponEquiped;
    public WeaponController weaponController;



    //private Ray rayPlayerInsight;
    bool m_HitDetect;
    private RaycastHit hitPlayerInsight;
    public GameObject bulletSpawn;

    [Header("Configuration")]
    public float shootDistance;


    // Start is called before the first frame update
    void Start()
    {
        playerInsight = true;
        npcAlive = true;


        player = GameObject.FindGameObjectWithTag("Player");
        GetComponent<SphereCollider>().radius = shootDistance;

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



    private void DoShooting()
    {
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
    }

    
    //public Text textPre;
    //public Text textPost;
    //public bool sinTexto = true;

    float xValue;
    float yValue;
    float zValue;
    void Orientacion()
    {

        //if (!sinTexto)
        //{
        //    textPre.text = "textPre: " + myself.transform.rotation.eulerAngles;
        //}

        //myself.transform.rotation = Quaternion.Euler(Vector3.zero);
        //myself.transform.LookAt(player.transform.position, transform.up);

        
        //transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        //transform.forward = direccion;

        Vector3 direccion = (player.transform.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direccion);
        //float x = rotation.eulerAngles.x;
        //transform.forward = direccion;
        //float y = transform.rotation.eulerAngles.y;
        //float z = transform.rotation.eulerAngles.z;
        //transform.rotation = Quaternion.Euler(new Vector3(x, y, z));
        transform.rotation = rotation;


        //rotation.forward = direction
        //salvas y, salvas z
        //transform.rotation = euler(x, y_salvada, z_salvada)

        /*
         * SETEAR AL ANGULO
         * 1- CALCULAR PITCH DEL ANGULO X
         * - DIFERENCIA ALTURA ENTRE TO
         */


        //yValue = myself.transform.rotation.eulerAngles.y;
        //myself.transform.rotation = Quaternion.Euler(new Vector3(0, yValue, 0));
        //myself.transform.Rotate(Vector3.up, 45f);

        //if (!sinTexto)
        //{
        //    textPost.text = "textPost: " + myself.transform.rotation.eulerAngles;
        //}
    }

    private void FixedUpdate()
    {
        if (npcAlive)
        {
            if (GameManager.Instance.isPlayerAlive)
            {
                //Look at player
                Orientacion();
                //myself.transform.LookAt(player.transform.position, Vector3.up);

                if (playerInsight)
                {
                    DoShooting();
                }
            }
        }
    }
}
