using UnityEngine;

public class Bow : RangedWeapon
{
    public GameObject _projectilePrefab;
    public Transform _firePoint;

    public override float projectileSpeed => 20f;
    public override GameObject projectilePrefab => _projectilePrefab;
    public override Transform firePoint => _firePoint;
}
