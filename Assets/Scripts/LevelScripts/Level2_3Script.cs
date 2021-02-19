using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2_3Script : MonoBehaviour
{
    public GameObject canvasInfo;

    public GameObject canvasToBuy;
    public bool pewpewRifleAdquired = false;
    public bool pr1n7moduleAdquired = false;
    private bool standingPoint = false;
    int idLevel = 7;

    

    void Start()
    {
        idLevel = 7;
        SoundManager.Instance.StopAllAudios();
        SoundManager.Instance.PlayMusic("Ambient");

        canvasToBuy.SetActive(false);
    }

    private void Update()
    {
        if(canvasToBuy.activeSelf && standingPoint)
        {
            if (Input.GetButtonDown("Use"))
            {
                if(pewpewRifleAdquired)
                {
                    GameManager.Instance.pewpewRifle = pewpewRifleAdquired;
                    GameManager.Instance.basicRifle = false;
                    GameManager.Instance.partyShotgun = false;
                }

                if (pr1n7moduleAdquired)
                {
                    GameManager.Instance.pr1n7Module = pr1n7moduleAdquired;
                }

                SceneManager.LoadScene(idLevel);
                
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            SoundManager.Instance.PlayRandomSfx(Utils.SFX.DRONETALK);
            canvasToBuy.SetActive(true);
            canvasInfo.SetActive(true);
            standingPoint = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            canvasToBuy.SetActive(false);
            canvasInfo.SetActive(false);
            standingPoint = false;
        }
    }
}
