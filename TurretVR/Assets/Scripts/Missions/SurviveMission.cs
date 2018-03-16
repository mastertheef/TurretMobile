using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurviveMission : MissionBase
{
    [SerializeField] private string description;
    public override string MissionDescription { get { return description; }  }

    public override bool Condition()
    {
        return GameManager.Instance.Player.GetComponent<PlayerHealth>().HitPoints > 0;
    }
}
