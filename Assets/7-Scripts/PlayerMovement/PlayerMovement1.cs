using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement1 : MonoBehaviour
{
    public float defaultVelocity = 10f;
    public float minVelocity = 5f;
    public float maxVelocity = 30f; // Max speed set to 30
    public float dashSpeed = 20f;
    public float dashCooldown = 0.5f;
    public float jumpForce = 10f;
    public float jumpDuration = 0.3f; // Duration of smooth jump
    public AnimationCurve accelerationCurve;
    public AnimationCurve decelerationCurve;
    public float accelerationTime;
    public float decelerationTime;
    public float decelerationMultiplier=1f;
    public Slider boostSlider;
    
    [HideInInspector]public Animator animator;
    
    public Coroutine AccelRoutine;
    public Coroutine DecelRoutine;

    public float previousAcceleration = 0f;
    public float previousDeceleration = 0f;
    
    public float currentVelocity;
    private bool isGrounded = true;
    private bool canDash = true; // Can dash or not
    private int jumpCount = 0; // Count of jumps (limit to 1 now)
    private Rigidbody rb;
    private bool isRampingUp = false; // Check if initial ramp-up is in progress
    private bool hasReached15 = false; // Check if reached 15 to trigger fast ramp
    private bool isAccelerating = false;
    private bool isDecelerating = false;
    private float currentMultiplier;
    private int boostCharges;
    private float boostTimeRemaining = 0f; // Remaining time for boost decay
    private int maxBoostCharges = 10; // Max boost charges
    private float boostDecayRate = 1f;
    private bool clampY;
    
    
    private void Start()
    {
        currentMultiplier = 1f;
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Player missing Rigidbody");
        }
        animator = GetComponentInChildren<Animator>();
        currentVelocity = defaultVelocity;

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        
        boostSlider.maxValue = maxBoostCharges;
        

    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        //if(Input.GetKeyDown(KeyCode.Escape))
        //    UiManager.instance.SwitchtoMode(2); Now this in UIManager.cs
        if (clampY)
            transform.position = new Vector3(transform.position.x, 1f, transform.position.z);

    }


    private void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        float horizontalSpeedModifier = Mathf.Clamp(1 - (currentVelocity / maxVelocity), 0.3f, 1f);
        moveHorizontal *= horizontalSpeedModifier;

        Vector3 movement = new Vector3(moveHorizontal, 0, 1) * currentVelocity;

        animator?.SetInteger("Direction", 0);
        if (Input.GetKey(KeyCode.A))
            animator?.SetInteger("Direction", 1);
        else if(Input.GetKey(KeyCode.D))
            animator?.SetInteger("Direction", 2);
        
        
            
        
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        if (Input.GetKey(KeyCode.W)  && boostCharges > 0)
        {

            if (!isAccelerating)
            {
                if (isDecelerating)
                {
                    StopCoroutine(DecelRoutine);
                    isDecelerating = false;
                }
                AccelRoutine = StartCoroutine(MovementCurve());
            }
            
            boostTimeRemaining -= Time.deltaTime;
            boostSlider.value = boostCharges - (1 - (boostTimeRemaining / boostDecayRate));
            if (boostTimeRemaining <= 0)
            {
                boostCharges--;
                boostTimeRemaining = boostDecayRate;

                if (boostSlider != null)
                {
                    boostSlider.value = boostCharges;
                }
            }
            
            isAccelerating = true;
        }
        else if ((!Input.GetKey(KeyCode.W) && isAccelerating)|| (boostCharges <= 0 && isAccelerating))
        {
            if (AccelRoutine != null)
            {
                StopCoroutine(AccelRoutine);
            }
            DecelRoutine = StartCoroutine(DecelerationCurve());
            isAccelerating = false;
        }
        
        
        

        if (Input.GetKey(KeyCode.S))
            currentMultiplier = decelerationMultiplier;
        else
            currentMultiplier = 1f;
        

        //Debug.Log("Current Velocity: " + currentVelocity);

        // Dashing
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && canDash)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                StartCoroutine(Dash(Vector3.left));
                
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                StartCoroutine(Dash(Vector3.right));
                
            }
        }
    }
    

    private IEnumerator Dash(Vector3 direction)
    {
        canDash = false;
        rb.velocity = direction * dashSpeed;
        int directionID = direction == Vector3.left ? 1 : 2;
        print(directionID);
        animator?.SetInteger("Direction", directionID);
        yield return new WaitForSeconds(dashCooldown);
        animator?.SetInteger("Direction", 0);
        canDash = true;
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 1)
        {
            clampY = false;
            animator?.SetInteger("Direction", 0);
            StartCoroutine(SmoothJump());
            jumpCount++;
            isGrounded = false;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            StopCoroutine(SmoothJump());
            rb.velocity = new Vector3(rb.velocity.x, Mathf.Min(rb.velocity.y, 0), rb.velocity.z); // Ограничиваем вертикальную скорость
        }
    }

    private IEnumerator SmoothJump()
    {
        float jumpTime = 0f;

        // Increase upward force for smooth jump
        while (jumpTime < jumpDuration && Input.GetKey(KeyCode.Space))
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce * (1 - (jumpTime / jumpDuration)), rb.velocity.z);
            jumpTime += Time.deltaTime;
            animator?.SetBool("IsJumping", true);
            yield return null;
        }
        
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0;
            animator?.SetBool("IsJumping", false);
            clampY = true;
        }
    }

    private IEnumerator MovementCurve()
    {
        float time = 0f;
        float t;
        isRampingUp = true;
        if(previousDeceleration != 0f)
            time = accelerationTime-accelerationCurve.Evaluate(previousDeceleration)*accelerationTime;
       
        
        while (time<=accelerationTime)
        {

            time += Time.deltaTime;
            t = time / accelerationTime;
            previousAcceleration = t;
            currentVelocity = Mathf.Lerp(defaultVelocity, maxVelocity, accelerationCurve.Evaluate(t));
            yield return null;
        }
        
        previousAcceleration = 0f;
        
        isRampingUp = false;
    }


    private IEnumerator DecelerationCurve()
    {
        float time = 0f;
        float t;
        isDecelerating = true;
        if(previousAcceleration != 0f)
            time = decelerationTime-decelerationCurve.Evaluate(previousAcceleration)*decelerationTime;
        
        
        
        while (time<=decelerationTime)
        {
            time += Time.deltaTime*currentMultiplier;
            t = time / decelerationTime;
            previousDeceleration = t;
            currentVelocity = Mathf.Lerp(maxVelocity, defaultVelocity, decelerationCurve.Evaluate(t));
            yield return null;
        }
        
        isRampingUp = false;
        t = 0;
        previousDeceleration = 0f;
        isDecelerating = false;
    }
    
    public void AddBoostCharge(int charge)
    {
        if (boostCharges < maxBoostCharges)
        {
            boostCharges++;
            boostTimeRemaining = boostDecayRate;

            if (boostSlider != null)
            {
                boostSlider.value = boostCharges;
            }
        }
    }
    
    
    
}
