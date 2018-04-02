﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEndManager : MonoBehaviour {

    [SerializeField] private Text StatusLabel;
    [SerializeField] private Text ScoreLabel;
    [SerializeField] private Image[] rewards;

	// Use this for initialization
	void Start () {
        ScoreLabel.text = string.Format("Score: {0}", GameManager.Instance.Score.ToString());

		if (MissionsManager.Instance.CheckAllConditions())
        {
            StatusLabel.text = "Mission Complete";
            int rating = MissionsManager.Instance.GetRating();
            for (int i = 0; i< rating; i++)
            {
                rewards[i].gameObject.SetActive(true);
            }

            LevelManager.Instance.UnlockNext();
            LevelManager.Instance.SaveCurrent(GameManager.Instance.Score, rating);
        }
        else
        {
            StatusLabel.text = "Mission Failed";
            StatusLabel.color = Color.red;
            LevelManager.Instance.SaveCurrent(GameManager.Instance.Score, 0);
        }
    }
	
	public void LoadLevelSelect()
    {
        SceneController.Instance.FadeAndLoadScene("LevelSelect");
    }
}