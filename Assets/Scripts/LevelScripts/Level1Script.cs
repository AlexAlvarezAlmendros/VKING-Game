using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.StopAllAudios();
        SoundManager.Instance.PlayMusic("Ambient");
        //SoundManager.Instance.PlayMusic("Combat");

        //GameManager.Instance.loadCharacterPowerups();
    }

    private void Update()
    {
        if (!SoundManager.Instance.SoundIsPlaying(Utils.AudioType.MUSIC, "Combat"))
        {
            SoundManager.Instance.PlayMusic("Combat");
        }
    }

}
