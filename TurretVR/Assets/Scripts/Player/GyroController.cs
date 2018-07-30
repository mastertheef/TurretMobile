// ***********************************************************
// Written by Heyworks Unity Studio http://unity.heyworks.com/
// ***********************************************************
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Gyroscope controller that works with any device orientation.
/// </summary>
public class GyroController : MonoBehaviour 
{
	#region [Private fields]

    [SerializeField] private float maxEulerAngle = 10f;
    [SerializeField] private float distance = 2000f;
    [SerializeField] private RectTransform aim;
    
   // [SerializeField] private float turretRotationLimitX = 0.05f;

    private bool gyroEnabled = true;
	private const float lowPassFilterFactor = 0.2f;

	private readonly Quaternion baseIdentity =  Quaternion.Euler(90, 0, 0);
	private readonly Quaternion landscapeRight =  Quaternion.Euler(0, 0, 90);
	private readonly Quaternion landscapeLeft =  Quaternion.Euler(0, 0, -90);
	private readonly Quaternion upsideDown =  Quaternion.Euler(0, 0, 180);
	
	private Quaternion cameraBase =  Quaternion.identity;
	private Quaternion calibration =  Quaternion.identity;
	private Quaternion baseOrientation =  Quaternion.Euler(90, 0, 0);
	private Quaternion baseOrientationRotationFix =  Quaternion.identity;

	private Quaternion referanceRotation = Quaternion.identity;
	private bool debug = false;
    private float yaw = 0f;
    private float pitch = 0f;
    private Vector3 aimPosition;
    private Vector3 worldAimPosition;
    private Quaternion playerRotation;
    private Vector3 playerPosition;

    #endregion

    #region [Unity events]

    protected void Start () 
	{
        gyroEnabled = SystemInfo.supportsGyroscope;
        
        AttachGyro();
	}

	protected void Update() 
	{
        if (gyroEnabled)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                cameraBase * (ConvertRotation(referanceRotation * Input.gyro.attitude) * GetRotFix()), lowPassFilterFactor);

            
        }
        else if (Input.GetKey(KeyCode.LeftAlt))
        {
            yaw += 2 * Input.GetAxis("Mouse X");
           // yaw = Mathf.Clamp(yaw, -1 * maxEulerAngle, maxEulerAngle);
            pitch += -2 * Input.GetAxis("Mouse Y");
           // pitch = Mathf.Clamp(pitch, -1 * maxEulerAngle, maxEulerAngle);
            transform.rotation = Quaternion.Slerp(transform.rotation, cameraBase * Quaternion.Euler(pitch, yaw, 0), lowPassFilterFactor); 
        }

        worldAimPosition = transform.TransformPoint(Vector3.forward * distance);
        aimPosition = GameManager.Instance.GameCamera.WorldToViewportPoint(worldAimPosition);
        aimPosition.x = Mathf.Clamp(aimPosition.x, 0.03f, 0.95f);
        aimPosition.y = Mathf.Clamp(aimPosition.y, 0.03f, 0.95f);
        aimPosition.z = 0;
        
        aim.position = GameManager.Instance.GameCamera.ViewportToScreenPoint(aimPosition);
        playerRotation = GameManager.Instance.Player.transform.rotation;
        playerPosition = GameManager.Instance.Player.transform.position;
        //worldAimPosition = GameManager.Instance.GameCamera.ViewportToWorldPoint(aim.position);
        GameManager.Instance.Player.transform.rotation = Quaternion.RotateTowards(playerRotation, Quaternion.LookRotation(worldAimPosition - playerPosition), 0.8f);
    }

	#endregion

	#region [Public methods]

	/// <summary>
	/// Attaches gyro controller to the transform.
	/// </summary>
	private void AttachGyro()
	{
        if (gyroEnabled)
        {
            Input.gyro.enabled = true;
            ResetBaseOrientation();
            UpdateCalibration(true);
            UpdateCameraBaseRotation(true);
            RecalculateReferenceRotation();
        }
	}

	/// <summary>
	/// Detaches gyro controller from the transform
	/// </summary>
	private void DetachGyro()
	{
		gyroEnabled = false;
	}

	#endregion

	#region [Private methods]

	/// <summary>
	/// Update the gyro calibration.
	/// </summary>
	private void UpdateCalibration(bool onlyHorizontal)
	{
		if (onlyHorizontal)
		{
			var fw = (Input.gyro.attitude) * (-Vector3.forward);
			fw.z = 0;
			if (fw == Vector3.zero)
			{
				calibration = Quaternion.identity;
			}
			else
			{
				calibration = (Quaternion.FromToRotation(baseOrientationRotationFix * Vector3.up, fw));
			}
		}
		else
		{
			calibration = Input.gyro.attitude;
		}
	}
	
	/// <summary>
	/// Update the camera base rotation.
	/// </summary>
	/// <param name='onlyHorizontal'>
	/// Only y rotation.
	/// </param>
	private void UpdateCameraBaseRotation(bool onlyHorizontal)
	{
		if (onlyHorizontal)
		{
			var fw = transform.forward;
			fw.y = 0;
			if (fw == Vector3.zero)
			{
				cameraBase = Quaternion.identity;
			}
			else
			{
				cameraBase = Quaternion.FromToRotation(Vector3.forward, fw);
			}
		}
		else
		{
			cameraBase = transform.rotation;
		}
	}
	
	/// <summary>
	/// Converts the rotation from right handed to left handed.
	/// </summary>
	/// <returns>
	/// The result rotation.
	/// </returns>
	/// <param name='q'>
	/// The rotation to convert.
	/// </param>
	private static Quaternion ConvertRotation(Quaternion q)
	{
		return new Quaternion(q.x, q.y, -q.z, -q.w);	
	}
	
	/// <summary>
	/// Gets the rot fix for different orientations.
	/// </summary>
	/// <returns>
	/// The rot fix.
	/// </returns>
	private Quaternion GetRotFix()
	{
#if UNITY_3_5
		if (Screen.orientation == ScreenOrientation.Portrait)
			return Quaternion.identity;
		
		if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.Landscape)
			return landscapeLeft;
				
		if (Screen.orientation == ScreenOrientation.LandscapeRight)
			return landscapeRight;
				
		if (Screen.orientation == ScreenOrientation.PortraitUpsideDown)
			return upsideDown;
		return Quaternion.identity;
#else
		return Quaternion.identity;
#endif
	}
	
	/// <summary>
	/// Recalculates reference system.
	/// </summary>
	private void ResetBaseOrientation()
	{
		baseOrientationRotationFix = GetRotFix();
		baseOrientation = baseOrientationRotationFix * baseIdentity;
	}

	/// <summary>
	/// Recalculates reference rotation.
	/// </summary>
	private void RecalculateReferenceRotation()
	{
		referanceRotation = Quaternion.Inverse(baseOrientation)*Quaternion.Inverse(calibration);
	}

	#endregion
}
