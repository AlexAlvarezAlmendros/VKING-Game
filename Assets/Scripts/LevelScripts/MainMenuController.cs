using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    public GameObject sprayParticles;
    private GameObject sprayPointPosition;


    public GameObject OptionsMenu;
    public GameObject CreditsMenu;
    public GameObject PlayMenu;

    
    public void GoToOptions()
    {
        SpawnSpray();


        PlayMenu.SetActive(false);
        CreditsMenu.SetActive(false);

        OptionsMenu.SetActive(true);

    }

    public void GoToCredits()
    {
        SpawnSpray();

        OptionsMenu.SetActive(false);
        PlayMenu.SetActive(false);

        CreditsMenu.SetActive(true);
    }

    public void GoToPlayGame()
    {
        SpawnSpray();

        OptionsMenu.SetActive(false);
        CreditsMenu.SetActive(false);

        PlayMenu.SetActive(true);
    }

    public void LoadTutorial()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(3);
    }

    public void GoToExit()
    {
        Application.Quit();
    }

    public void SpawnSpray()
    {
        GameObject newSpray = Instantiate(sprayParticles, sprayPointPosition.transform.position, sprayPointPosition.transform.rotation, sprayPointPosition.transform);
        Destroy(newSpray, 3f);
        SoundManager.Instance.PlaySfx("SprayPaint");
    }

    private Slider masterSlider;
    private Slider musicSlider;
    private Slider fxSlider;


    private void initAudioOptions()
    {
        float masterVal = masterSlider.value;
        float musicVal = musicSlider.value;
        float sfxVal = fxSlider.value;


        SoundManager.Instance.SetAllVolume(masterVal, musicVal, sfxVal, sfxVal);
    }

    public void OnMasterChange()
    {
        SoundManager.Instance.SetMasterVolume(masterSlider.value);
    }

    public void OnMusicChange()
    {
        SoundManager.Instance.SetMusicVolume(musicSlider.value);
    }

    public void OnFXChange()
    {
        SoundManager.Instance.SetSfxVolume(fxSlider.value);
    }


    public void OnMouseOverSound()
    {
        SoundManager.Instance.PlaySfx("SprayShake");
    }


    private void Start()
    {
        sprayPointPosition = GameObject.Find("SprayPointSpawn");    

        masterSlider = GameObject.Find("masterSlider").GetComponent<Slider>();
        musicSlider = GameObject.Find("musicSlider").GetComponent<Slider>();
        fxSlider = GameObject.Find("fxSlider").GetComponent<Slider>();
        PlayMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        initAudioOptions();

        SoundManager.Instance.StopAllAudios();  
    }

    private void Update()
    {
        if (!SoundManager.Instance.SoundIsPlaying(Utils.AudioType.MUSIC, "MainMenu"))
        {
            SoundManager.Instance.PlayMusic("MainMenu");
        }
    }

}
