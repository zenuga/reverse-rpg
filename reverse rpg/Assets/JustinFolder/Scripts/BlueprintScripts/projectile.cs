using UnityEngine;
using UnityEngine.Rendering;

public class projectile : MonoBehaviour
{
    public float lifeTime;
    public LayerMask enemyLayer;
    public float damage;
    public AudioClip impactSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
 

    public virtual void OnCollisionEnter(Collision collision)
    {
        bool hitFloor = collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Ground");

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            HealthSystem healthSystem = collision.gameObject.GetComponent<HealthSystem>();

            if (healthSystem != null)
            {
                healthSystem.TakeDamage(damage);
                Debug.Log("Hit " + collision.gameObject.name + " for " + damage + " damage");
            }
        }
        else if (hitFloor)
        {
            if (impactSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(impactSound);
            }
            Debug.Log("Magic projectile hit the floor");
        }
    }
}
