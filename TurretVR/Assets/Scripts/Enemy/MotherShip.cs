using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MotherShip : MonoBehaviour {
    [Header("Dont touch")]
    [SerializeField] private float shipLength = 12f;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float portalInOffset = 102f;
    [SerializeField] private float portalOutOffset = 227f;
    

    [Header("Prefabs and parts")]
    [SerializeField] private GameObject PortalPrefab;
    [SerializeField] private GameObject ship;
    [SerializeField] private GameObject invisibleCapsule;
    [SerializeField] private Explosion ExplosionPrefab;
    [SerializeField] private List<GameObject> rocketCannons;
    [SerializeField] private BossRocket rocket;
    [SerializeField] GameObject target;
    [SerializeField] Image ShieldBar;
    [SerializeField] Image HeathBar;

    [Header("Balance")]
    [SerializeField] private float spawnDistance = 350f;
    [SerializeField] private float EnergyShield = 1000f;
    [SerializeField] private float EnergyShieldRestoreSpeed = 10f;
    [SerializeField] private float Hitpoints = 5000f;
    [SerializeField] private float LaserBeamDamage = 5f;
    [SerializeField] private int ScorePoints = 200;
    [SerializeField] private int addSeconds = 20;
    [SerializeField] private int dieExplosionsCount = 30;
    [SerializeField] private float dieExplosionScale = 15;
    [SerializeField] private float rocketLaunchDelay = 0.7f;

    [Header("Not implemented")]
    [SerializeField] private float EnergyShieldRechrgeDelay = 2f;

    private bool started = false;
    private bool visible = false;
    private bool isDead = false;
    private Vector3 invisibleCapsuleDefaultPosition;
    private GameObject portal;
    private float energyShield;
    private float hitPoints;
    private Coroutine teleporting;


	// Use this for initialization
	void Start () {
        invisibleCapsuleDefaultPosition = invisibleCapsule.transform.localPosition;
        energyShield = EnergyShield;
        hitPoints = Hitpoints;
        target = GameManager.Instance.Player;
    }

    private void FixedUpdate()
    {
        if (!started)
        {
           teleporting = StartCoroutine(Teleporting());
        }
    }

    // Update is called once per frame
    void Update()
    {
        var energyScale = ShieldBar.transform.localScale;
        var healthScale = HeathBar.transform.localScale;
        var shield = (energyShield * 100 / EnergyShield) / 100;
        shield = shield < 0 ? 0 : shield;
        var health = (hitPoints * 100 / Hitpoints) / 100;
        health = health < 0 ? 0 : health;
        ShieldBar.transform.localScale = new Vector3(shield, energyScale.y, energyScale.z);
        HeathBar.transform.localScale = new Vector3(health, healthScale.y, healthScale.z);
    }


    private IEnumerator MoveIn()
    {
        Vector3 portalPosition = new Vector3(transform.localPosition.x - portalInOffset, transform.localPosition.y, transform.localPosition.z);
        portal = Instantiate(PortalPrefab, portalPosition, transform.rotation);
        Vector3 direction = new Vector3(0, 1, 0);
        while (ship.transform.localPosition.z > shipLength * (-1)) 
        {
            ship.transform.Translate(direction * moveSpeed * Time.deltaTime);
            yield return null;
        }
       
        Destroy(portal.gameObject);
        visible = true;
    }

    private IEnumerator MoveOut()
    {
        Vector3 portalPosition = new Vector3(transform.localPosition.x - portalOutOffset, transform.localPosition.y, transform.localPosition.z);
        portal = Instantiate(PortalPrefab, portalPosition, transform.rotation);
        invisibleCapsule.transform.localPosition = new Vector3(0, 0, -4.4f - shipLength*2);
        Vector3 direction = new Vector3(0, 1, 0);
        while (ship.transform.localPosition.z > shipLength * (-2))
        {
            ship.transform.Translate(direction * moveSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(portal.gameObject);

        ship.transform.localPosition = Vector3.zero;
        invisibleCapsule.transform.localPosition = invisibleCapsuleDefaultPosition;
        visible = false;
    }

    private Vector3 GetRandomPosition()
    {
        Quaternion randAng = Quaternion.Euler(Random.Range(GameManager.Instance.EnemyMaxBottom, GameManager.Instance.EnemyMaxTop), Random.Range(GameManager.Instance.EnemyMaxLeft, GameManager.Instance.EnemyMaxRight), 0);
        return randAng * Vector3.forward * spawnDistance;
    }

    private IEnumerator Teleporting()
    {
        started = true;
        while (true)
        {
            transform.position = GetRandomPosition();

            StartCoroutine(MoveIn());
            while (!visible) { yield return null; }

            yield return new WaitForSeconds(3);

            StartCoroutine(Shoot());
            yield return new WaitForSeconds(2);

            StartCoroutine(MoveOut());
            while (visible) { yield return null; }

            yield return new WaitForSeconds(1);
        }
    }

    public void Collision(Collision collision)
    {
        if (collision.gameObject.tag == "Laser" || collision.gameObject.tag == "Rocket")
        {
            var damage = collision.gameObject.GetComponent<Projectile>().Damage;

            if (energyShield > 0)
            {
                energyShield -= damage;
            }
            else
            {
                hitPoints -= damage;
            }

            if (collision.gameObject.tag == "Rocket")
            {
                var exp = Instantiate(ExplosionPrefab, collision.contacts[0].point, Quaternion.identity);
                exp.transform.localScale *= 5; 
            }

            if (hitPoints <= 0)
            {
                hitPoints = 0;
                StartCoroutine(Die());
            }
        }
    }

    public void ParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "LaserBeam")
        {
            if (energyShield > 0)
            {
                energyShield -= LaserBeamDamage;
            }
            else
            {
                hitPoints -= LaserBeamDamage;
            }

            if (hitPoints <= 0)
            {
                hitPoints = 0;
                StartCoroutine(Die());
            }
        }
    }

    private IEnumerator Die()
    {
        if (!isDead)
        {
            StopCoroutine(teleporting);
            invisibleCapsule.GetComponent<MeshRenderer>().enabled = false;
            ship.GetComponent<Collider>().enabled = false;
            IndicatorManager.Instance.RemoveIndicator(transform);

            isDead = true;
            for (int i = 0; i < dieExplosionsCount; i++)
            {
                var point = GetRandomSurfacePoint();
                var explosion = Instantiate(ExplosionPrefab, point, Quaternion.identity);
                explosion.transform.localScale *= dieExplosionScale;
                yield return new WaitForSeconds(0.2f);
            }

            ship.GetComponent<MeshRenderer>().enabled = false;
            GameManager.Instance.Score += ScorePoints;
            GameManager.Instance.CountDown += addSeconds;
            SoundManager.Instance.PlayBackground();
            yield return new WaitForSeconds(5);
            Destroy(gameObject);
        }
    }

    private Vector3 GetRandomSurfacePoint()
    {
        Vector3[] vertices = ship.GetComponent<MeshFilter>().mesh.vertices;
        return ship.transform.TransformPoint(vertices[Random.Range(0, vertices.Length - 1)]);
    }

    private IEnumerator Shoot()
    {
        foreach (var cannon in rocketCannons)
        {
            var r = Instantiate(rocket, cannon.transform.position, Quaternion.identity);
            var shootPosition = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);

            r.transform.rotation = Quaternion.LookRotation(shootPosition - r.transform.position);
            r.ReduceSeconds = 5;
            r.Fire();
            yield return new WaitForSeconds(rocketLaunchDelay);
        }
    }

}
