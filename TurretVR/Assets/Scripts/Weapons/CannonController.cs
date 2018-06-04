using Forge3D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour {

    [SerializeField] List<CannonInfo> cannons;
    [SerializeField] Transform[] Sockets;

    public int currentCannon = 0;
    public int currentBeamCannon = 6;
    private int curSocket = 0;
    private int timerId = -1;

    public void Start()
    {
        foreach (var cannon in cannons)
        {
            cannon.cannon.Damage = cannon.Damage;
            cannon.cannon.fireSpeed = cannon.FireSpeed;
            cannon.cannon.ProjectileSpeed = cannon.ProjectileSpeed;
        }
    }

    public void StartFire()
    {
        
        Fire();
        timerId = F3DTime.time.AddTimer(cannons[currentCannon].FireSpeed, Fire);
        
    }

    public float BeamStartFire()
    {
        for (int i = 0; i< Sockets.Length; i++)
        {
            cannons[currentBeamCannon].cannon.Fire(Sockets[i]);
        }
        return (cannons[currentBeamCannon].cannon as BeamCannon).Duration;
    }


    private void Fire()
    {
        cannons[currentCannon].cannon.Fire(Sockets[curSocket]);
        AdvanceSocket();
    }

    public void StopFire()
    {
        F3DTime.time.RemoveTimer(timerId);
    }

    void AdvanceSocket()
    {
        curSocket++;
        if (curSocket >= Sockets.Length)
            curSocket = 0;
    }
}
