using UnityEngine;
using System.Collections.Generic;
using UnityEngine.VR;
using System.Collections;

/// <summary>
/// Off screen indicator.
/// Classic wrapper, user doesn't need to worry about implementation
/// </summary>
namespace Greyman
{
    public class OffScreenIndicator : Singleton<OffScreenIndicator>
    {

        public bool enableDebug = true;
        public bool VirtualRealitySupported = false;
        public float VR_cameraDistance = 5;
        public float VR_radius = 1.8f;
        public float VR_indicatorScale = 0.1f;
        public GameObject canvas;
        public int Canvas_circleRadius = 5; //size in pixels
        public int Canvas_border = 10; // when Canvas is Square pixels in border
        public int Canvas_indicatorSize = 100; //size in pixels
        public List<Indicator> indicators;
        public List<FixedTarget> targets;

        [SerializeField] private Camera camera;
        //public 
        private OffScreenIndicatorManager manager;

        void Awake()
        {

            indicators = new List<Indicator>();
            targets = new List<FixedTarget>();
            /*
			if (VRSettings.enabled){
				VR = true;
			} else {
				VR = false;
			}
			*/

            manager = gameObject.AddComponent<MyIndicatorManager>();
            (manager as MyIndicatorManager).indicatorsParentObj = canvas;
            (manager as MyIndicatorManager).border = Canvas_border;
            (manager as MyIndicatorManager).circleRadius = Canvas_circleRadius;
            (manager as MyIndicatorManager).indicatorSize = Canvas_indicatorSize;
            (manager as MyIndicatorManager).camera = camera;
            manager.indicators = indicators;
            manager.enableDebug = enableDebug;
            manager.CheckFields();
            foreach (FixedTarget target in targets)
            {
                if (target != null)
                {
                    AddIndicator(target.target, target.indicatorID);
                }
            }
        }

        public void AddIndicator(Transform target, int indicatorID)
        {
            manager.AddIndicator(target, indicatorID);
        }

        public void RemoveIndicator(Transform target)
        {
            manager.RemoveIndicator(target);
        }

    }

    /// <summary>
    /// Indicator.
    /// References and colors for indicator sprites
    /// </summary>
    [System.Serializable]
    public class Indicator
    {
        public Sprite onScreenSprite;
        public Color onScreenColor = Color.white;
        public bool onScreenRotates;
        public Sprite offScreenSprite;
        public Color offScreenColor = Color.white;
        public bool offScreenRotates;
        public Vector3 targetOffset;
        /// <summary>
        /// Both sprites need to have the same transition
        /// aswell both sprites need to have the same duration.
        /// </summary>
        public Transition transition;
        public float transitionDuration = 1;
        [System.NonSerialized]
        public bool showOnScreen;
        [System.NonSerialized]
        public bool showOffScreen;

        public enum Transition
        {
            None,
            Fading,
            Scaling
        }
    }

    [System.Serializable]
    public class FixedTarget
    {
        public Transform target;
        public int indicatorID;
    }
}