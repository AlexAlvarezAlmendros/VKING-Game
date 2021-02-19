using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;


//[System.Serializable]
public class Audio
{
    public string name;
    public AudioClip clip;
    public Utils.AudioType audioType;

    private AudioSource source;

    [Range(0.25f, 3f)]
    public float pitch = 1f;

    [Range(0f, 3f)]
    public float randomPitch = 0f;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
    }

    public void SetSourceVolume(float master, float volume)
    {
        source.volume = master;
        source.volume *= volume;
    }
    public void SetSourceVolume(float volume)
    {
        source.volume = volume;
    }

    public void Play()
    {
        if (randomPitch != 0) source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));

        //Al ser distintos AudioSource se pueden ejecutar a la vez, no es necesario PlayAtOnce()
        source.Play();
    }

    public void PlayForcePitch(float newPitch)
    {
        source.pitch = newPitch;
        source.Play();
    }

    public AudioSource GetAudioSource()
    {
        return source;
    }

    public void Stop()
    {
        source.Stop();
    }
}

[System.Serializable]
public class Music : Audio
{
    public Utils.Music musicType;
}

[System.Serializable]
public class SpecialEffect : Audio
{
    public Utils.SFX sfxType;
}

[System.Serializable]
public class Voice : Audio
{
    public Utils.Voice voiceType;
}

public class SoundManager : MonoBehaviour
{
    /*index
      #######################
      #     VARIABLES       #
      #   GETTERS/SETTERS   #
      #######################
    */

    private float masterVolume;
    private float sfxVolume;
    private float musicVolume;
    private float voiceVolume;

    public void SetMasterVolume(float newVolume)
    {
        masterVolume = newVolume;
        
        ApplySfxVolume();
        ApplyMusicVolume();
        ApplyVoiceVolume();
    }

    public void SetSfxVolume(float newVolume)
    {
        sfxVolume = newVolume;
        
        ApplySfxVolume();
    }
    private void ApplySfxVolume()
    {
        for (int i = 0; i < sfxClips.Length; i++)
        {
            sfxClips[i].SetSourceVolume(masterVolume, sfxVolume);
        }
    }

    public void SetMusicVolume(float newVolume)
    {
        musicVolume = newVolume;
        
        ApplyMusicVolume();
    }

    
    private void ApplyMusicVolume()
    {
        musicAudioSource.volume = masterVolume;
        musicAudioSource.volume *= musicVolume;

        for (int i = 0; i < musicClips.Length; i++)
        {
            musicClips[i].SetSourceVolume(masterVolume, musicVolume);
        }
    }


    public void SetVoiceVolumen(float newVolume)
    {
        voiceVolume = newVolume;
        
        ApplyVoiceVolume();
    }
    private void ApplyVoiceVolume()
    {
        for (int i = 0; i < voiceClips.Length; i++)
        {
            voiceClips[i].SetSourceVolume(masterVolume, voiceVolume);
        }
    }

    public void SetAllVolume(float masterNewVolume, float sfxNewVolume, float musicNewVolume, float voiceNewVolume)
    {
        masterVolume = masterNewVolume;
        
       
        sfxVolume = sfxNewVolume;
        ApplySfxVolume();
        
        musicVolume = musicNewVolume;
        ApplyMusicVolume();
        
        voiceVolume = voiceNewVolume;
        ApplyVoiceVolume();
    }

    public float GetMasterVolume() { return masterVolume; }
    public float GetSfxVolume() { return sfxVolume; }
    public float GetMusicVolume() { return musicVolume; }
    public float GetVoiceVolume() { return voiceVolume; }


    /*index
      ######################
      #   SOUND EFFECTS    #
      ######################
    */

    [SerializeField]
    public SpecialEffect[] sfxClips;


    public void PlaySfxForcePitch(string _name, float newPitch)
    {
        for (int i = 0; i < sfxClips.Length; i++)
        {
            if (sfxClips[i].name == _name)
            {
                sfxClips[i].PlayForcePitch(newPitch);
                return;
            }
        }
        Debug.LogWarning("AudioManager> SFX not found: " + _name);
        return;
    }

