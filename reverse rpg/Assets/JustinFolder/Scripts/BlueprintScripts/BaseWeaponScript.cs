using UnityEngine;

public class BaseWeaponScript : MonoBehaviour
{

    void Start()
    {
        Init();
    }

    void Update()
    {
        UpdateStats();  
    }



//-----------------------------------------------------------------------------------------------------

    public virtual void UpdateStats()
    {
        Debug.Log("Updating the statsss!!!!");
    }

    public virtual void Init()
    {
        Debug.Log("Initializing the weapon!!!!");
    }
    public virtual void Attack()
    {
        Debug.Log("Attacking with the weapon!!!!");
    }
}