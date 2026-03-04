using UnityEngine;
using UnityEngine.InputSystem;

public class RaycastParticleScript : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject particlePrefab;
    public float maxDistance = 100f;

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Klik gedetecteerd");

            Ray ray = playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                Debug.Log("Raakt: " + hit.collider.name);

                Instantiate(particlePrefab, hit.point, Quaternion.identity);
            }
            else
            {
                Debug.Log("Raycast raakt niets");
            }
        }
    }
}