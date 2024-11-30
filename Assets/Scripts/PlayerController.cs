using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement Related")]
    public Rigidbody rgb;
    public float moveSpeed = 5f;       // Adjusted for practical movement
    public bool canMove = true;

    public bool inUI = false;
    public bool isTyping = false;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.name == "PlayerObj")
        {
            rgb = GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Place additional logic here if needed
    }

    private void FixedUpdate()
    {
        if (canMove && !inUI && !isTyping)
        {
            // Get input for movement
            float horizontal = 0f;
            float vertical = 0f;
            float jump = 0f;

            if (Input.GetKey(KeyCode.RightArrow) | Input.GetKey(KeyCode.D))
            {
                horizontal = 1f;
            }
            if (Input.GetKey(KeyCode.LeftArrow) | Input.GetKey(KeyCode.A))
            {
                horizontal = -1f;
            }
            if (Input.GetKey(KeyCode.UpArrow) | Input.GetKey(KeyCode.W))
            {
                vertical = 1f;
            }
            if (Input.GetKey(KeyCode.DownArrow) | Input.GetKey(KeyCode.S))
            {
                vertical = -1f;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                jump = 1f;
            }

            // Calculate movement direction relative to player's orientation
            Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;

            // Normalize movement direction and apply speed
            moveDirection = moveDirection.normalized * moveSpeed;

            // Preserve existing vertical velocity and add jump force
            moveDirection.y = rgb.velocity.y + (jump > 0 ? 5f : 0f); // Adjust jump strength as needed

            // Apply movement to the rigidbody
            rgb.velocity = moveDirection;
        }
    }
}
