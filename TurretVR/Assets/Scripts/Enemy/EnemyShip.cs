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
    [SerializeField] private float shootDelay = 2;
    private float shootTimer;

    private bool canShoot = false;
    private int shootCount = 0;
    private Transform player;
    
    private Vector3 targetPoint;
     
	// Use this for initialization
	void Start () {
        player = GameManager.Instance.Player.transform;
       // float left = Random.Range(moveScatter * (-2), moveScatter * (-1));
        //targetPoint = (Vector3.zero - transform.position) + Random.insideUnitSphere * moveScatter;
        //transform.rotation = Quaternion.LookRotation(targetPoint.normalized);
	}
	
	// Update is called once per frame
	void Update () {
        shootTimer += Time.deltaTime;
        ExplodeIfKilled();

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
    }

    private void Shoot()
    {
        if (shootTimer > shootDelay && isInFront() && Vector3.Distance(transform.position, player.position) <= shootDistance)
        {
            var target = player.position;
            Projectile l = Instantiate(laser, cannons[Random.Range(0, cannons.Length - 1)].transform.position, transform.rotation);
            l.ReduceSeconds = reduceSeconds;
            var targetRange = new Vector3(target.x + Random.Range(shootScatter * -1, shootScatter), target.y + Random.Range(shootScatter * -1, shootScatter), target.z);
            l.transform.LookAt(targetRange);
            //l.transform.LookAt(target);
            l.Fire();
            shootTimer = 0;
        }
    }

    public override void Die()
    {
        Explode();
        GameManager.Instance.ReduceShips();
        IndicatorManager.Instance.RemoveIndicator(transform);
    }

    private void FixedUpdate()
    {
        if (!isExploded)
        {
            Shoot();
        }

        //if (fliedAway())
        //{
        //    IndicatorManager.Instance.RemoveIndicator(transform);
        //    Destroy(gameObject);
        //}
        // StartCoroutine(Fly());
    }

    private bool isInFront()
    {
        return Vector3.Dot(Vector3.forward, transform.InverseTransformPoint(player.position)) > 0;
    }

    private IEnumerator Fly()
    {
        while (!isExploded)
        {
            transform.position += targetPoint.normalized * moveSpeed * Time.deltaTime;
            yield return null;
        }
    }

    private bool fliedAway()
    {
        return Vector3.Distance(Vector3.zero, transform.position) >= flyAwayDistance;
    }

    private bool isBehind()
    {
        return (Vector3.zero - transform.position).z > 0;
    }
}
