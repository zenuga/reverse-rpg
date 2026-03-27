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
        if (Input.GetMouseButton(0))
        {
            Attack();
        }
    }

    public virtual void Init()
    {
      
    }
    public virtual void Attack()
    {
    
    }
}