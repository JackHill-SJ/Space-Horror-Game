using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 50f;
    float horizontal;
    float vertical;
    Vector3 Direction;
    Rigidbody rb;

    public int maxStamina = 100; //max amount of stamina a player has
    [SerializeField]
    int currentStamina; //current calculataed amount of stamina player has
    int staminaRate = 1; //Rate at which the stamina will increase or decrease
    bool staminaDepleted = false; //Bool to know if stamina is at zero

    public GroundCheck groundCheck;

    private void Awake() => rb = gameObject.GetComponent<Rigidbody>();

    private void Start() => ResetStamina();

    void Update()
    {
        //Input
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Direction = Vector3.zero;
        Direction = transform.forward * vertical;
        Direction += transform.right * horizontal;

        //just incase the current stamina goes over the max stamina it will reset itself back down to the set max amount of stamina
        if (currentStamina >= maxStamina)
            ResetStamina();
    }

    private void FixedUpdate()
    {
        //Moving
        MoveCharacter();

        if (ShouldJump)
        {
            rb.AddForce(Vector3.up * jumpForce);
        }
    }

    public void MoveCharacter()
    { 
        
        //running will make it player go faster. Set to be 1.5x faster than the speed of walking
        if (/*Input.GetAxis("Run") > 0.1f*/ Input.GetKey(KeyCode.LeftShift) && !staminaDepleted)
        {
            Direction *= (speed * 1.5f) * Time.fixedDeltaTime;
            DepleteStamina(staminaRate);
        }
        else
        {
            //Walking speed so it doesn't add anything extra, increases stamina if not already maxed(done in coroutine)
            Direction *= speed * Time.fixedDeltaTime;

            StartCoroutine(IncreaseStamina(staminaRate));
        }

        rb.MovePosition(transform.position + (Direction));
    }

    //Jump button set in unity input system is being pressed
    private bool ShouldJump => Input.GetAxis("Jump") > 0.1f && groundCheck.isGrounded;

    private void DepleteStamina(int depletionAmount)
    {
        if (currentStamina <= 0)
        {
            staminaDepleted = true;
        }
        else
        {
            staminaDepleted = false;
            currentStamina -= depletionAmount;
        }
    }

    IEnumerator IncreaseStamina(int increaseAmount)
    {
        //Want it to wait a couple seconds if player hits 0 for their stamina. Then it goes into the else to start going back up
        if (currentStamina <= 0)
        {
            staminaDepleted = true;
            yield return new WaitForSeconds(2f);
            currentStamina = currentStamina + 1; //to push it out of the if statment to the else
        }
        else if (currentStamina <= maxStamina)
        {
            staminaDepleted = false;
            currentStamina += increaseAmount;
        }
    }

    private void ResetStamina()
    {
        currentStamina = maxStamina;
    }
}
