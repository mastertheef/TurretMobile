using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPointer : MonoBehaviour {

    [SerializeField] private Image arrow;
    [SerializeField] private Image pointer;
    private bool indicatioEnabled = true;
    private Image actualArrow;
    private Image actualPointer;
    float scalefactor;

    // Use this for initialization
    void Start () {
        var Canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        var UI = Canvas.transform;

        actualArrow = Instantiate(arrow);
        actualArrow.transform.SetParent(UI);
        actualArrow.enabled = false;

        actualPointer = Instantiate(pointer);
        actualPointer.transform.SetParent(UI);
        actualPointer.enabled = false;

        scalefactor = Canvas.scaleFactor;

    }
	
	// Update is called once per frame
	void Update () {
        if (indicatioEnabled)
        {
            PlaceSprite(GetAngle());
        }
	}

    float GetAngle()
    {
        GameObject target = gameObject;
        float angle;
        float xDiff = target.transform.position.x - Camera.main.transform.transform.position.x;
        float zDiff = target.transform.position.z - Camera.main.transform.transform.position.z;

        angle = Mathf.Atan(xDiff / zDiff) * 180f / Mathf.PI;
        // tangent only returns an angle from -90 to +90.  we need to check if its behind us and adjust.
        if (zDiff < 0)
        {
            if (xDiff >= 0)
                angle += 180f;
            else
                angle -= 180f;
            //angle *= -1;
        }

        // this is our angle of rotation from 0->360
        float playerAngle = Camera.main.transform.eulerAngles.y;
        // we  need to adjust this to our -180->180 system.
        if (playerAngle > 180f)
            playerAngle = 360f - playerAngle;

        // now subtract the player angle to get our relative angle to target.
        angle -= playerAngle;

        // Make sure we didn't rotate past 180 in either direction
        if (angle < -180f)
            angle += 360;
        else if (angle > 180f)
            angle -= 360;

        // now we have our correct relative angle to the target between -180 and 180
        // Lets clamp it between -135 and 135
        Mathf.Clamp(angle, -135f, 135f);
        return angle;
    }
    void PlaceSprite(float angle)
    {
        if (!indicatioEnabled)
        {
            return;
        }

        var screenpos = Camera.main.WorldToScreenPoint(transform.position);

        if (screenpos.z > 0 &&
            screenpos.x > 0 && screenpos.x < Screen.width &&
            screenpos.y > 0 && screenpos.y < Screen.height)
        {
            actualPointer.GetComponent<RectTransform>().anchoredPosition = screenpos;
            
            actualPointer.enabled = true;
            actualArrow.enabled = false;
        }
        else
        {

            // Get half the Images width and height to adjust it off the screen edge;
            RectTransform arrowRect = actualArrow.GetComponent<RectTransform>();
            float halfImageWidth = arrowRect.sizeDelta.x / 2f;
            float halfImageHeight = arrowRect.sizeDelta.y / 2f;

            // Get Half the ScreenHeight and Width to position the image
            float halfScreenWidth = (float)Screen.width / 2f;
            float halfScreenHeight = (float)Screen.height / 2f;

            float xPos = 0f;
            float yPos = 0f;

            // Left side of screen;
            if (angle < -45)
            {
                xPos = -halfScreenWidth + halfImageWidth;
                // Ypos can go between +ScreenHeight/2  down to -ScreenHeight/2
                // angle goes between -45 and -135
                // change angle to a value between 0f and 1.0f and Lerp on that
                float normalizedAngle = (angle + 45f) / -90f;
                yPos = Mathf.Lerp(halfScreenHeight, -halfScreenHeight, normalizedAngle);
                // at the top of the screen we need to move the image down half its height
                // at the bottom of the screen we need to move it up half its height
                // in the middle we need to do nothing. so we lerp on the angle again
                float yImageOffset = Mathf.Lerp(-halfImageHeight, halfImageHeight, normalizedAngle);
                yPos += yImageOffset;

            }
            // top of screen
            else if (angle < 45)
            {
                yPos = halfScreenHeight - halfImageHeight;
                float normalizedAngle = (angle + 45f) / 90f;
                xPos = Mathf.Lerp(-halfScreenWidth, halfScreenWidth, normalizedAngle);
                float xImageOffset = Mathf.Lerp(halfImageWidth, -halfImageWidth, normalizedAngle);
                xPos += xImageOffset;
            }
            // right side of screen
            else
            {
                xPos = halfScreenWidth - halfImageWidth;
                float normalizedAngle = (angle - 45) / 90f;
                yPos = Mathf.Lerp(halfScreenHeight, -halfScreenHeight, normalizedAngle);
                float yImageOffset = Mathf.Lerp(-halfImageHeight, halfImageHeight, normalizedAngle);
                yPos += yImageOffset;
            }

            arrowRect.anchoredPosition = new Vector3(xPos, yPos, 0);
            // UI rotation is backwards from our system.  Positive angles go counterclockwise
            arrowRect.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
            actualArrow.enabled = true;
            actualPointer.enabled = false;
        }

    }

    public void RemoveIndication()
    {
        indicatioEnabled = false;
        if (actualPointer != null)
            Destroy(actualPointer.gameObject);

        if (actualArrow != null)
            Destroy(actualArrow.gameObject);
    }
}
