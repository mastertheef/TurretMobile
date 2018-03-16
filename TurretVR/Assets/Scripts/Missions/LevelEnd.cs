using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var levelSuccess = MissionsManager.Instance.CheckAllConditions();

            if (levelSuccess)
            {
                Debug.Log("Level Success");
                // save level progression somwewhere
            }
            else
            {
                Debug.Log("Level Fail");
            }
            SceneController.Instance.FinalScore = GameManager.Instance.Score;
            SceneController.Instance.FadeAndLoadScene("Score");
        }
    }
}
