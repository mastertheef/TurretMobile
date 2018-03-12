using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    [SerializeField] private Explosion explosion;
    [SerializeField] protected float hitPoints = 12;
    [SerializeField] protected float moveSpeed = 0.05f;
    [SerializeField] protected int score = 5;
    [SerializeField] protected float addSeconds = 1;
    [SerializeField] protected float reduceSeconds = 1;
    [SerializeField] private float resorceProbability = 0.1f;

    public GenerationPoint SpawnPoint { get; set; }
    

    protected bool isExploded = false;

    public bool IsExploded { get { return isExploded; } }
    public virtual void Die()
    {
        Explode();
    }

    protected virtual void Explode()
    {
        if (!isExploded)
        {
            var exp = Instantiate(explosion, gameObject.transform);
            exp.transform.position = gameObject.transform.position;
            isExploded = true;
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            Collider collider = GetComponent<Collider>();

            if (meshRenderer != null)
                meshRenderer.enabled = false;
            if (collider != null)
                collider.enabled = false;

            GameManager.Instance.ShipsCount++;
            GameManager.Instance.CountDown += addSeconds;
            GameManager.Instance.Score += score;
            IndicatorManager.Instance.RemoveIndicator(gameObject.transform);

            GiveResource();
        }
    }

    protected void ExplodeIfKilled()
    {
        if (hitPoints <= 0 && !isExploded)
        {
            Die();
        }
    }

    public virtual void TakeDamage(Collider collider)
    {
        var projectile = collider.gameObject.GetComponent<Projectile>();
        hitPoints -= projectile.Damage;
    }

    public virtual void TakeDamage(float damage)
    {
        hitPoints -= damage;
    }

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
