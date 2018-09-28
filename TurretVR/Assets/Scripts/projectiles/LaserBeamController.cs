using Forge3D;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class LaserBeamController : Singleton<LaserBeamController> {

    [SerializeField] private int MaxLaserCharges = 3;
    [SerializeField] private float ChargeDuration = 2f;

    [SerializeField] private Text ChargesCount;
    [SerializeField] private Image ChareImagePrefab;

    private int laserCharges;

    private int LaserCharges {
        get { return laserCharges; }
        set
        {
            laserCharges = value;
            ChargesCount.text = laserCharges.ToString();
        }
    }

    private bool isFiring = false;
    private bool canFire = true;
    private Coroutine blinking;
    private Image currentCharge;
    private CannonController cannonController;


    // Use this for initialization
    void Start () {
        LaserCharges = MissionsManager.Instance != null? MissionsManager.Instance.StartLaserCount: 5;
        
        cannonController = GetComponent<CannonController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (CrossPlatformInputManager.GetButtonDown("Laser") && LaserCharges > 0 && !isFiring && canFire)
        {
            //blinking = StartCoroutine(BlinkCharge(currentCharge));
            StartCoroutine(FireLasers());
        }
    }

    private IEnumerator FireLasers()
    {
        LaserCharges--;
        isFiring = true;
        var duration = cannonController.BeamStartFire();
        yield return new WaitForSeconds(duration);
       
        isFiring = false;
        // StopCoroutine(blinking);
        Destroy(currentCharge.gameObject);
    }

    private IEnumerator BlinkCharge(Image charge)
    {
        while(true)
        {
            charge.enabled = false;
            yield return new WaitForSeconds(0.5f);
            charge.enabled = true;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void AddCharge()
    {
        if (LaserCharges < MaxLaserCharges)
        {
            LaserCharges++;
        }
    }
}
