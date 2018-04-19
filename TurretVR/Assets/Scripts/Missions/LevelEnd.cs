using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MotherShip")
        {
            DTO.MissionSuccess = MissionsManager.Instance.CheckAllConditions();
            DTO.CurrentRating = MissionsManager.Instance.GetRating();
            SceneController.Instance.FadeAndLoadScene("LevelEnd");
        }
    }
}
