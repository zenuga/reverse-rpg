using UnityEngine;

public class explosionLifetime : MonoBehaviour
{
    public float lifeTime = 1;

    void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
