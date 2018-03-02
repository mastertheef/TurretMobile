using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : Enemy
{
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private BaseEffect debuff;

    private Vector3 rotationDirection;

	// Use this for initialization
	void Start () {
        
        rotationDirection = new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), Random.Range(-100f, 100f));
    }
	
	// Update is called once per frame
	void Update () {
        ExplodeIfKilled();
	}

    private void FixedUpdate()
    {
        transform.Rotate(rotationDirection, rotationSpeed);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Laser")
        {
            TakeDamage(other.collider);
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Player")
        {
            this.Explode();
            EffectSystem.Instance.AddEffect(debuff);
            GameManager.Instance.CountDown -= reduceSeconds;
        }

        if (other.gameObject.tag == "LaserBeam")
        {
            Explode();
        }
    }

    private IEnumerator MoveToPlayer()
    {
        while(!isExploded)
        {
            transform.localPosition = Vector3.MoveTowards(transform.position, Vector3.zero, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    protected override void Explode()
    {
        base.Explode();
        GameManager.Instance.AsteroidsCount++;
        GameManager.Instance.ReduceAsteroids();

    }

    private void OnParticleCollision(GameObject other)
    {
        Explode();
    }
}
