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
    [SerializeField] private bool canFly;
    private Transform MotherShip;

    public bool CanFly { get { return canFly; } }

    private float pitch, yaw;
    private Rigidbody rigidBody;
    float velocity = 0;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        MotherShip = GameManager.Instance.MotherShip.transform;
        speedSlider = GameObject.Find("SpeedSlider").GetComponent<Slider>();
        if (canFly)
        {
            speedSlider.maxValue = maxSpeed;
        }
        else
        {
            speedSlider.enabled = false;
            speedSlider.transform.localScale = Vector3.zero;
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (canFly)
        {
            rigidBody.velocity = transform.forward * speedSlider.value;
        }
        else
        {
            Vector3 mothershipPosition = new Vector3(MotherShip.position.x, MotherShip.position.y + 20f, MotherShip.position.z + 28.6f);
            transform.position = Vector3.MoveTowards(transform.position, mothershipPosition, 7f);
        }
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
