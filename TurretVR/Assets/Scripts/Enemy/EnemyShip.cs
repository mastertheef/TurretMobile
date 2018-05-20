using Forge3D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : Enemy {

    [SerializeField] private Projectile laser;
    [SerializeField] private GameObject[] cannons;

    [SerializeField] private float shootScatter = 10;
    [SerializeField] private float moveScatter = 20;
    [SerializeField] private float flyAwayDistance = 600;

    [SerializeField] private float shootDistance = 300;
    [SerializeField] private float shootDelay = 3;
    [SerializeField] private float shootingTime = 2;
    private int shootTimerId = -1;
    private float shootTimer;

    private bool canShoot = false;
    private int shootCount = 0;
    private Transform player;
    private Transform motherShip;
    private CannonController cannonController;
    
    
    private Vector3 targetPoint;
     
	// Use this for initialization
	void Start () {
        player = GameManager.Instance.Player.transform;
        motherShip = GameManager.Instance.MotherShip.transform;
        cannonController = GetComponent<CannonController>();
        //IndicatorManager.Instance.AddIndicator(transform);
    }
	
	// Update is called once per frame
	void Update () {
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

    private void Shoot()
    {
        if (shootTimer > shootDelay && isInFront() && Vector3.Distance(transform.position, player.position) <= shootDistance)
        {
            cannonController.StartFire();
            shootTimerId = F3DTime.time.AddTimer(shootingTime, stopFiring);
            //var target = GetNearestTarget();
            //Projectile l = Instantiate(laser, cannons[Random.Range(0, cannons.Length - 1)].transform.position, transform.rotation);
            //l.ReduceSeconds = reduceSeconds;
            //var targetRange = new Vector3(target.x + Random.Range(shootScatter * -1, shootScatter), target.y + Random.Range(shootScatter * -1, shootScatter), target.z);
            //l.transform.LookAt(targetRange);
            ////l.transform.LookAt(target);
            //l.Fire();
            shootTimer = 0;
        }
        //else
        //{
        //    cannonController.StopFire();
        //}
    }

    private void stopFiring()
    {
        F3DTime.time.RemoveTimer(shootTimerId);
        cannonController.StopFire();
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

    private bool isInFront()
    {
        return Vector3.Dot(Vector3.forward, transform.InverseTransformPoint(player.position)) > 0;
    }

    private Vector3 GetNearestTarget()
    {
        var playerDistance = Vector3.Distance(transform.position, player.position);
        var motherShipDistance = Vector3.Distance(transform.position, motherShip.position);

        if (motherShipDistance<= playerDistance)
        {
            return player.position;
        }

        return motherShip.position;
    }
}
