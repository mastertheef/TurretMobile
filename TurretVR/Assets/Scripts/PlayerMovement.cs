using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] private float speed = 0;
    [SerializeField] private float rotationSpeed = 10;

    private const float lowPassFilterFactor = 0.2f;
    private float pitch, yaw;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {



        yaw += CrossPlatformInputManager.GetAxis("Horizontal");
        // yaw = Mathf.Clamp(yaw, -1 * maxEulerAngle, maxEulerAngle);
        pitch += -CrossPlatformInputManager.GetAxis("Vertical");
        // pitch = Mathf.Clamp(pitch, -1 * maxEulerAngle, maxEulerAngle);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(pitch, yaw, 0), rotationSpeed);
    }
}
