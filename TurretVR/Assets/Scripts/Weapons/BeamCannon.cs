using Forge3D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamCannon : AutoCannon {

    [SerializeField] GameObject beamPrefab;
    [SerializeField] bool isInBattleMode = true;
    public override float Damage
    {
        get
        {
            return base.Damage;
        }

        set
        {
            base.Damage = value;
            beamPrefab.GetComponent<F3DBeam>().damage = value;
        }
    }

    public float Duration { get { return beamPrefab.GetComponent<AutoDestroy>().TTL; } }

    public override void Fire(Transform socket)
    {
        var beam = Instantiate(beamPrefab, socket.transform.position, socket.transform.rotation, socket);
        if (GameManager.Instance.IsInBattleMode)
        {
            beam.GetComponent<F3DBeam>().ContactAction = MakeDamage;
        }
        else
        {
            beam.GetComponent<F3DBeam>().ContactAction = FixOrHarvest;
        }
    }

    public void MakeDamage(RaycastHit hitPoint)
    {
        if (hitPoint.transform.gameObject != null)
        {
            var Health = hitPoint.transform.gameObject.GetComponentInParent<Health>();
            if (Health != null)
            {
                Health.TakeDamage(Damage);
            }
        }
    }

    public void FixOrHarvest(RaycastHit hitPoint)
    {

    }
}
