using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    // TODO: rebild to incapsulate the energy logic
    [SerializeField] private GameObject LaserEffects;
    [SerializeField] private ParticleSystem MainLaser;
    [SerializeField] private GameObject LaserBeamHitPrefab;
    [SerializeField] private AudioSource LaserChargeAudio;
    [SerializeField] private AudioSource LaserAudio;
    [SerializeField] private AudioSource LaserStopAudio;

    private GameObject laserBeamHit;
    private List<ParticleCollisionEvent> collisionEvents;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire()
    {
        LaserAudio.Play();
        LaserEffects.SetActive(true);
    }

    public void StopFire()
    {
        LaserAudio.Stop();
        LaserChargeAudio.Stop();
        LaserStopAudio.Play();
        LaserEffects.SetActive(false);
        if (laserBeamHit != null)
        {
            Destroy(laserBeamHit.gameObject);
        }
    }

    public void ParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "Boss")
        {
            collisionEvents = new List<ParticleCollisionEvent>();
            MainLaser.GetCollisionEvents(other, collisionEvents);

            if (laserBeamHit == null)
            {
                laserBeamHit = Instantiate(LaserBeamHitPrefab, collisionEvents.First().intersection.normalized, Quaternion.identity);

            }
            else
            {
                laserBeamHit.transform.position = collisionEvents.First().intersection;
            }
            var direction = Camera.main.transform.position - transform.position;
            laserBeamHit.transform.rotation = Quaternion.FromToRotation(transform.up, direction);
        }
    }
}
