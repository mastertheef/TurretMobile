using Forge3D;
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
    private bool isFiring;
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

    private F3DFXController fxController;
    private CannonController cannonController;
    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        fxController = GetComponent<F3DFXController>();
        cannonController = GetComponent<CannonController>();
    }

    private void FixedUpdate()
    {
        if (!SystemInfo.supportsGyroscope)
        {
            if (Input.GetMouseButton(0) && CanFire && !isFiring)
            {
                // StartCoroutine(PlayStartShootAndWait());
                //StartFiring();
                isFiring = true;
                //fxController.Fire();
                cannonController.StartFire();
            }

            if (!Input.GetMouseButton(0) && isFiring)
            {
                //StopFiring();
                isFiring = false;
                //fxController.Stop();
                cannonController.StopFire();
            }
        }
        else
        {
            if (CrossPlatformInputManager.GetButton("Fire") && CanFire && !isFiring)
            {
                // StartCoroutine(PlayStartShootAndWait());
                isFiring = true;
                //StartFiring();
                fxController.Fire();

            }

            if (isFiring && !CrossPlatformInputManager.GetButton("Fire"))
            {
                isFiring = false;
                //StopFiring();
                fxController.Stop();
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

        //if (!IsInvoking("FireLeft"))
        //    InvokeRepeating("FireLeft", delay, ShootCounter);
        //if (!IsInvoking("FireRight"))
        //    InvokeRepeating("FireRight", delay + ShootCounter / 2, ShootCounter);

        if (!isFiring)
        {
            F3DFXController.instance.Fire();
        }
    }

    public void StopFiring()
    {
        F3DFXController.instance.Stop();
    }

    private IEnumerator ShootingDelay()
    {
        canFire = false;
        yield return new WaitForSeconds(ShootCounter);
        canFire = true;
    }
}
