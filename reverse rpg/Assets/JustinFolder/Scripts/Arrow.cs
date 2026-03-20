using UnityEngine;

public class Arrow : projectile
{
    public Rigidbody rb;
    
    void Update()
    {
        damage = 50f;
        lifeTime = 90f;
    }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        
        ContactPoint contact = collision.contacts[0];

        transform.position = contact.point;

        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;

        GetComponent<Collider>().enabled = false;

        transform.SetParent(collision.transform);

        Debug.Log("Arrow hit " + collision.gameObject.name);
    }
}
