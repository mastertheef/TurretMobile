using Forge3D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Turret : Singleton<Turret>
{

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
    private bool isInBattleMode = true;

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

    private CannonController cannonController;
    private FixHarvestBeam fixHarvestBeam;
    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        cannonController = GetComponent<CannonController>();
        fixHarvestBeam = GetComponent<FixHarvestBeam>();
    }

    private void FixedUpdate()
    {
        if (!SystemInfo.supportsGyroscope)
        {
            if (GameManager.Instance.IsInBattleMode)
            {
                if (Input.GetMouseButton(0) && CanFire && !isFiring)
                {
                    isFiring = true;
                    cannonController.StartFire();
                }

                if (!Input.GetMouseButton(0) && isFiring)
                {
                    isFiring = false;
                    cannonController.StopFire();
                }
            }
            else
            {
                if (Input.GetMouseButtonUp(0) && CanFire && !isFiring)
                {
                    fixHarvestBeam.Fire();
                }
            }
        }
        else
        {
            if (CrossPlatformInputManager.GetButton("Fire") && CanFire && !isFiring)
            {
                isFiring = true;
                cannonController.StartFire();

            }

            if (isFiring && !CrossPlatformInputManager.GetButton("Fire"))
            {
                isFiring = false;
                cannonController.StopFire();
            }
        }
    }


    // dprecateed
    public void RestartIfFiring()
    {
        if (isFiring)
        {
            StopFiring();
            StartFiring();
        }
    }

    // deprecated
    public void StartFiring(float delay = 0)
    {
        if (!isFiring)
        {
            F3DFXController.instance.Fire();
        }
    }

    // deprecated
    public void StopFiring()
    {
        F3DFXController.instance.Stop();
    }

    // deprecated
    private IEnumerator ShootingDelay()
    {
        canFire = false;
        yield return new WaitForSeconds(ShootCounter);
        canFire = true;
    }
}
