using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothershipMovement : MonoBehaviour {
    [SerializeField] private float speed;
	// Use this for initialization
	void Start () {
        
        
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.Translate(transform.forward * speed * Time.deltaTime);
    }
}