    public void PlaySfxOnce(string _name)
    {
        for (int i = 0; i < sfxClips.Length; i++)
        {
            if (sfxClips[i].name == _name)
            {
                sfxClips[i].Play();
                return;
            }
        }
        Debug.LogWarning("AudioManager> SFX not found: " + _name);
        return;
    }

    public void PlaySfx(string _name)
    {
        for (int i = 0; i < sfxClips.Length; i++)
        {
            if (sfxClips[i].name == _name)
            {
                sfxClips[i].Play();
                return;
            }
        }
        Debug.LogWarning("AudioManager> SFX not found: " + _name);
        return;
    }

    public void PlaySfxVolumePercent(string _name, float volumeUp)
    {
        for (int i = 0; i < sfxClips.Length; i++)
        {
            if (sfxClips[i].name == _name)
            {
                AudioSource audSource = sfxClips[i].GetAudioSource();
                
                float volumeInit = audSource.volume;
                Debug.Log(audSource.volume);
                audSource.volume *= volumeUp;
                Debug.Log(audSource.volume);
                sfxClips[i].Play();
                audSource.volume = volumeInit;
                return;
            }
        }
        Debug.LogWarning("AudioManager> SFX not found: " + _name);
        return;
    }

    public void PlaySfxLoop(string _name)
    {
        for (int i = 0; i < sfxClips.Length; i++)
        {
            if (sfxClips[i].name == _name)
            {
                if (!sfxClips[i].GetAudioSource().isPlaying)
                {
                    sfxClips[i].Play();
                }
                return;
            }
        }
    }

    //Llamar usando: StartCoroutine(SoundManager.Instance.PlayAudioAfterSeconds(Utils.SFX.xxxx,1.5f));
    public IEnumerator PlayRandomSfxAudioAfterSeconds(Utils.SFX specialEffect, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        PlayRandomSfx(specialEffect);

    }

    public void PlayRandomSfx(Utils.SFX sE)
    {

        List<Audio> randomList = new List<Audio>();

        for (int i = 0; i < sfxClips.Length; i++)
        {
            if (sfxClips[i].sfxType == sE)
            {
                randomList.Add(sfxClips[i]);
            }
        }
        
        if (randomList.Count == 0) return;
        randomList[Random.Range(0, randomList.Count)].Play();

    }


    /*index
      ######################
      #   VOICE LINES      #
      ######################
    */

    [SerializeField]
    public Voice[] voiceClips;

    public void PlayVoice(string _name)
    {
        for (int i = 0; i < voiceClips.Length; i++)
        {
            if (voiceClips[i].name == _name)
            {
                voiceClips[i].Play();
                return;
            }
        }
        Debug.LogWarning("AudioManager> VOICE not found: " + _name);
        return;
    }


    /*index
      ######################
      #   MUSIC CLIPS      #
      ######################
    */
    #region musicClips

    [SerializeField]
    public Music[] musicClips;

    public void PlayMusic(string _name)
    {
        for (int i = 0; i < musicClips.Length; i++)
        {
            if (musicClips[i].name == _name)
            {
                musicClips[i].Play();
                return;
            }
        }
        Debug.LogWarning("AudioManager> MUSIC not found: " + _name);
        return;
    }

    public void PlayMusicPercent(string _name, float volumeUp)
    {
        for (int i = 0; i < musicClips.Length; i++)
        {
            if (musicClips[i].name == _name)
            {
                AudioSource audSource = musicClips[i].GetAudioSource();

                float volumeInit = audSource.volume;
                Debug.Log(audSource.volume);
                audSource.volume *= volumeUp;
                Debug.Log(audSource.volume);
                musicClips[i].Play();
                audSource.volume = volumeInit;
                return;
            }
        }
        Debug.LogWarning("AudioManager> MUSIC not found: " + _name);
        return;
    }

    #endregion
    /*index
      #########################
      #   MANAGER FUNCTIONS   #
      #########################
    */
    #region managerFunctions


