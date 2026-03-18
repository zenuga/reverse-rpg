using UnityEngine;

public class Arrow : projectile
{
    public override float lifeTime { get; set; } = 90f;
    public override float damage { get; } = 10f;
    public Rigidbody rb;

    public override void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];

        transform.position = contact.point;

        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;

        GetComponent<Collider>().enabled = false;

        transform.SetParent(collision.transform);

        Debug.Log("Arrow hit " + collision.gameObject.name);
    }
}
