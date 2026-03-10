using UnityEngine;

public abstract class BaseWeaponScript : MonoBehaviour
{
    public float damage = 20f;
    public abstract void Attack();
}
