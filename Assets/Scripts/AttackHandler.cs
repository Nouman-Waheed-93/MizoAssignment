using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    [SerializeField]
    MeleeAttack punchAttack;
    [SerializeField]
    MeleeAttack kickAttack;

    [SerializeField]
    Transform meleePoint;
    [SerializeField]
    LayerMask targetLayer;
    [SerializeField]
    Camera shootingCamera;
    [SerializeField]
    float fireDamage = 1;
    [SerializeField]
    Transform gunNozzle;
    [SerializeField]
    GameObject BulletPrefab;

    
    public void ExecutePunch()
    {
        ExecuteMelee(punchAttack);
    }

    public void ExecuteKick()
    {
        ExecuteMelee(kickAttack);
    }

    public void Fire()
    {
        Ray ray = shootingCamera.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1000))
        {
            Health targetHealth;
            if(hit.collider.TryGetComponent<Health>(out targetHealth))
            {
                targetHealth.TakeDamage(fireDamage);
            }
        }
        Projectile bProjectile = Instantiate(BulletPrefab, gunNozzle.position, Quaternion.identity).GetComponent<Projectile>();
        bProjectile.transform.LookAt(ray.GetPoint(100));
        bProjectile.Shoot();
    }

    private void ExecuteMelee(MeleeAttack attack)
    {
        Collider[] effectedTargets = Physics.OverlapSphere(meleePoint.position, attack.effectRadius, targetLayer);
        foreach(Collider target in effectedTargets)
        {
            Health targetHealth;
            if(target.TryGetComponent(out targetHealth))
            {
                targetHealth.TakeDamage(attack.damage);
            }
        }
    }
}
