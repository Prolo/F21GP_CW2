using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    // related to player movement
    [Header("Movement Variables")]
    [SerializeField] Transform playerCam, groundCheck;
    [SerializeField][Range(0.0f, 0.5f)] private float camSmoothTime = 0.03f, moveSmoothtime = 0.3f;
    [SerializeField] private bool cursorLock = true, isGrounded;
    [SerializeField] private float mouseSensitivity = 3.5f, speed = 6.0f, gravity = -30f, jumpHeight = 6f, velocityY, cameraCap;
    [SerializeField] private int maxJumps = 1;
    [SerializeField] LayerMask ground;
    private int jumpCount;
    Vector2 currDir, currDirVelocity, currMouseDelta, currMouseDeltaVelocity;
    Vector3 velocity;
    CharacterController controller;

    // Related to system tasks, such as colour adjustment and scene transitions
    [Header("System Variables")]
    [SerializeField] private GameObject fadeOut, fadeIn, lightGrenadePrefab;
    [SerializeField] private float waitTime, throwForce;
    [SerializeField] private string targetScene;
    [SerializeField] private bool isInvulnerable = false;
    [SerializeField] private Transform throwPoint;

    // Signals sent to update the canvas
    [Header("Player related signals")]
    [SerializeField] private SignalSender hpSignal, pickupSignal, grenadeSignal, scoreSignal, killsSignal, bossSignal, totalSignal;

    // Players stats
    [Header("Player Stats")]
    [SerializeField] private FloatValue currHP, maxHP, grenades, kills, bossKill;
    [SerializeField] private Inventory playerInv;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        
        transform.position = new Vector3(0, 1, 0);
        transform.rotation = new Quaternion(0, 0, 0, 0);
        

        controller = GetComponent<CharacterController>();
        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }

        playerInv.ammo.runtimeValue = playerInv.ammo.startValue;
        playerInv.grenades.runtimeValue = playerInv.grenades.startValue;
        playerInv.score.runtimeValue = playerInv.score.startValue;
        kills.runtimeValue = kills.startValue;
        bossKill.runtimeValue = bossKill.startValue;


        // initial start up, such as player state and sending values to the canvas
        Application.targetFrameRate = 60;
        // currentState = PlayerState.walk;
        hpSignal.Raise();
        pickupSignal.Raise();
        scoreSignal.Raise();
        killsSignal.Raise();
        bossSignal.Raise();
        totalSignal.Raise();

    }

    // Update is called once per frame
    void Update()
    {
        MoveCharacter();
        MoveMouse();
        if (Input.GetKeyDown(KeyCode.G))
        {
            ThrowGrenade();
        }
        if (transform.position.y <= -10)
        {
            controller.enabled = false;
            transform.position = new Vector3(0, 1, 0);
            controller.enabled = true;
        }
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

    }

    void MoveMouse()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        Vector2 targetMouseDelta = new Vector2(mouseX, mouseY);
        currMouseDelta = Vector2.SmoothDamp(currMouseDelta, targetMouseDelta, ref currMouseDeltaVelocity, camSmoothTime);
        cameraCap -= currMouseDelta.y * mouseSensitivity;

        // locks the players view to 90 degrees either way on the y axis
        cameraCap = Mathf.Clamp(cameraCap, -15f, 15f);

        playerCam.localEulerAngles = Vector3.right * cameraCap;
        transform.Rotate(Vector3.up * currMouseDelta.x * mouseSensitivity);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        // checks that key is pressed
        if (!context.started) return;
        // checks if player in on the groundS
        if (!IsGrounded() && jumpCount >= maxJumps) return;
        if (jumpCount == 0) StartCoroutine(whenGrounded());

        jumpCount++;
        velocityY = jumpHeight;
    }

    private bool IsGrounded()
    {
        return controller.isGrounded;
    }

    private IEnumerator whenGrounded()
    {
        // checks the player is in the air
        yield return new WaitUntil(() => !IsGrounded());
        // checks the player is grounded after they jump or step off a ledge
        yield return new WaitUntil(IsGrounded);
        // resets jump count
        jumpCount = 0;
    }

    public void damaged(float time, float damage)
    {
        // if the player is invulnerable, don't apply damage
        if (isInvulnerable)
        {
            return;
        }

        // lowers the players current HP
        currHP.runtimeValue -= damage;
        hpSignal.Raise();

        // activates iframes
        if (currHP.runtimeValue > 0)
        {
            StartCoroutine(invulnTime(time));
        }
        // if the players HP is now 0, trigger gameover.
        else if (currHP.runtimeValue <= 0)
        {
            targetScene = "GAMEOVER";
            StartCoroutine(FadeCo());
        }

    }
    private IEnumerator FadeCo()
    {
        if (fadeIn != null)
        {
            Instantiate(fadeIn, Vector3.zero, Quaternion.identity);
        }
        yield return new WaitForSeconds(waitTime);
        currHP.runtimeValue = maxHP.runtimeValue * 2;
        hpSignal.Raise();
        AsyncOperation op = SceneManager.LoadSceneAsync(targetScene);
        while (op.isDone)
        {
            yield return null;
        }

    }

    private IEnumerator invulnTime(float time)
    {

        isInvulnerable = true;
        // col.a = 0.5f;
        yield return new WaitForSeconds(time);
        isInvulnerable = false;
        // col.a = 1f;
    }

    void ThrowGrenade()
    {
        if (grenades.runtimeValue > 0)
        {
            GameObject grenade = Instantiate(lightGrenadePrefab, throwPoint.position, throwPoint.rotation);
            Rigidbody rb = grenade.GetComponent<Rigidbody>();
            rb.AddForce(throwPoint.forward * throwForce, ForceMode.Impulse);
            grenades.runtimeValue -= 1;
            grenadeSignal.Raise();
            Debug.Log("Light Grenade Thrown!");
        }
        else
        {
            Debug.Log("No grenades left");
            return;
        }
    }
}
