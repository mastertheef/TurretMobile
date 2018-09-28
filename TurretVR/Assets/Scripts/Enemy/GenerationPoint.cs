using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Forge3D;
using UnityEngine;

public class GenerationPoint : MonoBehaviour
{
    [SerializeField] private List<Enemy> enemies;
    [SerializeField] private float aggroRadius;
    [SerializeField] private float battleRadius;
    [SerializeField] private int enemyCount;
    [SerializeField] private float changeTargetCooldown;
    private Transform player;
    private Transform motherShip;
    private List<Enemy> spawnedEnemies;
    private List<Transform> inRadiusUnits = new List<Transform>();
    private float changeTargetTimer;

    // Use this for initialization
    void Start()
    {
        player = GameManager.Instance.Player.transform;
        motherShip = GameManager.Instance.MotherShip.transform;
        spawnedEnemies = new List<Enemy>();
        for (int i = 0; i < enemyCount; i++)
        {
            int enemyIndex = Random.Range(0, enemies.Count);
            Vector3 position = transform.position + Random.insideUnitSphere * aggroRadius;
            Enemy enemy = Instantiate(enemies[enemyIndex], position, Random.rotation);
            enemy.GetComponent<ShipMovement>().Target = transform;
            enemy.GetComponent<EnemyShip>().Target = transform;
            enemy.SpawnPoint = this;
            spawnedEnemies.Add(enemy);
        }
    }

    private void Update()
    {
        changeTargetTimer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        SetTarget(player, motherShip);
    }

    private void SetTarget(params Transform[] portentialTargets)
    {
        foreach (var potentialTarget in portentialTargets)
        {
            if (!inRadiusUnits.Exists(x => x == potentialTarget) && Vector3.Distance(potentialTarget.position, transform.position) <= aggroRadius)
            {
                if (!inRadiusUnits.Any())
                    foreach (var ship in spawnedEnemies)
                    {
                        IndicatorManager.Instance.AddIndicator(ship.transform);
                    }
                inRadiusUnits.Add(potentialTarget);
            }
            else if (inRadiusUnits.Exists(x => x == potentialTarget) && Vector3.Distance(potentialTarget.position, transform.position) > battleRadius)
            {
                inRadiusUnits.Remove(potentialTarget);
                if (!inRadiusUnits.Any())
                    foreach (var ship in spawnedEnemies)
                    {
                        IndicatorManager.Instance.RemoveIndicator(ship.transform);
                        ship.GetComponent<ShipMovement>().Target = transform;
                        ship.GetComponent<EnemyShip>().Target = transform;
                    }
            }
        }
        if (changeTargetTimer > changeTargetCooldown)
        {
            if (inRadiusUnits.Any())
                foreach (var ship in spawnedEnemies)
                {
                    var target = GetNearestTarget(ship.transform);
                    ship.GetComponent<ShipMovement>().Target = target;
                    ship.GetComponent<EnemyShip>().Target = target;
                }
            changeTargetTimer = 0;
        }
    }

    private Transform GetNearestTarget(Transform ship)
    {
        Transform closestTarget = null;
        float closestDistance = float.MaxValue;
        foreach (var potentialTarget in inRadiusUnits)
        {
            var distance = Vector3.Distance(ship.position, potentialTarget.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = potentialTarget;
            }
        }
        return closestTarget;
    }

    public void RemoveEnemy(Enemy enemy)
    {
        spawnedEnemies.Remove(enemy);
    }
}
