using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] Transform playerCam, groundCheck;
    [SerializeField][Range(0.0f, 0.5f)] private float camSmoothTime = 0.03f, moveSmoothtime = 0.3f;
    [SerializeField] private bool cursorLock = true, isGrounded;
    [SerializeField] private float mouseSensitivity = 3.5f, speed = 6.0f, gravity = -30f, jumpHeight = 6f, velocityY, cameraCap;
    [SerializeField] LayerMask ground;
    Vector2 currDir, currDirVelocity, currMouseDelta, currMouseDeltaVelocity;
    Vector3 velocity;
    CharacterController controller;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveCharacter();
        MoveMouse();
    }

    void MoveCharacter()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, ground);

        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currDir = Vector2.SmoothDamp(currDir, targetDir, ref currDirVelocity, moveSmoothtime);
        velocityY += gravity * 2f * Time.deltaTime;
        velocity = (transform.forward * currDir.y + transform.right * currDir.x) * speed + Vector3.up * velocityY;
        controller.Move(velocity * Time.deltaTime);

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (isGrounded! && controller.velocity.y < -1f)
        {
            velocityY = -8f;
        }
    }

    void MoveMouse()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        currMouseDelta = Vector2.SmoothDamp(currMouseDelta, targetMouseDelta, ref currMouseDeltaVelocity, camSmoothTime);
        cameraCap -= currMouseDelta.y * mouseSensitivity;

        // locks the players view to 90 degrees either way on the y axis
        cameraCap = Mathf.Clamp(cameraCap, -90.0f, 90f);

        playerCam.localEulerAngles = Vector3.right * cameraCap;
        transform.Rotate(Vector3.up * currMouseDelta.x * mouseSensitivity);
    }
}
