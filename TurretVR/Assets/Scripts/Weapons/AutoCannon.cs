using Forge3D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AutoCannon : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform muzzlePrefab;
    
    public virtual float Damage { get; set; }
    public float ProjectileSpeed { get; set; }
    public float fireSpeed;

    public float FireSpeed { get { return fireSpeed; } }

    public virtual void Fire(Transform socket)
    {
        Instantiate(muzzlePrefab, socket.position, socket.rotation);
        var projectile = Instantiate(projectilePrefab, socket.position, socket.rotation);
        projectile.SetValues(ProjectileSpeed, Damage);
    }
}
