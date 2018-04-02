using Greyman;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IndicatorManager : Singleton<IndicatorManager>
{
    [SerializeField] private OffScreenIndicator indicator;
    [SerializeField] private Sprite onScreenIndicator;
    [SerializeField] private Sprite offScreenIndicator;

    public Sprite OffScreenIndicatorSprite { get { return offScreenIndicator; } }

    // Use this for initialization
    void Awake()
    {
        int countTargets = GameManager.Instance.MaxEnemies;
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


        indicator.indicators.Add(ind);
        var indicatorTarget = new FixedTarget
        {
            target = target,
            indicatorID = indicator.indicators.Count
        };
        indicator.targets.Add(indicatorTarget);

        indicator.AddIndicator(target, 0);

    }

    public void RemoveIndicator(Transform target)
    {
        indicator.RemoveIndicator(target);
    }
}
