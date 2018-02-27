using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CantFire : BaseEffect {

    protected override string timerText
    {
        get
        {
            return "Cant fire for {0} sconds ";
        }
    }

    protected override void RevertEffect()
    {
        Turret.Instance.IsDamaged = false;
    }

    protected override void SetEffect()
    {
        Turret.Instance.IsDamaged = true;
        Turret.Instance.StopFiring();
    }
}
