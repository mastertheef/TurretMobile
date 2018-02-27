using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(DestroyAfterEffectEnds());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private IEnumerator DestroyAfterEffectEnds()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
