using UnityEngine;

public class WeaponHandeler : MonoBehaviour
{
    public BaseWeaponScript meleeWeapon;
    public BaseWeaponScript rangedWeapon;
    private BaseWeaponScript currentWeapon;

    void Start()
    {
        currentWeapon = meleeWeapon;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeapon = meleeWeapon;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeapon = rangedWeapon;
        }
    }

    public BaseWeaponScript GetCurrentWeapon()
    {
        return currentWeapon;
    }
}
