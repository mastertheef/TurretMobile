using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    [SerializeField] GameObject firstButton;
    [SerializeField] Text ScoreLabel;
    [SerializeField] Text GyroLabel;
        // Use this for initialization
	void Start () {
        EventSystem.current.SetSelectedGameObject(firstButton);
        if (SystemInfo.supportsGyroscope)
        {
            GyroLabel.text = "Gyro is supported";
        }
        else
        {
            GyroLabel.text = "Gyro is not supported";
            GyroLabel.color = Color.red;
        }
        if (ScoreLabel != null)
        {
            ScoreLabel.text = string.Format("You: {0}", SceneController.Instance.FinalScore);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.touches.Length > 0)
        {
            EventSystem.current.SetSelectedGameObject(firstButton);
        }
	}

    public void Play()
    {
        SceneController.Instance.FadeAndLoadScene("LevelSelect");
    }
}
