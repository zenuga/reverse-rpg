using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class Magic : RangedWeapon
{
    public override void Init()
    {
        attackChargeTime = 0;
        attackCooldown = 1;
        baseAttackCooldown = 1;
        projectileSpeed = 40;
    }

    public override void Attack()
    {
        base.Attack();
    }

}
