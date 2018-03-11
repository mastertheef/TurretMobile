using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Turret : Singleton<Turret>
{
    [Header("Prefabs")]
    [SerializeField] private Projectile Shot;
    [SerializeField] private GameObject CannonLeft;
    [SerializeField] private GameObject CannonRight;
    [SerializeField] private GameObject FireStartEffect;
    [SerializeField] private LaserBeam LaserBeam;

    [Header("Shooting")]
    [SerializeField] private float shootCounter;
    [SerializeField] private float shootDelay = 3;

    private float shootCounterPenetration = 0;
    private float shootCounterSpeedUp = 0;
    private float projectileAdditionalDamage = 0;

    private GameObject fireStartLeft;
    private GameObject fireStartRight;

    private bool canFire = true;
    private bool isFiring = false;
    private bool isDamaged = false;
    private bool laserBeamStarted = false;

    private List<LaserBeam> laserBeams;

    public float ShootCounterPenetration
    {
        get { return shootCounterPenetration; }
        set { shootCounterPenetration = value; }
    }

    public float ShootCounterSpeedUp
    {
        get { return shootCounterSpeedUp; }
        set { shootCounterSpeedUp = value; }
    }

    public float ProjectileAdditionalDamage
    {
        get { return projectileAdditionalDamage; }
        set { projectileAdditionalDamage = value; }
    }

    public bool IsFiring { get { return isFiring; } }
    public bool CanFire { get { return canFire && !isDamaged; } }
    public bool IsDamaged
    {
        get { return isDamaged; }
        set { isDamaged = value; }
    }

    public float ShootCounter
    {
        get
        {
            return shootCounter + shootCounterPenetration - shootCounterSpeedUp;
        }
    }

    public float ProjectileAdditionalScale { get; set; }

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (!SystemInfo.supportsGyroscope)
        {
            if (Input.GetMouseButton(0) && CanFire && !isFiring)
            {
                // StartCoroutine(PlayStartShootAndWait());
                StartFiring();
            }

            if (!Input.GetMouseButton(0))
            {
                StopFiring();
            }
        }
        else
        {
            if (CrossPlatformInputManager.GetButton("Fire") && CanFire && !isFiring)
            {
                // StartCoroutine(PlayStartShootAndWait());
                StartFiring();
            }

            if (!CrossPlatformInputManager.GetButton("Fire"))
            {
                StopFiring();
            }
        }
    }

    private IEnumerator PlayStartShootAndWait()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioSource.clip);
        ShowFireStart();
        yield return new WaitForSeconds(shootDelay);
        DestroyFireStart();
    }

    private void ShowFireStart()
    {
        fireStartLeft = fireStartLeft == null
           ? Instantiate(FireStartEffect, CannonLeft.transform)
           : fireStartLeft;
        fireStartRight = fireStartRight == null
            ? Instantiate(FireStartEffect, CannonRight.transform)
            : fireStartRight;
    }

    private void DestroyFireStart()
    {
        if (fireStartLeft != null) Destroy(fireStartLeft);
        if (fireStartRight != null) Destroy(fireStartRight);
    }

    public void RestartIfFiring()
    {
        if (isFiring)
        {
            StopFiring();
            StartFiring();
        }
    }

    public void StartFiring(float delay = 0)
    {
        isFiring = true;
        if (!IsInvoking("FireLeft"))
            InvokeRepeating("FireLeft", delay, ShootCounter);
        if (!IsInvoking("FireRight"))
            InvokeRepeating("FireRight", delay + ShootCounter / 2, ShootCounter);

    }

    public void StopFiring()
    {
        isFiring = false;
        CancelInvoke("FireLeft");
        CancelInvoke("FireRight");

        StartCoroutine(ShootingDelay());
    }

    private IEnumerator ShootingDelay()
    {
        canFire = false;
        yield return new WaitForSeconds(ShootCounter);
        canFire = true;
    }

    private void FireProjectile(GameObject Cannon)
    {
        Projectile proj = Instantiate(Shot, Cannon.transform.position, transform.rotation);
        ApplyProjectileModifiers(proj);
        proj.Fire();
    }

    private void FireLeft()
    {
        Projectile proj = Instantiate(Shot, CannonLeft.transform.position, transform.rotation);
        ApplyProjectileModifiers(proj);
        proj.Fire();
    }

    private void FireRight()
    {
        Projectile proj = Instantiate(Shot, CannonRight.transform.position, transform.rotation);
        ApplyProjectileModifiers(proj);
        proj.Fire(); ;
    }

    private void ApplyProjectileModifiers(Projectile proj)
    {
        proj.AddDamage = ProjectileAdditionalDamage;
        proj.transform.localScale += new Vector3(ProjectileAdditionalScale, ProjectileAdditionalScale, ProjectileAdditionalScale);
    }
}
