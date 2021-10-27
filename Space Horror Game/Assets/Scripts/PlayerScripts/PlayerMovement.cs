using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 50f;
    float horizontal;
    float vertical;
    Vector3 Direction;
    Rigidbody rb;

    [SerializeField] InputActionAsset PlayerControls;
    InputAction movementAction, jumpAction, runAction;
    Vector3 moveDirection;
    bool isRunning;

    public int maxStamina = 100; //max amount of stamina a player has
    [SerializeField]
    int currentStamina; //current calculataed amount of stamina player has
    int staminaRate = 1; //Rate at which the stamina will increase or decrease
    bool staminaDepleted = false; //Bool to know if stamina is at zero
    bool staminaFull = true; //Bool for when the stamina is at max

    public GroundCheck groundCheck;

    private void Awake() 
    {
        var PlayerActionMap = PlayerControls.FindActionMap("Player");
        movementAction = PlayerActionMap.FindAction("Move");
        jumpAction = PlayerActionMap.FindAction("Jump");
        runAction = PlayerActionMap.FindAction("Run");

        movementAction.performed += InputMovement;
        movementAction.canceled += InputMovement;
        jumpAction.performed += Jumping;
        jumpAction.canceled += Jumping;
        runAction.performed += Running;
        runAction.canceled += Running;


        rb = gameObject.GetComponent<Rigidbody>();
    }


    private void OnEnable()
    {
        movementAction.Enable();
        jumpAction.Enable();
        runAction.Enable();
    }
    private void OnDisable()
    {
        movementAction.Disable();
        jumpAction.Disable();
        runAction.Disable();
    }

    private void Start() => ResetStamina();

    void Update()
    {
        //Input
        //horizontal = Input.GetAxis("Horizontal");
        //vertical = Input.GetAxis("Vertical");

        Direction = Vector3.zero;
        Direction = transform.forward * moveDirection.z;
        Direction += transform.right * moveDirection.x;

        //just incase the current stamina goes over the max stamina it will reset itself back down to the set max amount of stamina
        if (currentStamina >= maxStamina)
            ResetStamina();
    }

    private void FixedUpdate()
    {
        //Moving
        MoveCharacter();
    }

    public void MoveCharacter()
    { 
        //running will make it player go faster. Set to be 1.5x faster than the speed of walking
        if (/*Input.GetAxis("Run") > 0.1f Input.GetKey(KeyCode.LeftShift)*/ isRunning && !staminaDepleted)
        {
            Direction *= (speed * 1.5f) * Time.fixedDeltaTime;
            DepleteStamina(staminaRate);
        }
        else
        {
            //Walking speed so it doesn't add anything extra, increases stamina if not already maxed(done in coroutine)
            Direction *= speed * Time.fixedDeltaTime;
            if (!staminaFull)
            {
                StartCoroutine(IncreaseStamina(staminaRate));
            }
        }

        rb.MovePosition(transform.position + (Direction));
    }

    //Jump button set in unity input system is being pressed
    //private bool ShouldJump => jumpAction.triggered && groundCheck.isGrounded;

    private void Running(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    public void Jumping(InputAction.CallbackContext context)
    {
        if (groundCheck.isGrounded)
            rb.AddForce(Vector3.up * jumpForce);
    }

    public void InputMovement(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();

        moveDirection = new Vector3(direction.x, 0, direction.y);
    }

    private void DepleteStamina(int depletionAmount)
    {
        if (currentStamina <= 0)
        {
            staminaDepleted = true;
        }
        else
        {
            staminaFull = false;
            staminaDepleted = false;
            currentStamina -= depletionAmount;
        }
    }

    IEnumerator IncreaseStamina(int increaseAmount)
    {
        //Want it to wait a couple seconds if player hits 0 for their stamina. Then it goes into the else to start going back up
        if (currentStamina <= 0 && !staminaFull)
        {
            staminaDepleted = true;
            yield return new WaitForSeconds(5f);
            currentStamina = currentStamina + 1; //to push it out of the if statment to the else if
        }
        else if (currentStamina <= maxStamina && !staminaFull)
        {
            staminaDepleted = false;
            yield return new WaitForSeconds(3f);
            currentStamina += increaseAmount;
        }
        else if (currentStamina >= maxStamina)
        {
            staminaFull = true;
        }
    }

    private void ResetStamina()
    {
        currentStamina = maxStamina;
    }
}
