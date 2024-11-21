using UnityEngine;

public class CameraFollowAndControl : MonoBehaviour
{
    public Transform player;           // Player transform to follow
    public Vector3 offset;             // Offset from the player
    public float sensitivity = 100f;   // Mouse sensitivity
    public float followSpeed = 10f;    // Speed of camera follow

    public float pitch = 40f;          // Vertical rotation (up/down)
    public float yaw = 0f;            // Horizontal rotation (left/right)
    private bool anch = false;
    private bool anchCooldown = false;
    private bool firstView = false;
    private bool latePitchChange = false;
    private Vector3 lastVectView;

    void Start()
    {
        // Lock the cursor to the center of the screen initially
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        // Initialize the yaw to match the player's rotation
        yaw = player.eulerAngles.y;
    }

    void Update()
    {
        // Check if the right mouse button is held down
        if (anch)
        {
            // Get mouse input
            float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

            // Adjust yaw and pitch
            yaw += mouseX;
            pitch -= mouseY;

            // Clamp the pitch to prevent excessive vertical rotation
            pitch = Mathf.Clamp(pitch, -30f, 60f);

            // Rotate the player based on yaw (horizontal rotation)
            player.rotation = Quaternion.Euler(0f, yaw, 0f);
        }

        // Allow cursor unlock with Escape key
        if (Input.GetKeyDown(KeyCode.L) && anchCooldown == false && anch == true)
        {
            anch = false;
            anchCooldown = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (Input.GetKeyDown(KeyCode.L) && anchCooldown == false && anch == false)
        {
            anch = true;
            anchCooldown = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }
        if (Input.GetKeyDown(KeyCode.V) && anchCooldown == false && firstView == false)
        {
            anchCooldown = true;
            firstView = true;
            lastVectView = offset;
            offset = new Vector3(0, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.V) && anchCooldown == false && firstView == true)
        {
            anchCooldown = true;
            firstView = false;
            latePitchChange = true;
            offset = lastVectView;
        }

        if (anchCooldown)
        {
            anchCooldown = false;
        }

    }

    void LateUpdate()
    {
        // Calculate the camera's target position based on the player's position and the offset
        Vector3 targetPosition = player.position + Quaternion.Euler(0f, yaw, 0f) * offset;

        // Smoothly move the camera to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Rotate the camera based on the calculated pitch and yaw
        if (latePitchChange)
        {
            latePitchChange = false;
            pitch = 40f;
        }
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}
