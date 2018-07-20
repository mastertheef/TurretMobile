using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixLaser : MonoBehaviour {

    LineRenderer lineRenderer;
    float animateUVTime = -6;
    [SerializeField] float maxLength = 100;
    [SerializeField] Transform Impact;
    [SerializeField] Transform Muzzle;

    public Vector3 targetPosition;
    public Transform startPosition;
    // Use this for initialization
    void Start () {
        lineRenderer = GetComponent<LineRenderer>();
        Impact.position = targetPosition;
    }
	
	// Update is called once per frame
	void Update () {
        animateUVTime += Time.deltaTime;

        if (animateUVTime > 1.0f)
            animateUVTime = 0f;

        lineRenderer.material.SetTextureOffset("_MainTex", new Vector2(animateUVTime, 0f));

        lineRenderer.SetPositions(new Vector3[] { startPosition.position, targetPosition });
        Muzzle.position = startPosition.position;
        //lineRenderer.SetPosition(1, targetPosition);
    }
}
