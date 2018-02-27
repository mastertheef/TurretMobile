using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseEffect : MonoBehaviour
{
    [SerializeField] protected float duration = 3f;
    [SerializeField] protected GameObject Prefab;
    private GameObject instance;
    protected Text label;
    public float Ttl { get; set; }

    protected abstract string timerText { get; }

    // Use this for initialization
    protected void Start()
    {
        var ui = GameObject.FindGameObjectWithTag("Canvas").transform;
        instance = Instantiate(Prefab, ui);
        label = instance.GetComponentsInChildren<Text>().Where(x => x.name == "Timer").FirstOrDefault();

        SetEffect();
        Ttl = duration;
        InvokeRepeating("CountDown", 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Ttl <= 0)
        {
            RevertEffect();
            Destroy(instance);
            Destroy(this);
        }
    }

    public void ResetTtl()
    {
        Ttl = duration;
    }

    private void OnDestroy()
    {
        if (label != null)
        {
            Destroy(label.gameObject);
        }
    }

    protected void CountDown()
    {
        label.enabled = true;
        if (--Ttl == 0)
        {
            CancelInvoke("CountDown");
            label.enabled = false;
        };
        label.text = GameManager.Instance.NiceTime(Ttl);
    }

    public virtual bool? Condition()
    {
        return null;
    }
    protected abstract void SetEffect();
    protected virtual void RevertEffect()
    {
        EffectSystem.Instance.effects[this.tag] = null;
    }
}
