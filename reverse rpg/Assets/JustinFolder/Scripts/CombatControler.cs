using UnityEngine;

public class CombatControler : MonoBehaviour
{
    public BaseWeaponScript currentWeapon;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    void Attack()
    {
        if (currentWeapon != null)
        {
            currentWeapon.Attack();
        }
    }
}
