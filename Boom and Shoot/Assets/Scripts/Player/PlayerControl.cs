using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour
{
    // related to player movement
    [Header("Movement Variables")]
    [SerializeField] private float speed, smoothTime = 0.05f, gravityMultiplier = 3.0f;
    [SerializeField] private Movement movement;
    // [SerializeField] private PlayerState currentState;
    private Vector2 _input;
    private CharacterController _characterController;
    private Vector3 _direction;
    private float _currentVelocity, _velocity, _gravity = -9.81f;

    // Related to player jumping
    [Header("Jumping Variables")]
    [SerializeField] private int maxJumps = 1;
    [SerializeField] private float jumpHeight;
    private int _jumpCount;

    // Players stats
    [Header("Player Stats")]
    [SerializeField] private FloatValue currHP, maxHP;
    [SerializeField] private Inventory playerInv;

    // Signals sent to update the canvas
    [Header("Player related signals")]
    [SerializeField] private SignalSender hpSignal, pickupSignal;

    // Used when the player takes damage
    [Header("Player Iframes")]
    // [SerializeField] private Color col;
    [SerializeField] private float duration;
    [SerializeField] private int flashes;
    [SerializeField] private bool isInvulnerable = false;

    // Related to system tasks, such as colour adjustment and scene transitions
    [Header("System Variables")]
    [SerializeField] private GameObject fadeOut, fadeIn;
    [SerializeField] private float waitTime;
    [SerializeField] private string targetScene;


    //[SerializeField] private Animator _animator;


    private void Start()
    {
        // initial start up, such as player state and sending values to the canvas
        Application.targetFrameRate = 60;
        // currentState = PlayerState.walk;
        hpSignal.Raise();
        pickupSignal.Raise();

        //_animator = GetComponent<Animator>();
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Gravity();
        Rotation();
        MoveCharacter();



        //if (_input == Vector2.zero)
        //{
        //    _animator.SetBool("isWalking", false);
        //}
    }

    private void MoveCharacter()
    {
        // determines the correct speed when dashing
        var targetSpeed = movement.isDashing ? movement.speed * movement.multiplier : movement.speed;
        movement.currentSpeed = Mathf.MoveTowards(movement.currentSpeed, targetSpeed, movement.acceleration * Time.deltaTime);

        // detects player input and moves appropriately. IMPORTANT: DeltaTime ensures movement is consistent regardless of framerate
        _characterController.Move(_direction * movement.currentSpeed * Time.deltaTime);

    }

    private void Rotation()
    {
        // Stops the character from resetting rotation when no input is detected
        if (_input.sqrMagnitude == 0)
            return;

        // handles the rotation of the characters model
        var target = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target, ref _currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }

    private void Gravity()
    {
        // ensures the character adheres to gravity, instead of floating.      
        if (isGrounded() && _velocity < 0.0f)
        {
            // resets velocity when the character is on the ground, so that they do not fall faster every time they step off a ledge or jump
            _velocity = -1.0f;
        }
        else
        {
            // IMPORTANT: DeltaTime ensures the character falls consistently, regardless of framerate
            _velocity += _gravity * gravityMultiplier * Time.deltaTime;
        }

        _direction.y = _velocity;
    }


    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
        // converts the y axis from the input manager into z
        _direction = new Vector3(_input.x, 0.0f, _input.y);

        //_animator.SetBool("isWalking", true);
    }


    public void Jump(InputAction.CallbackContext context)
    {
        // checks that key is pressed
        if (!context.started) return;
        // checks if player in on the ground
        if (!isGrounded() && _jumpCount >= maxJumps) return;
        if (_jumpCount == 0) StartCoroutine(whenGrounded());

        _jumpCount++;
        _velocity = jumpHeight;

    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!isGrounded()) return;
    }

    public void Dash(InputAction.CallbackContext context)
    {
        movement.isDashing = context.started || context.performed;
    }

    private IEnumerator whenGrounded()
    {
        // checks the player is in the air
        yield return new WaitUntil(() => !isGrounded());
        // checks the player is grounded after they jump or step off a ledge
        yield return new WaitUntil(isGrounded);
        // resets jump count
        _jumpCount = 0;
    }

    // applies damage to the player
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
            targetScene = "Game Over";
            StartCoroutine(FadeCo());
        }

    }

    // runs when the player takes damage to prevent multiple instances of damage occurring at once
    private IEnumerator invulnTime(float time)
    {

        isInvulnerable = true;
        // col.a = 0.5f;
        yield return new WaitForSeconds(time);
        isInvulnerable = false;
        // col.a = 1f;
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

    private bool isGrounded()
    {
        return _characterController.isGrounded;
    }

    [Serializable]
    public struct Movement
    {
        public float speed, multiplier, acceleration;
        [HideInInspector] public bool isDashing;
        [HideInInspector] public float currentSpeed;
    }

    // Called when changing scenes, using a fade out/fade in effect to make the transition less harsh.

}

