using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health {

    protected override IEnumerator Die()
    {
        isDead = true;
        GetComponent<Enemy>().Die();
        yield return null;
    }    
}
