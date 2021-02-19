using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingletonesLevel : MonoBehaviour
{
    private float time;

    private void Update()
    {
        time += Time.time;
    }
    void LateUpdate()
    {        
        if(time >= 3)
        {
            SceneManager.LoadScene(1);
        }
    }
}
