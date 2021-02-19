using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{

    ParticleSystem partSys;
    List<ParticleCollisionEvent> collisionEvents;
    public GameObject ExplosionDamageAreaPrefab;

    // Start is called before the first frame update
    void Start()
    {
        partSys = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = partSys.GetCollisionEvents(other, collisionEvents);
        for (int i = 0; i < numCollisionEvents; i++)
        {
            Vector3 spawnPosition = collisionEvents[i].intersection;
            GameObject newExplosionArea = Instantiate(ExplosionDamageAreaPrefab, spawnPosition, Quaternion.identity);
            Destroy(newExplosionArea,0.25f);
            SoundManager.Instance.PlayRandomSfx(Utils.SFX.EXPLOSION);
        }
        
        //Debug.Log("Boom");
    }
}
