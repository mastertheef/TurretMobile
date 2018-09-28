using UnityEngine;
using System;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class CustomPointer : MonoBehaviour
{

    public Texture pointerTexture; //The image for the pointer, generally a crosshair or dot.

    public bool use_mouse_input = false; //Pointer will be controlled by the mouse.
    public bool use_gamepad_input = false; //Pointer will be controlled by a joystick
    public bool use_mobile_input = true;
    //public bool use_accelerometer_input = false;	//Pointer will be controlled by accelerometer
    public bool pointer_returns_to_center = false; //Pointer will drift to the center of the screen (Use this for joysticks)
    public bool instant_snapping = false; //If the pointer returns to the center, this will make it return to the center instantly when input is idle. Only works for joysticks
    public float center_speed = 5f; //How fast the pointer returns to the center.

    public bool center_lock = false; //Pointer graphic will be locked to the center. Also affects shooting raycast (always shoots to the center of the screen)

    public bool invert_y_axis = false; //Inverts the y axis.


    public float deadzone_radius = 0f; //Deadzone in the center of the screen where the pointer can move without affecting the ship's movement.

    public float thumbstick_speed_modifier = 1f; //Speed multiplier for joysticks.
    public float mouse_sensitivity_modifier = 15f; //Speed multiplier for the mouse.

    public static Vector2 pointerPosition; //Position of the pointer in screen coordinates.

    [HideInInspector]
    public Rect deadzone_rect; //Rect representation of the deadzone.

    public static CustomPointer instance; //The instance of this class (Should only be one)


    private Vector2 startTouch, touchDelta, pointerStart;
    private bool isTouching = false;
    static Touch? controllingTouch = null;

    private Vector2 mouseStart, mouseDelta;

    Toggle pointerFormulaToggle;

    private Vector2 oldPointerPosition, newPointerPosition, pointerShift;
    // Use this for initialization

    void Awake()
    {
        pointerPosition = new Vector2(Screen.width / 2, Screen.height / 2); //Set pointer position to center of screen
        pointerStart = pointerPosition;
        instance = this;
    }

    void Start()
    {

        //Uncomment for Unity 5 to get rid of the warnings.
        //Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        use_mobile_input = true;
        pointerFormulaToggle = GameObject.Find("PointerFormula").GetComponent<Toggle>();
        //Screen.lockCursor = true;


        deadzone_rect = new Rect((Screen.width / 2) - (deadzone_radius), (Screen.height / 2) - (deadzone_radius), deadzone_radius * 2, deadzone_radius * 2);

        if (pointerTexture == null)
            Debug.LogWarning("(FlightControls) Warning: No texture set for the custom pointer!");


        if (!use_mouse_input && !use_gamepad_input && !use_mobile_input)
            Debug.LogError("(FlightControls) No input method selected! See the Custom Pointer script on the Main Camera and select either mouse or gamepad.");
    }

    // Update is called once per frame
    void Update()
    {

        if (use_mouse_input)
        {

            float x_axis = Input.GetAxis("Mouse X");
            float y_axis = Input.GetAxis("Mouse Y");


            if (invert_y_axis)
                y_axis = -y_axis;

            //Add the input to the pointer's position
            //pointerPosition += new Vector2(x_axis * mouse_sensitivity_modifier,
            //                               y_axis * mouse_sensitivity_modifier);



            if (Input.GetMouseButtonDown(0))
            {
                mouseStart = Input.mousePosition;
                isTouching = true;
                oldPointerPosition = newPointerPosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                mouseStart = mouseDelta = Vector2.zero;
                isTouching = false;

                newPointerPosition = oldPointerPosition = Vector2.zero;
            }

            if (Input.GetMouseButton(0))
            {
                oldPointerPosition = newPointerPosition;
                newPointerPosition = (Vector2)Input.mousePosition;
                mouseDelta = (Vector2)Input.mousePosition - mouseStart;

                var temp = (newPointerPosition - oldPointerPosition);
                var maxSpeed = 50;
                var pointerDiff = new Vector2(Mathf.Clamp(temp.x, -maxSpeed, maxSpeed) / maxSpeed, Mathf.Clamp(temp.y, -maxSpeed, maxSpeed) / maxSpeed);


                pointerShift = new Vector2(
                    (float)Math.Sin(pointerDiff.x * Math.PI / 2)
                    ,
                    (float)Math.Sin(pointerDiff.y * Math.PI / 2)) * 2;

                pointerPosition = pointerFormulaToggle.isOn
                    ? pointerStart + mouseDelta * 5 // linear
                    : pointerPosition + pointerShift * maxSpeed; // square


                //pointerStart + mouseDelta * mouseDelta * mouseDelta.normalized / 20;
                //pointerPosition = pointerStart + mouseDelta * 5;
            }

            pointer_returns_to_center = !isTouching;

        }
        else if (use_gamepad_input)
        {

            float x_axis = Input.GetAxis("Horizontal");
            float y_axis = Input.GetAxis("Vertical");

            if (invert_y_axis)
                y_axis = -y_axis;


            pointerPosition += new Vector2(x_axis * thumbstick_speed_modifier * Mathf.Pow(Input.GetAxis("Horizontal"), 2),
                                               y_axis * thumbstick_speed_modifier * Mathf.Pow(Input.GetAxis("Vertical"), 2));

        }/* else if (use_accelerometer_input) {
			//WARNING: UNTESTED.
			//This /should/ be fairly close to working, though.
			//I would have tested this, but apparently Unity couldn't detect my Windows Phone 8 SDK.
			
			//Even though it's untested, the priciples of control are probably going to be very similar to the above two.
			
			float x_axis = Input.acceleration.x;
			float y_axis = -Input.acceleration.z;
		
			pointerPosition += new Vector2(x_axis * thumbstick_speed_modifier * Mathf.Pow(Input.GetAxis("Horizontal"), 2),
			                               y_axis * thumbstick_speed_modifier * Mathf.Pow(Input.GetAxis("Vertical"), 2));
			
		
		
		}*/

        else if (use_mobile_input)
        {
            float x_axis = CrossPlatformInputManager.GetAxis("Horizontal");
            float y_axis = CrossPlatformInputManager.GetAxis("Vertical");

            if (invert_y_axis)
                y_axis = -y_axis;

            if (Input.touches.Count() > 0)
            {
                controllingTouch = GetControlligTouch();
                if (controllingTouch.HasValue)
                {
                    if (controllingTouch.Value.phase == TouchPhase.Began)
                    {
                        isTouching = true;
                        startTouch = controllingTouch.Value.position;

                    }
                    else if (controllingTouch.Value.phase == TouchPhase.Ended || controllingTouch.Value.phase == TouchPhase.Canceled)
                    {
                        ResetTouch();
                    }

                    if (isTouching)
                    {
                        touchDelta = controllingTouch.Value.position - startTouch;
                        // pointerPosition =  pointerStart + touchDelta * touchDelta * touchDelta.normalized / 10;

                        pointerPosition = pointerFormulaToggle.isOn
                            ? pointerStart + touchDelta * 5 // linear
                            : pointerStart + touchDelta * touchDelta * touchDelta.normalized / 20; // square
                    }
                }
            }
            else
            {
                isTouching = false;
            }

            //pointerPosition += new Vector2(x_axis * thumbstick_speed_modifier * Mathf.Pow(CrossPlatformInputManager.GetAxis("Horizontal"), 2),
            //y_axis * thumbstick_speed_modifier * Mathf.Pow(CrossPlatformInputManager.GetAxis("Vertical"), 2));

            pointer_returns_to_center = !isTouching;

        }

        //If the pointer returns to the center of the screen and it's not in the deadzone...
        if (pointer_returns_to_center && !deadzone_rect.Contains(pointerPosition))
        {
            //If there's no input and instant snapping is on...
            if (CrossPlatformInputManager.GetAxis("Horizontal") == 0 && CrossPlatformInputManager.GetAxis("Vertical") == 0 && instant_snapping)
            {
                pointerPosition = new Vector2(Screen.width / 2, Screen.height / 2); //Place pointer at the center.


            }
            else
            {
                //Move pointer to the center (Will stop when it hits the deadzone)
                pointerPosition.x = Mathf.Lerp(pointerPosition.x, Screen.width / 2, center_speed * Time.deltaTime);
                pointerPosition.y = Mathf.Lerp(pointerPosition.y, Screen.height / 2, center_speed * Time.deltaTime);
            }
        }

        //Keep the pointer within the bounds of the screen.
        pointerPosition.x = Mathf.Clamp(pointerPosition.x, 0, Screen.width);
        pointerPosition.y = Mathf.Clamp(pointerPosition.y, 0, Screen.height);


    }



    void OnGUI()
    {
        //Draw the pointer texture.
        if (pointerTexture != null && !center_lock)
            GUI.DrawTexture(new Rect(pointerPosition.x - (pointerTexture.width / 2), Screen.height - pointerPosition.y - (pointerTexture.height / 2), pointerTexture.width, pointerTexture.height), pointerTexture);
        else
        {

            GUI.DrawTexture(new Rect((Screen.width / 2f) - (pointerTexture.width / 2), (Screen.height / 2f) - (pointerTexture.height / 2), pointerTexture.width, pointerTexture.height), pointerTexture);

        }
    }

    private void ResetTouch()
    {
        controllingTouch = null;
        startTouch = touchDelta = Vector2.zero;
        isTouching = false;
    }

    private Touch? GetControlligTouch()
    {
        for (int i = 0; i < Input.touches.Length; i++)
        {

            PointerEventData pointer = new PointerEventData(EventSystem.current);
            pointer.position = Input.GetTouch(i).position;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, raycastResults);

            if (!raycastResults.Any(x => x.gameObject.tag == "UI"))
            {
                return Input.GetTouch(i);
            }
        }

        return null;
    }

}
