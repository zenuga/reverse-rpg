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

        if (Input.GetMouseButton(0))
        {
            Attack();
        }
        
    }



//-----------------------------------------------------------------------------------------------------

    public virtual void UpdateStats()
    {

    }

    public virtual void Init()
    {
      
    }
    public virtual void Attack()
    {
    
    }
}