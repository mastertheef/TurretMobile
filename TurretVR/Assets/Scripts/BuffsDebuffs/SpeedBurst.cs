using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBurst : BaseEffect
{
    [SerializeField] private float value;
    [SerializeField] private float asteroidsToBlow = 5;
    protected override string timerText
    {
        get
        {
            return "Fire speed improved for {0} seconds";
        }
    }

    protected override void RevertEffect()
    {
        Turret.Instance.ShootCounterSpeedUp = 0;
        Turret.Instance.RestartIfFiring();
    }

    protected override void SetEffect()
    {
        Turret.Instance.ShootCounterSpeedUp = value;
        Turret.Instance.RestartIfFiring();
    }

    public override bool? Condition()
    {
        if (GameManager.Instance.AsteroidsCount >= asteroidsToBlow)
        {
            GameManager.Instance.AsteroidsCount = 0;
            return true;
        }
        return false;
    }
}
