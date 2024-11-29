using UnityEngine;

public class CameraFollowAndControl : MonoBehaviour
{
    public Transform player;           // Player transform to follow
    public Vector3 offset;             // Offset from the player
    public float sensitivity = 100f;   // Mouse sensitivity
    public float followSpeed = 10f;    // Speed of camera follow
    public float rotationSmoothTime = 0.1f; // Smoothness for rotation adjustments

    public float pitch = 40f;          // Vertical rotation (up/down)
    public float yaw = 0f;             // Horizontal rotation (left/right)

    private bool anch = false;         // Anchor mode toggle
    private Vector3 currentOffset;     // Smoothly transitioning offset
    private Vector3 rotationVelocity;  // Velocity for smooth rotation

    private float shakeDuration = 0f;  // Duration of the screen shake
    private float shakeMagnitude = 0.1f; // Magnitude of the screen shake
    private float dampingSpeed = 1.0f;   // Speed at which the shake diminishes
    private Vector3 shakeOffset;       // Offset for the screen shake

    private void Start()
    {
        currentOffset = offset;
        yaw = player.eulerAngles.y;
    }

    private void Update()
    {
        HandleMouseInput();
    }

    private void LateUpdate()
    {
        SmoothFollow();
        ApplyScreenShake();
    }

    private void HandleMouseInput()
    {
        if (anch)
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

            yaw += mouseX;
            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, -30f, 60f);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            anch = !anch;
            Cursor.lockState = anch ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !anch;
        }
    }

    private void SmoothFollow()
    {
        // Calculate target position based on player position and offset
        Vector3 targetPosition = player.position + Quaternion.Euler(0f, yaw, 0f) * currentOffset;

        // Smoothly move the camera to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Smoothly adjust rotation
        float smoothYaw = Mathf.SmoothDampAngle(transform.eulerAngles.y, yaw, ref rotationVelocity.y, rotationSmoothTime);
        float smoothPitch = Mathf.SmoothDampAngle(transform.eulerAngles.x, pitch, ref rotationVelocity.x, rotationSmoothTime);
        transform.rotation = Quaternion.Euler(smoothPitch, smoothYaw, 0f);
    }

    private void ApplyScreenShake()
    {
        if (shakeDuration > 0)
        {
            // Add random noise to the camera's position
            shakeOffset = Random.insideUnitSphere * shakeMagnitude;

            // Reduce the shake duration over time
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeOffset = Vector3.zero; // Reset shake offset when shake ends
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
    }
}
