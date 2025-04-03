using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollowAndControl : MonoBehaviour
{
    public Transform player;           // Player transform to follow
    public Vector3 offset;             // Offset from the player
    public float sensitivity = 100f;   // Mouse sensitivity
    public float followSpeed = 10f;    // Speed of camera follow

    public float pitch = 0f;           // Vertical rotation (up/down)
    public float yaw = 0f;             // Horizontal rotation (left/right)

    private bool isMouseControlActive = true; // Toggle for mouse control
    private float shakeDuration = 0f;  // Duration of the screen shake
    private float shakeMagnitude = 0.1f; // Magnitude of the screen shake
    private float dampingSpeed = 1.0f;   // Speed at which the shake diminishes
    private Vector3 shakeOffset;       // Offset for the screen shake

    public bool locked = false;
    private bool oncooldown = false;
    private float Orfs = 200f;

    private bool lateApplying = false;

    public static CameraFollowAndControl instance;

    private void Start()
    {
        // Initialize the yaw to match the player's horizontal rotation
        yaw = player.eulerAngles.y;

        // Lock and hide the cursor initially
    }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        HandleMouseInput();
        ApplyScreenShake();
    }

    private void LateUpdate()
    {
        SmoothFollow();
    }

    private void HandleMouseInput()
    {
        if (locked)
        {
            // Get mouse input for rotation
            float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

            // Adjust yaw and pitch based on mouse movement
            yaw += mouseX;
            pitch -= mouseY;

            // Clamp pitch to prevent the camera from flipping
            pitch = Mathf.Clamp(pitch, -30f, 60f);

            // Rotate the player to match the horizontal yaw
            player.rotation = Quaternion.Euler(0f, yaw, 0f);
        }

        // Toggle mouse control with Escape key

        if (Input.GetKeyDown(KeyCode.L) && locked == false && oncooldown == false)
        {
            locked = true;
            Cursor.lockState = CursorLockMode.Locked;
            oncooldown = true;
        }
        if (Input.GetKeyDown(KeyCode.L) && locked == true && oncooldown == false)
        {
            locked = false;
            Cursor.lockState = CursorLockMode.None;
            oncooldown = true;
        }
        if (oncooldown)
        {
            oncooldown = false;
        }
    }

    private void SmoothFollow()
    {
        // Calculate the target position of the camera based on the player's position and offset
        Vector3 targetPosition = player.position + Quaternion.Euler(0f, yaw, 0f) * offset;

        // Smoothly move the camera to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Set the camera's rotation based on pitch and yaw
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    private void ApplyScreenShake()
    {
        if (shakeDuration > 0)
        {
            // Add random noise to the camera's position
            shakeOffset = Vector3.Lerp(transform.position, Random.insideUnitSphere * shakeMagnitude, dampingSpeed);
            
            // Reduce the shake duration over time
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeOffset = Vector3.zero; // Reset shake offset when shake ends

            if (lateApplying)
            {
                followSpeed = Orfs;
                lateApplying = false;
            }
        }

        // Apply the shake offset on top of the current position
        transform.position += shakeOffset;
    }

    /// <summary>
    /// Triggers the screen shake effect.
    /// </summary>
    /// <param name="duration">How long the shake lasts.</param>
    /// <param name="magnitude">The intensity of the shake.</param>
    public void TriggerScreenShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        Orfs = followSpeed;
        followSpeed = 50f;
        lateApplying = true;
    }
}
