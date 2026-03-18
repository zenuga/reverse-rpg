using UnityEngine;

public class WeaponHandeler : MonoBehaviour
{
    public BaseWeaponScript weapon1;
    public BaseWeaponScript weapon2;
    private BaseWeaponScript currentWeapon;

    void Start()
    {
        currentWeapon = weapon1 ;
    }

    public BaseWeaponScript GetCurrentWeapon()
    {
        return currentWeapon;
    }
}
