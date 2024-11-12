using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float defaultVelocity = 10f;
    public float minVelocity = 5f;
    public float maxVelocity = 20f;
    public float dashSpeed = 30f;
    public float dashCooldown = 0.5f;
    public float jumpForce = 10f;
    public float hoverTime = 0.5f;
    public float jumpDuration = 0.3f;
    public float changeVelPerFrame = 15f;

    public TextMeshProUGUI velocityText;
    public Slider boostSlider;

    public float currentVelocity;
    private bool canDash = true;
    private int jumpCount = 0;
    private Rigidbody rb;
    private int boostCharges;
    private float boostTimeRemaining = 0f; // Remaining time for boost decay
    private int maxBoostCharges = 10; // Max boost charges
    private float boostDecayRate = 1f; // Time for 1 boost charge to decay (1 second)

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Player missing Rigidbody");
        }

        currentVelocity = defaultVelocity;
        //rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        if (boostSlider != null)
        {
            boostSlider.maxValue = maxBoostCharges;
            boostSlider.value = 0f;
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        UpdateVelocityDisplay();

        
    }

    private void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(moveHorizontal, 0, 1) * currentVelocity;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        AdjustVelocity();

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

    private void AdjustVelocity()
    {
        if (Input.GetKey(KeyCode.W) && boostCharges > 0)
        {
            currentVelocity = Mathf.Min(maxVelocity, currentVelocity + changeVelPerFrame);

            boostTimeRemaining -= Time.deltaTime;
            

            
            if (boostTimeRemaining <= 0)
            {
                boostCharges--;
                boostTimeRemaining = boostDecayRate;

                if (boostSlider != null)
                {
                    boostSlider.value = boostCharges;
                }
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            currentVelocity = Mathf.Max(minVelocity, currentVelocity - changeVelPerFrame);
        }
        else
        {
            currentVelocity = Mathf.Lerp(currentVelocity, defaultVelocity, Time.deltaTime * 5);
        }
    }

    private IEnumerator Dash(Vector3 direction)
    {
        canDash = false;
        rb.velocity = direction * dashSpeed;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 1)
        {
            StartCoroutine(SmoothJump());
            jumpCount++;
        }
    }

    private IEnumerator SmoothJump()
    {
        float jumpTime = 0f;
        while (jumpTime < jumpDuration)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce * (1 - (jumpTime / jumpDuration)), rb.velocity.z);
            jumpTime += Time.deltaTime;
            yield return null;
        }

        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }

    private void UpdateVelocityDisplay()
    {
        if (velocityText != null)
        {
            velocityText.text = "Speed: " + Mathf.RoundToInt(currentVelocity);
        }
    }


    public void CollectSphere()
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
        }
    }


    public void AddBoostCharge(int charge)
    {
        boostCharges += charge;
        boostSlider.value = boostCharges;
    }
}
