using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPart : MonoBehaviour {

    private void OnCollisionEnter(Collision other)
    {
        if (other.rigidbody.tag == "Laser")
        {
            transform.parent.gameObject.GetComponent<EnemyShip>().TakeDamage(other.collider);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "LaserBeam")
        {
            transform.parent.gameObject.GetComponent<EnemyShip>().Die();
        }
    }
}
