using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleAffected : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach(Renderer r in renderers)
        {
            r.material.renderQueue = 2503;
        }
	}
}
