using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossRocket : Projectile {

    [SerializeField] private Explosion ExplosionPrefab;
    [SerializeField] private float resorceProbability = 0.5f;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private new void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (collision.gameObject.tag == "Player")
        {
            var exp = Instantiate(ExplosionPrefab, collision.contacts.First().point, Quaternion.identity);
            exp.transform.localScale *= 3;

            GameManager.Instance.CountDown -= ReduceSeconds;
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Laser" )
        {
            Explode();
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "LaserBeam")
        {
            Explode();
        }
    }

    private void Explode()
    {
        var exp = Instantiate(ExplosionPrefab, transform.localPosition, Quaternion.identity);
        exp.transform.localScale *= 3;
        Destroy(gameObject);

        GiveResource();
    }

    // todo: refactor code duplication
    private void GiveResource()
    {
        float probe = Random.Range(0f, 1f);
        if (probe < resorceProbability)
        {
            float choice = Random.Range(0f, 1f);
            if (choice < 0.5)
            {
                LaserBeamController.Instance.AddCharge();
            }
            else
            {
                RocketController.Instance.AddRocket();
            }
        }
    }
}
