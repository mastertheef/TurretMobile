using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReduceSpeed : BaseEffect {
    [SerializeField] private float speedReduction = 4f;

    protected override string timerText
    {
        get
        {
            return "Fire speed slower for {0} sconds ";
        }
    }

    protected override void RevertEffect()
    {
        base.RevertEffect();
        Turret.Instance.ShootCounterPenetration = 0;
        Turret.Instance.RestartIfFiring();
    }

    protected override void SetEffect()
    {
        Turret.Instance.ShootCounterPenetration = speedReduction;
        Turret.Instance.RestartIfFiring();
    }
}
