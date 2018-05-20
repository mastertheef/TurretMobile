using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Health : MonoBehaviour {
    protected float hitPoints;
    protected float shield;
    protected bool isDead = false;
    [SerializeField] protected float maxHitPoints;
    [SerializeField] protected float maxShield;

    // Use this for initialization
    protected void Start () {
        hitPoints = maxHitPoints;
        shield = maxShield;
    }
	
    public void TakeDamage(float damage)
    {
        if (shield > 0)
        {
            if (shield >= damage)
            {
                shield -= damage;
            }
            else
            {
                damage = damage - shield;
                shield = 0;
                hitPoints -= damage;
                DieIfNoHP();
            }
        }
        else
        {
            hitPoints -= damage;
            DieIfNoHP();
        }
    }

    private void DieIfNoHP()
    {
        if (hitPoints <= 0)
        {
            hitPoints = 0;
            StartCoroutine(Die()); ;
        }
    }

    protected abstract IEnumerator Die();
}
