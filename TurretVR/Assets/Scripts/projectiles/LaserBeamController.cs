using Forge3D;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class LaserBeamController : Singleton<LaserBeamController> {

    [SerializeField] private List<LaserBeam> lasers;
    [SerializeField] private int MaxLaserCharges = 3;
    [SerializeField] private float ChargeDuration = 2f;

    [SerializeField] private RectTransform ChargesPanel;
    [SerializeField] private Image ChareImagePrefab;

    private int LaserCharges;

    private bool isFiring = false;
    private bool canFire = true;
    private Coroutine blinking;
    private Image currentCharge;
    private List<Image> chargeImages;
    private F3DFXController fxController;


    // Use this for initialization
    void Start () {
        LaserCharges = MissionsManager.Instance.StartLaserCount;
        chargeImages = new List<Image>();
        for (int i = 0; i<LaserCharges; i++)
        {
             chargeImages.Add(Instantiate(ChareImagePrefab, ChargesPanel));
        }
        fxController = GetComponent<F3DFXController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (CrossPlatformInputManager.GetButtonDown("Laser") && LaserCharges > 0 && !isFiring && canFire)
        {
            currentCharge = chargeImages.Last();
            blinking = StartCoroutine(BlinkCharge(currentCharge));
            StartCoroutine(FireLasers());
        }
    }

    private IEnumerator FireLasers()
    {
        LaserCharges--;
        isFiring = true;
        //lasers.ForEach(x => x.Fire());
        fxController.FireLaser();
        yield return new WaitForSeconds(ChargeDuration);
        //lasers.ForEach(x => x.StopFire());
        //fxController.StopLaser();

        isFiring = false;
        StopCoroutine(blinking);
        chargeImages.Remove(currentCharge);
        Destroy(currentCharge.gameObject);
    }

    private void StopFiring()
    {
        lasers.ForEach(x => x.StopFire());
        isFiring = false;
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
            chargeImages.Add(Instantiate(ChareImagePrefab, ChargesPanel));
        }
    }
}
