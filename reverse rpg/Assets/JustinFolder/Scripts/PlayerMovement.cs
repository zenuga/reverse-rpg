using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform playerCamera;

    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float crouchSpeed = 2.5f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;
    private bool isSprinting;
    private bool isGrounded;
    private bool isCrouching;
    float currentSpeed;

    [Header("Crouch")]
    public float crouchHeight = 1f;
    public float normalHeight = 2f;
    public float crouchCameraHeight = 0.5f;
    public float normalCameraHeight = 0.9f;
    private bool CanStand()
{
    float heightDifference = normalHeight - crouchHeight;

    Ray ray = new Ray(transform.position, Vector3.up);

    return !Physics.Raycast(ray, heightDifference);
}

    [Header("Mouse Look")]
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;
    private Camera cam;
    private float normalFOV = 60f;
    private float sprintFOV = 70f;
    private float crouchFOV = 50f;


    private Vector3 velocity;
    private Vector3 lastPosition;


    void Start()
    {
        // camera starting position

        playerCamera.localPosition = new Vector3(0, normalCameraHeight, 0);

        // get camera component and set FOV

        cam = playerCamera.GetComponent<Camera>();
        cam.fieldOfView = normalFOV;
        
        // save last position

        lastPosition = transform.position;

        // set initial speed

        currentSpeed = walkSpeed;

    }


    void Update()
    {
        // gound check

        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // movement

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * currentSpeed * Time.deltaTime);

        // not sprinting

        if (!Input.GetKey(KeyCode.LeftShift) && isSprinting)
        {
            isSprinting = false;
            currentSpeed = walkSpeed;
        }

        // sprinting and fov

        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            currentSpeed = sprintSpeed;
            float distanceMoved = Vector3.Distance(transform.position, lastPosition);

            if (distanceMoved > 0.01f)
            {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, sprintFOV, Time.deltaTime * 5f);
            }   
            else
            {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, normalFOV, Time.deltaTime * 5f);
            }

            lastPosition = transform.position;
            isSprinting = true;
        }

        //crouch speed and fov

        if (isCrouching)
        {
            currentSpeed = crouchSpeed;
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, crouchFOV, Time.deltaTime * 5f);
        }

        if (!isCrouching && !isSprinting)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, normalFOV, Time.deltaTime * 5f);
        }

        

        // Jump

        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Crouch

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = true;
            controller.height = crouchHeight;
            playerCamera.localPosition = new Vector3(0, crouchCameraHeight, 0);
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) && CanStand())
        {
            isCrouching = false;
            controller.height = normalHeight;
            playerCamera.localPosition = new Vector3(0, normalCameraHeight, 0);
        }

        if (CanStand() && !Input.GetKey(KeyCode.LeftControl) && isCrouching)
        {
            isCrouching = false;
            controller.height = normalHeight;
            playerCamera.localPosition = new Vector3(0, normalCameraHeight, 0);
        }

        // Gravity

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Mouse Look

        if (Input.GetKey(KeyCode.Mouse0) && CursorLockMode.Locked != Cursor.lockState)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        //mouse unlock

        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (CursorLockMode.Locked == Cursor.lockState)
        {
            
            // Mouse input

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // Rotate player left/right

            transform.Rotate(Vector3.up * mouseX);

            // Rotate camera up/down

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

    }
    
}
