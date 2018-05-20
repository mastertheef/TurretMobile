using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    protected int TTL = 100;
    protected float projectileSpeed = 100f;
    protected float damage = 10;
    public float RaycastAdvance = 2f;
    bool isHit = false;
    RaycastHit hitPoint; // Raycast structure 

    [SerializeField] Transform ImpactPrefab;
    [SerializeField] LayerMask layerMask;

    public void SetValues(float speed, float damage)
    {
        this.projectileSpeed = speed;
        this.damage = damage;
    }

    public void Update()
    {
        if (isHit)
        {
            var impact = Instantiate(ImpactPrefab, transform.position, Quaternion.identity);
            var health = hitPoint.transform.gameObject.GetComponentInParent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else
        {

            Vector3 step = transform.forward * Time.deltaTime * projectileSpeed;
            transform.position += step;
            if (Physics.Raycast(transform.position, transform.forward, out hitPoint, step.magnitude * RaycastAdvance,
                       layerMask))
            {
                isHit = true;
            }
        }
    }
}
