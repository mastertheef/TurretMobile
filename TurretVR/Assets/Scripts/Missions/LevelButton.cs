using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {
    [SerializeField] private Text LevelLabel;
    [SerializeField] private Text ScoreLabel;
    [SerializeField] private Image[] Rewards;
    [SerializeField] private Sprite unlockedSprite;
    [SerializeField] private Sprite lockedSprite;
    
    private string SceneName;

    public void ConfigureButton(LevelInfo info)
    {
        SceneName = info.SceneName;
        LevelLabel.text = info.DisplayName;
        if (info.Unlocked)
        {
            ScoreLabel.text = "Score: " + info.Score.ToString();
            GetComponent<Image>().sprite = unlockedSprite;
            
            for (int i = 0; i < info.rating; i++)
            {
                Rewards[i].gameObject.SetActive(true);
            }
        }
        else
        {
            GetComponent<Image>().sprite = lockedSprite;
        }

        GetComponent<Button>().onClick.AddListener(LoadLevel);
    }

    public void LoadLevel()
    {
        SceneController.Instance.FadeAndLoadScene(SceneName);
    }
}
