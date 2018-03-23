﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionsManager : Singleton<MissionsManager> {

    [SerializeField] private List<MissionBase> missions;
    [SerializeField] private bool canUseLaser = false;
    [SerializeField] private bool canUseRockets = false;
    [SerializeField] private int startRocketsCount = 0;
    [SerializeField] private int startLaserCount = 0;

    public bool CanUseLaser { get { return canUseLaser; } }
    public bool CanUseRockets { get { return canUseRockets; } }
    public int StartRocketsCount { get { return startRocketsCount;  } }
    public int StartLaserCount { get { return startLaserCount; } }

    public bool CheckAllConditions()
    {
        bool currentState = true;
        foreach (var mission in missions)
        {
            currentState = currentState && mission.Condition();
        }

        return currentState;
    }
}