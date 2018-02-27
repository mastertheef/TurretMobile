using Greyman;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IndicatorManager : Singleton<IndicatorManager>
{

    private OffScreenIndicator indicator;
    [SerializeField] private Sprite onScreenIndicator;
    [SerializeField] private Sprite offScreenIndicator;

    public Sprite OffScreenIndicatorSprite { get { return offScreenIndicator; } }

    // Use this for initialization
    void Awake()
    {
        indicator = OffScreenIndicator.Instance;

        int countTargets = GameManager.Instance.MaxEnemies;
        //indicator.Targets 
        //indicator.Indicators = 
    }

    public void AddIndicator(Transform target)
    {
        var ind = new Indicator()
        {
            offScreenColor = Color.red,
            onScreenColor = Color.red,
            onScreenSprite = onScreenIndicator,
            offScreenSprite = offScreenIndicator,
            showOffScreen = true,
            showOnScreen = true,
            offScreenRotates = true
        };


        OffScreenIndicator.Instance.indicators.Add(ind);
        var indicatorTarget = new FixedTarget
        {
            target = target,
            indicatorID = OffScreenIndicator.Instance.indicators.Count
        };
        OffScreenIndicator.Instance.targets.Add(indicatorTarget);

        OffScreenIndicator.Instance.AddIndicator(target, 0);

    }

    public void RemoveIndicator(Transform target)
    {
        indicator.RemoveIndicator(target);
    }
}
