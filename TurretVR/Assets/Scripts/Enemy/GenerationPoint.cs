using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationPoint : MonoBehaviour {
    [SerializeField] private List<Enemy> enemies;
    [SerializeField] private float aggroRadius;
    [SerializeField] private float battleRadius;
    [SerializeField] private int enemyCount;
    private Transform player;
    private List<Enemy> spawnedEnemies;
    private bool isInRadius = false;

    // Use this for initialization
    void Start () {
        player = GameManager.Instance.Player.transform;
        spawnedEnemies = new List<Enemy>();
		for (int i = 0; i < enemyCount; i++)
        {
            int enemyIndex = Random.Range(0, enemies.Count);
            Vector3 position = transform.position + Random.insideUnitSphere * aggroRadius;
            Enemy enemy = Instantiate(enemies[enemyIndex], position, Random.rotation);
            enemy.SpawnPoint = this;
            spawnedEnemies.Add(enemy);
        }
	}

    private void Update()
    {
        if (Vector3.Distance(player.position, transform.position) <= aggroRadius && !isInRadius)
        {
            foreach (var ship in spawnedEnemies)
            {
                ship.GetComponent<ShipMovement>().Target = player.transform;
                IndicatorManager.Instance.AddIndicator(ship.transform);
            }
            isInRadius = true;
        }

        else if (Vector3.Distance(player.position, transform.position) > battleRadius && isInRadius)
        {
            foreach (var ship in spawnedEnemies)
            {
                ship.GetComponent<ShipMovement>().Target = transform;
                IndicatorManager.Instance.RemoveIndicator(ship.transform);
            }
            isInRadius = false;
        }
    }

    public void RemoveEnemy(Enemy enemy)
    {
        spawnedEnemies.Remove(enemy);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, aggroRadius);
    }
}