    //Llamar usando: StartCoroutine(SoundManager.Instance.PlayAudioAfterSeconds(Utils.AudioType.SFX,"audioSfx1",1.5f));
    public IEnumerator PlayAudioAfterSeconds(Utils.AudioType type, string name, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Debug.LogWarning("Fase2 sound:" + Time.deltaTime * 1000);
        PlayAudio(type, name);
    }

    public void PlayAudio(Utils.AudioType type, string name)
    {
        switch (type)
        {
            case Utils.AudioType.VOICE:
                PlayVoice(name);
                return;

            case Utils.AudioType.SFX:
                PlaySfx(name);
                return;

            case Utils.AudioType.MUSIC:
                PlayMusic(name);
                return;
        }

        Debug.LogWarning("AudioManager> AUDIO not found: " + name);
    }


    


    public void StopAllAudiosByType(Utils.AudioType type)
    {

        switch (type)
        {
            case Utils.AudioType.VOICE:
                for (int i = 0; i < sfxClips.Length; i++)
                {
                    sfxClips[i].Stop();
                }
                break;

            case Utils.AudioType.SFX:
                for (int i = 0; i < voiceClips.Length; i++)
                {
                    voiceClips[i].Stop();
                }
                break;

            case Utils.AudioType.MUSIC:
                for (int i = 0; i < musicClips.Length; i++)
                {
                    musicClips[i].Stop();
                }
                break;
        }
    }

    public void StopAllAudios()
    {
        for (int i = 0; i < sfxClips.Length; i++)
        {
            sfxClips[i].Stop();
        }

        for (int i = 0; i < voiceClips.Length; i++)
        {
            voiceClips[i].Stop();
        }

        for (int i = 0; i < musicClips.Length; i++)
        {
            musicClips[i].Stop();
        }

    }

    public void StopAudioByName(Utils.AudioType type, string _name)
    {

        switch (type)
        {
            case Utils.AudioType.VOICE:
                for (int i = 0; i < sfxClips.Length; i++)
                {
                    if (sfxClips[i].name == _name)
                    {
                        sfxClips[i].Stop();
                        return;
                    }
                    
                }
                break;
            case Utils.AudioType.SFX:
                for (int i = 0; i < voiceClips.Length; i++)
                {
                    if (voiceClips[i].name == _name)
                    {
                        voiceClips[i].Stop();
                        return;
                    }
                }
                return;

            case Utils.AudioType.MUSIC:
                for (int i = 0; i < musicClips.Length; i++)
                {
                    if (musicClips[i].name == _name)
                    {
                        musicClips[i].Stop();
                        return;
                    }
                }
                return;
        }

        Debug.LogWarning("AudioManager> AUDIO not found: " + _name);
        return;
    }

    

    public bool SoundIsPlaying(Utils.AudioType type, string name)
    {

        switch (type)
        {
            case Utils.AudioType.VOICE:
                for (int i = 0; i < voiceClips.Length; i++)
                {
                    if (voiceClips[i].name == name)
                    {
                        return voiceClips[i].GetAudioSource().isPlaying;
                    }
                }
                break;

            case Utils.AudioType.SFX:
                for (int i = 0; i < sfxClips.Length; i++)
                {
                    if (sfxClips[i].name == name)
                    {
                        return sfxClips[i].GetAudioSource().isPlaying;
                    }
                }
                break;

            case Utils.AudioType.MUSIC:
                for (int i = 0; i < musicClips.Length; i++)
                {
                    if (musicClips[i].name == name)
                    {
                        return musicClips[i].GetAudioSource().isPlaying;
                    }
                }
                break;
        }

        Debug.LogWarning("AudioManager> AUDIO not found: " + name);
        return false;
    }

    #endregion

    /*index
      ########################
      #                      #
      #  MUSICA DE JUEGO     #
      #                      #
      ########################
    */
    #region MUSICA

    private GameObject musicSource;
    private AudioSource musicAudioSource;

    private Utils.Music playingNow;

    public void SetPlayingNow(Utils.Music newMusic) { playingNow = newMusic; }
    public Utils.Music GetPlayingNow() { return playingNow; }

