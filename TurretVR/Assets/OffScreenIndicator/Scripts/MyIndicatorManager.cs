using Greyman;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyIndicatorManager : OffScreenIndicatorManagerCanvas
{
    private Sprite offScreenSprite;

    private void Start()
    {
        //arrowIndicators = new List<ArrowIndicator>();
        offScreenSprite = IndicatorManager.Instance.OffScreenIndicatorSprite;
    }

    public override void AddIndicator(Transform target, int indicatorID)
    {
        //if (indicatorID > indicators.Count)
        //{
        //    Debug.LogError("Indicator ID not valid. Check Off Screen Indicator Indicators list.");
        //    return;
        //}
        if (!ExistsIndicator(target))
        {
            ArrowIndicatorCanvas newArrowIndicator = new ArrowIndicatorCanvas();
            newArrowIndicator.target = target;
            newArrowIndicator.arrow = new GameObject();
            newArrowIndicator.arrow.transform.SetParent(indicatorsParentObj.transform);
            newArrowIndicator.arrow.name = "Indicator";
            newArrowIndicator.arrow.transform.SetAsFirstSibling();
            newArrowIndicator.arrow.transform.localScale = Vector3.one;
            newArrowIndicator.arrow.AddComponent<Image>();
            newArrowIndicator.indicator = indicators[indicatorID];
            newArrowIndicator.arrow.GetComponent<Image>().sprite = offScreenSprite;
            newArrowIndicator.arrow.GetComponent<RectTransform>().sizeDelta = new Vector2(indicatorSize, indicatorSize);
            newArrowIndicator.arrow.GetComponent<Image>().color = Color.red;
            newArrowIndicator.arrow.SetActive(true);
            newArrowIndicator.onScreen = true;
            arrowIndicators.Add(newArrowIndicator);
        }
        else
        {
            Debug.LogWarning("Target already added: " + target.name);
        }
    }
}
