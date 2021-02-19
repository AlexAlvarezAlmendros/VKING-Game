using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1_2Script : MonoBehaviour
{
    public GameObject canvasInfo;

    public GameObject canvasToBuy;
    public bool partyShotgunAdquired = false;
    public bool vikingShieldAdquired = false;
    private bool standingPoint = false;
    int idLevel = 5;

    

    void Start()
    {
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
                if(partyShotgunAdquired)
                {
                    GameManager.Instance.partyShotgun = partyShotgunAdquired;
                    GameManager.Instance.basicRifle = false;
                    GameManager.Instance.pewpewRifle = false;
                }

                if (vikingShieldAdquired)
                {
                    GameManager.Instance.vikingShield = vikingShieldAdquired;
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
