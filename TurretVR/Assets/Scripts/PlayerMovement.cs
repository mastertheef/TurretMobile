using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 0.1f;
    [SerializeField] private Slider speedSlider;

    private const float lowPassFilterFactor = 0.2f;
    private float pitch, yaw;
    private Rigidbody rigidBody;
    float velocity = 0;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        speedSlider.maxValue = maxSpeed;
        
        //speedSlider.onValueChanged+=
        
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        yaw = Mathf.Tan(CrossPlatformInputManager.GetAxis("Horizontal")) * rotationSpeed;
        pitch = Mathf.Tan(-CrossPlatformInputManager.GetAxis("Vertical")) * rotationSpeed;
        
        transform.Rotate(pitch, yaw, 0, Space.Self);

        rigidBody.velocity = transform.forward * speedSlider.value;
        
    }

    private Quaternion GetRotation(float yaw, float pitch)
    {
        if (transform.rotation.x <= -0.5f)
        {
            return Quaternion.Euler(0, pitch, -yaw);
        }
        return Quaternion.Euler(pitch, yaw, 0);
    }
}
