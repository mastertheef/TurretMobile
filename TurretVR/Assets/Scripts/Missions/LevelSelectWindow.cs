using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectWindow : MonoBehaviour {

    [SerializeField] private RectTransform LevelContainer;
    [SerializeField] private LevelButton levelButtonPrefab;

    // Use this for initialization
    void Start () {
        var buttons = LevelContainer.GetComponentsInChildren<LevelButton>();
        foreach (var button in buttons)
        {
            Destroy(button.gameObject);
        }

        var levels = LevelManager.Instance.GetLevels();

        foreach (var level in levels)
        {
            var newButton = Instantiate(levelButtonPrefab, LevelContainer);
            newButton.ConfigureButton(level);
        }
    }
}
