using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class RepareModel : MonoBehaviour {

    [SerializeField] Transform greenBeemPrefab;
    [SerializeField] Button fireButton;
    [SerializeField] Button laserButton;
    [SerializeField] Button rocketButton;
    [SerializeField] float harvestFixDistance = 200f;
    [SerializeField] float harvestTime = 5f;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Transform[] sockets;
    

    bool isWorking = false;
    RaycastHit hitPoint;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (!SystemInfo.supportsGyroscope)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Physics.Raycast(transform.position, transform.forward, out hitPoint, harvestFixDistance, layerMask);

                if (hitPoint.transform.gameObject != null)
                {
                    foreach (var socket in sockets)
                    {
                        // TODO: destroy it when time is up or distance is too big
                        var beam = Instantiate(greenBeemPrefab, socket.transform.position, socket.transform.rotation, socket);
                    }
                }
            }
        }
        else
        {
            if (CrossPlatformInputManager.GetButton("Fire"))
            {

            }

            if (!CrossPlatformInputManager.GetButton("Fire"))
            {
            }
        }
    }

    public void Activate()
    {
        fireButton.GetComponentInChildren<Text>().text = "Fix/Harvest";
        laserButton.gameObject.SetActive(false);
        rocketButton.gameObject.SetActive(false);
    }

    private void FixOrHarvest(RaycastHit hitPoint)
    {

    }
}
