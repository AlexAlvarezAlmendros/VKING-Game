using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsFinale : MonoBehaviour
{
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void GoToExit()
    {
        Application.Quit();
    }


    public void OnMouseOverSound()
    {
        SoundManager.Instance.PlaySfx("SprayShake");
    }

    private void Start()
    {
        SoundManager.Instance.StopAllAudios();
    }



    void Update()
    {
        if (!SoundManager.Instance.SoundIsPlaying(Utils.AudioType.MUSIC, "MainMenu"))
        {
            SoundManager.Instance.PlayMusic("MainMenu");
        }
    }
}
