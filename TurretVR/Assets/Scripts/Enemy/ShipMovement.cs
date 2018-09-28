﻿using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    [SerializeField] float speed = 0.05f;
    [SerializeField] float turnSpeed = 0.5f;
    [SerializeField] float raycastOffset = 2.5f;
    [SerializeField] float detectoionDistance = 20f;
    [SerializeField] float virageDistance = 20f;
    Vector3 virageOffset = Vector3.zero;
    Quaternion virageRotOffset = Quaternion.identity;

    public Transform Target { get; set; }

    private bool makingVirage = false;
    // Use this for initialization
    void Start()
    {
        Target = GetComponentInParent<Enemy>().SpawnPoint.transform; //GameManager.Instance.Player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        PathFind();
    }

    void Move()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, Target.position) <= detectoionDistance)
        {
            makingVirage = true;
        }

        if (Vector3.Distance(transform.position, Target.position) >= virageDistance)
        {
            makingVirage = false;
            virageRotOffset = Quaternion.identity;
        }
    }

    void Turn()
    {
        Vector3 pos = Target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(pos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
    }

    void MakeVirage()
    {
        virageRotOffset = Quaternion.LookRotation(-Target.transform.forward * 10000);
        transform.rotation = Quaternion.Slerp(transform.rotation, virageRotOffset, turnSpeed * Time.deltaTime);
    }

    void PathFind()
    {
        RaycastHit hit;
        Vector3 offset = Vector3.zero;

        Vector3 left = transform.position - transform.right * raycastOffset;
        Vector3 right = transform.position + transform.right * raycastOffset;
        Vector3 up = transform.position + transform.up * raycastOffset;
        Vector3 down = transform.position - transform.up * raycastOffset;

        Debug.DrawRay(transform.position, transform.forward * 400, Color.red);
        Debug.DrawRay(left, transform.forward * detectoionDistance, Color.green);
        Debug.DrawRay(right, transform.forward * detectoionDistance, Color.green);
        Debug.DrawRay(up, transform.forward * detectoionDistance, Color.green);
        Debug.DrawRay(down, transform.forward * detectoionDistance, Color.green);

        if (Physics.Raycast(left, transform.forward, out hit, detectoionDistance))
        {
            offset += Vector3.right;
        }
        else if (Physics.Raycast(right, transform.forward, out hit, detectoionDistance))
        {
            offset -= Vector3.right;
        }

        if (Physics.Raycast(up, transform.forward, out hit, detectoionDistance))
        {
            offset -= Vector3.up;
        }
        else if (Physics.Raycast(down, transform.forward, out hit, detectoionDistance))
        {
            offset += Vector3.up;
        }

        if (offset != Vector3.zero)
        {
            transform.Rotate(offset * 5f * Time.deltaTime);
        }
        else
        {
            if (makingVirage)
            {
                MakeVirage();
            }
            else
            {
                Turn();
            }
        }
    }
}

