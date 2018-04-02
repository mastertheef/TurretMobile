using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    float hitPoints;
    float shield;
    [SerializeField] float maxHitPoints;
    [SerializeField] float maxShield;
    [SerializeField] float restoreShieldDelay;
    [SerializeField] float restoreShieldSpeed;
    [SerializeField] Slider hitPointsSlider;
    [SerializeField] Slider shieldSlider;

    private Coroutine shieldRestoration;
    private bool isDead = false;

    public float HitPoints { get { return hitPoints; } }

	// Use this for initialization
	void Start () {
        hitPointsSlider.maxValue = maxHitPoints;
        shieldSlider.maxValue = maxShield;
        hitPoints = maxHitPoints;
        shield = maxShield;
    }
	
	// Update is called once per frame
	void Update () {
		if (shield < maxShield && shieldRestoration == null && !isDead)
        {
            shieldRestoration = StartCoroutine(RestoreShield());
        }
	}

    public void TakeDamage(float damage)
    {
        if (shield > 0)
        {
            if (shield >= damage)
            {
                shield -= damage;
            }
            else
            {
                damage = damage - shield;
                shield = 0;
                hitPoints -= damage;
                DieIfNoHP();
            }
        }
        else
        {
            hitPoints -= damage;
            DieIfNoHP();
        }
    }

    private void DieIfNoHP()
    {
        if (hitPoints <= 0)
        {
            hitPoints = 0;
            StartCoroutine(Die()); ;
        }
    }


    private IEnumerator RestoreShield()
    {
        yield return new WaitForSeconds(restoreShieldDelay);
        while (shield < maxShield)
        {
            shield += restoreShieldSpeed * Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator Die()
    {
        isDead = true;
        var camera = GameManager.Instance.GameCamera;
        camera.transform.SetParent(null);
        camera.transform.Translate(camera.transform.forward * (-5));
        hitPoints = 0;
        shield = 0;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GameManager.Instance.UI.enabled = false;
        GameManager.Instance.playerIsDead = true;
        // Explode

        yield return new WaitForSeconds(3);
        SceneController.Instance.FadeAndLoadScene("LevelEnd");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (shieldRestoration != null)
        {
            StopCoroutine(shieldRestoration);
            shieldRestoration = null;
        }

        if (collision.gameObject.tag == "EnemyLaser" || collision.gameObject.tag == "BossRocket")
        {
            float damage = collision.gameObject.GetComponent<Projectile>().Damage;
            TakeDamage(damage);
        }
        else if (collision.gameObject.tag == "EnemyShip" ||
                 collision.gameObject.tag == "EnemyPart" ||
                 collision.gameObject.tag == "Asteroid" ||
                 collision.gameObject.tag == "Boss")
        {
            if (!collision.gameObject.GetComponentInParent<Enemy>().IsExploded)
            {
                TakeDamage(150);
                if (collision.gameObject.tag != "Boss")
                {
                    collision.gameObject.GetComponentInParent<Enemy>().Die();
                }
            }
        }
    }

    private void OnGUI()
    {
        shieldSlider.value = shield;
        hitPointsSlider.value = hitPoints;
    }
}