    //Variables de control, última música reproducida en el juego
    private int lastMainMenuMusicIndex;
    private int lastInGameMusicIndex;

    private void SelectMusicClipByType(Utils.Music musicZone, ref int actualIndex)
    {
        int firstIndex = actualIndex;

        while (actualIndex < musicClips.Length)
        {
            //Cargamos el clip correspondiente al actualIndex
            if (musicClips[actualIndex].musicType == musicZone)
            {
                musicAudioSource.clip = musicClips[actualIndex].clip;
            }

            //Aumentamos el index actual
            actualIndex++;
        }

        //En caso de salir del Length, asignamos index a 0
        actualIndex = 0;

        while (actualIndex < musicClips.Length)
        {
            //Cargamos el clip correspondiente al actualIndex
            if (musicClips[actualIndex].musicType == musicZone)
            {
                musicAudioSource.clip = musicClips[actualIndex].clip;
            }

            //Aumentamos el index actual
            actualIndex++;
        }

        //En caso de salir del Length por segunda vez: Restauramos index y devolvemos warning
        actualIndex = firstIndex;
        Debug.LogWarning("AudioManager> SelectMusicClipByType: Init index("+firstIndex+"). Can't found any musicClip with type: " + musicZone);
    }

    public void StopMusic()
    {
        musicAudioSource.Stop();
    }

    public void PlayMusic(Utils.Music musicZone)
    {
        //Paramos la música que esté sonando y liberamos el clip. Hacemos una parada en caso de querer llamar esta función fuera del Update.
        musicAudioSource.Stop();
        musicAudioSource.clip = null;

        switch (musicZone)
        {
            case Utils.Music.MAINMENU:
                musicAudioSource.clip = musicClips[1].clip;
                break;

            case Utils.Music.INGAME:
                musicAudioSource.clip = musicClips[0].clip;
                break;
        }

        musicAudioSource.Play();

    }



    #endregion

    /*index
      ########################
      #                      #
      #  FUNCIONES DE UNITY  #
      #                      #
      ########################
    */

    // Instanciar SoundManager
    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.Log("Warning: multiple " + this + " in scene!");
        }
    }

    private void Start()
    {
        //Instanciamos y preparamos el AudioSource de la música. Recogemos referencia de su AudioSource
        musicSource = new GameObject("MusicSource");
        musicSource.transform.SetParent(this.transform);
        musicSource.AddComponent<AudioSource>();
        musicAudioSource = musicSource.GetComponent<AudioSource>();


        masterVolume = 0.50f;
        musicVolume = 0.25f;
        sfxVolume = 0.25f;


        playingNow = Utils.Music.NONE;

        //Instanciamos los sfx
        for (int i = 0; i < sfxClips.Length; i++)
        {
            GameObject _go = new GameObject("SoundEffect_" + i + "_" + sfxClips[i].name);
            _go.transform.SetParent(this.transform);
            sfxClips[i].SetSource(_go.AddComponent<AudioSource>());
        }

        //Instanciamos las musicas
        for (int i = 0; i < musicClips.Length; i++)
        {
            GameObject _go = new GameObject("SoundEffect_" + i + "_" + musicClips[i].name);
            _go.transform.SetParent(this.transform);
            musicClips[i].SetSource(_go.AddComponent<AudioSource>());
        }

        //Instanciamos las voices lines
        for (int i = 0; i < voiceClips.Length; i++)
        {
            GameObject _go = new GameObject("Voice_" + i + "_" + sfxClips[i].name);
            _go.transform.SetParent(this.transform);
            voiceClips[i].SetSource(_go.AddComponent<AudioSource>());
        }

        //Setting index
        lastMainMenuMusicIndex = 0;
        lastInGameMusicIndex = 0;
    }


    private void Update()
    {
        //Esta condicion sirve para asegurarse de que siempre hay musica de fondo.
        if (musicAudioSource.isPlaying == false && playingNow != Utils.Music.NONE)
        {
            PlayMusic(playingNow);
        }
    }
}