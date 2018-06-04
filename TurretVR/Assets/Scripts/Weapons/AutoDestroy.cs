using Forge3D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour {

    [SerializeField] float ttl;
    public float TTL { get { return ttl; } }

    int timerId = -1;

	// Use this for initialization
	void Start () {
        timerId = F3DTime.time.AddTimer(ttl, SeldDestruct);
	}
	
	private void SeldDestruct()
    {
        F3DTime.time.RemoveTimer(timerId);
        if (this != null && gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}
