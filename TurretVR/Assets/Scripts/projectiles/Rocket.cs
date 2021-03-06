﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Rocket : Projectile {

    [SerializeField] private Explosion explosion;
    private GameObject target;
    private bool isExploded = false;
    private float motherShipOffset;
    private Camera gameCamera;

	// Use this for initialization
	void Start () {
        gameCamera = GameManager.Instance.GameCamera;
        motherShipOffset = 0f;
        target = FindNearestTarget();
        if (target != null)
        {
            transform.LookAt(target.transform);
        }
        else
        {
            transform.rotation = GameManager.Instance.Player.transform.rotation;
        }
        StartCoroutine(Move());
        StartCoroutine(TimeToLive());
	}

    private IEnumerator TimeToLive()
    {
        yield return new WaitForSeconds(TTL);
        StartCoroutine(Explode());
    }
	
    private new void OnCollisionEnter(Collision collision)
    {
        string colTag = collision.gameObject.tag;
        if (colTag == "EnemyShip")
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(damage);
            StartCoroutine(Explode());
        }
        else if (colTag == "EnemyPart")
        {
            collision.gameObject.GetComponentInParent<Health>().TakeDamage(damage);
            StartCoroutine(Explode());
        }
        else if (colTag == "Boss")
        {
            StartCoroutine(Explode());
        }
    }

    private GameObject FindNearestTarget()
    {
        List<GameObject> allEnemies = //GameObject.FindGameObjectsWithTag("Asteroid").ToList();
        GameObject.FindGameObjectsWithTag("EnemyShip").ToList();
        allEnemies.AddRange(GameObject.FindGameObjectsWithTag("Boss").ToList());

        List<GameObject> allVisible = GetAllOnScreen(allEnemies);
        return GetNearest(allVisible);
    }

    private List<GameObject> GetAllOnScreen(List<GameObject> allEnemies)
    {
        for (int i = allEnemies.Count - 1; i >= 0; i--)
        {
            Vector3 screenPoint = gameCamera.WorldToViewportPoint(allEnemies[i].transform.position);
            var enemy = allEnemies[i].GetComponent<Enemy>();
            
            if (enemy == null || !IsOnScreen(screenPoint) || enemy.IsExploded)
            {
                allEnemies.Remove(allEnemies[i]);
            }
        }

        return allEnemies;
    }

    private GameObject GetNearest(List<GameObject> allVisible)
    {
        List<float> ranges = new List<float>();

        if (allVisible == null || allVisible.Count == 0)
        {
            return null;
        }

        allVisible.ForEach(x => ranges.Add(Vector3.Distance(Vector3.zero, x.transform.position)));
        int minIndex = ranges.IndexOf(Mathf.Min(ranges.ToArray()));
        return allVisible[minIndex];
    }

    private bool IsOnScreen(Vector3 point)
    {
        return point.z > 0 && point.x > 0 && point.x < 1 && point.y > 0 && point.y < 1;
    }

    protected IEnumerator Move()
    {

            if (target == null || target.GetComponent<Enemy>().IsExploded)
            {
                target = FindNearestTarget();
            }

            if (target != null && !target.GetComponent<Enemy>().IsExploded)
            {
                
                if (target.GetComponentInParent<MotherShip>() != null && motherShipOffset == 0)
                {
                    motherShipOffset = Random.Range(50f, 100f);
                }

                Vector3 targetPosition = new Vector3(target.transform.position.x - motherShipOffset, target.transform.position.y, target.transform.position.z);

                transform.position = Vector3.MoveTowards(transform.position, targetPosition, projectileSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
            }
            yield return null;
       
    }

    private IEnumerator Explode()
    {
        if (!isExploded)
        {
            Instantiate(explosion, transform);
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            var emmision = GetComponentInChildren<ParticleSystem>().emission;
            emmision.enabled = false;

            yield return new WaitForSeconds(5);
            Destroy(gameObject);
        }
    }
}
