using UnityEngine;

public class PlaceholderCamera : MonoBehaviour
{
    public float peekAngle = 90f;
    public float smoothSpeed = 10f; 

    private Quaternion originalRotation;
    private Quaternion targetRotation;

    void Start()
    {
        // Store the rotation the camera had when the game started
        originalRotation = transform.localRotation;
        targetRotation = originalRotation;
    }

    void Update()
    {
        // 1. Check keys and set the target angle
        if (Input.GetKey(KeyCode.A))
        {
            // Lean Left
            targetRotation = originalRotation * Quaternion.Euler(0, -peekAngle, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            // Lean Right
            targetRotation = originalRotation * Quaternion.Euler(0, peekAngle, 0);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            // Look Back (180 degrees)
            targetRotation = originalRotation * Quaternion.Euler(0, 180f, 0);
        }
        else
        {
            // Return to center if no keys are held
            targetRotation = originalRotation;
        }

        // 2. Smoothly rotate toward the target
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * smoothSpeed);
    }
}