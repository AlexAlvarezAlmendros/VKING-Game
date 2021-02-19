using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportTowerVisuals : MonoBehaviour
{
    public bool doMovementAnimation;

    public float speedRotation;
    //public GameObject fxAntena;
    public GameObject fxBase;
    
    public GameObject explosionPrefab;
    public GameObject explosionPoint;

    public GameObject fxSmoke;
    public GameObject fxDamage;
    public GameObject fxBigDamage;

    private void Start()
    {
        doMovementAnimation = true;

        fxDamage.SetActive(false);
        fxBigDamage.SetActive(false);
        fxSmoke.SetActive(false);
    }

    public void ActivateDamageFX()
    {
        fxDamage.SetActive(true);
        
    }

    public void ActivateBigDamageFX()
    {
        fxBigDamage.SetActive(true);
    }

    public void ActivateSmokeFX()
    {
        fxSmoke.SetActive(true);
        fxDamage.SetActive(false);
        SoundManager.Instance.StopAudioByName(Utils.AudioType.SFX, "Electricity");
        SoundManager.Instance.PlayRandomSfx(Utils.SFX.EXPLOSION);
    }

    public void DoExplosionFX()
    {
        GameObject explosion = Instantiate(explosionPrefab, explosionPoint.transform.position, explosionPoint.transform.rotation, explosionPoint.transform);
        Destroy(explosion, 4f);
    }

    public void DeactivateParticlesFX()
    {
        //fxAntena.SetActive(false);
        fxBase.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (fxDamage.activeSelf)
        {
            SoundManager.Instance.PlaySfxLoop("Electricity");
        }

        if (doMovementAnimation)
        {
            transform.Rotate(-speedRotation/2 * Vector3.up * Time.deltaTime);
            transform.Rotate(speedRotation * Vector3.forward * Time.deltaTime);
        }
    }
}
