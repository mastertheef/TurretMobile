using Forge3D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : Enemy
{

    [SerializeField] private Projectile laser;
    [SerializeField] private GameObject[] cannons;

    [SerializeField] private float shootScatter = 10;
    [SerializeField] private float moveScatter = 20;
    [SerializeField] private float flyAwayDistance = 600;

    [SerializeField] private float shootDistance = 30000;
    [SerializeField] private float shootDelay = 3;
    [SerializeField] private float shootingTime = 2;
    private int shootTimerId = -1;
    private float shootTimer;

    private bool canShoot = false;
    private int shootCount = 0;
    public Transform Target { get; set; }
    private CannonController cannonController;

    private Vector3 targetPoint;

    // Use this for initialization
    void Start()
    {
        cannonController = GetComponent<CannonController>();
        //IndicatorManager.Instance.AddIndicator(transform);
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
        //ExplodeIfKilled();

        float distance = Vector3.Distance(Vector3.zero, transform.position);
        canShoot = (shootCount == 0 && distance <= GameManager.Instance.FirstShootDistance) ||
                   (shootCount == 1 && distance <= GameManager.Instance.SecondShootDistance);
    }

    protected override void Explode()
    {
        base.Explode();
        Rigidbody[] children = gameObject.GetComponentsInChildren<Rigidbody>();
        Collider[] chidrenColliders = gameObject.GetComponentsInChildren<Collider>();
        for (int i = 0; i < children.Length; i++)
        {
            children[i].isKinematic = false;
            chidrenColliders[i].isTrigger = false;
        }
        GameManager.Instance.ShipsCount++;
        SpawnPoint.RemoveEnemy(this);
    }

    private bool isFire;

    private void Shoot()
    {
        if (isInFront(Target) && Vector3.Distance(transform.position, Target.position) <= shootDistance)
        {
            if (!isFire)
            {
                isFire = true;
            }

            if (shootTimer > shootDelay)
            {
                cannonController.StartFire();
                shootTimerId = F3DTime.time.AddTimer(shootingTime, stopFiring);
                shootTimer = 0;
            }
        }
        else
        {
            if (isFire)
            {
                isFire = false;
                stopFiring();
            }
        }
    }

    private void stopFiring()
    {
        cannonController.StopFire();
        F3DTime.time.RemoveTimer(shootTimerId);
    }

    public override void Die()
    {
        Explode();
        IndicatorManager.Instance.RemoveIndicator(transform);
    }

    private void FixedUpdate()
    {
        if (!isExploded)
        {
            Shoot();
        }
    }

    private bool isInFront(Transform target)
    {
        var targetSize = 7;
        var shootAngle = Mathf.Rad2Deg * Mathf.Atan2(targetSize, Vector3.Distance(transform.position, target.position));
        return Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(target.position)) < shootAngle;
    }
}
