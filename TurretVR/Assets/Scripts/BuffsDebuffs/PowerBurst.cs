using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBurst : BaseEffect {
    [SerializeField] private float value;
    [SerializeField] private int shipsToKill = 5;
    [SerializeField] private float enlargeLaser = 0.1f;
    
    private Vector3 startSize;

    private void Start()
    {
        base.Start();
        
    }

    protected override string timerText
    {
        get
        {
            return "Power burst for {0} sconds";
        }
    }

    protected override void RevertEffect()
    {
        base.RevertEffect();
        Turret.Instance.ProjectileAdditionalDamage = 0;
        Turret.Instance.ProjectileAdditionalScale = 0;
    }

    protected override void SetEffect()
    {
        Turret.Instance.ProjectileAdditionalDamage = value;
        Turret.Instance.ProjectileAdditionalScale = enlargeLaser;
    }

    public override bool? Condition()
    {
        if (GameManager.Instance.ShipsCount >= shipsToKill)
        {
            GameManager.Instance.ShipsCount = 0;
            return true;
        }
        return false;
    }

}
