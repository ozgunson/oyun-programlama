using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float gravityMultiplier;
    [SerializeField]
    private float jumpHorizontalSpeed;
    [SerializeField]
    private float jumpButtonGracePeriod;


    [SerializeField]
    private Transform cameraTransform;


    private Animator animator;
    private string[] attackAnimations = { "PunchRight", "PunchLeft", "Kick", "Kick2" };

    private CharacterController characterController;
    private float ySpeed;
    private float originalStepOffset;
    private float? lastGroundTime;
    private float? jumpButtonPressedTime;
    private bool isJumping;
    private bool isGrounded;


    private float health = 100;
    private bool isDead;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude) / 2;

        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            inputMagnitude *= 2;
        }
        animator.SetFloat("Input Magnitude", inputMagnitude, 0.05f, Time.deltaTime);
        
        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.Normalize();

        float gravity = Physics.gravity.y * gravityMultiplier;
        if(isJumping && ySpeed > 0 && Input.GetButton("Jump") == false)
        {
            gravity *= 2;
        }
        ySpeed += gravity * Time.deltaTime;

        if (characterController.isGrounded)
        {
            lastGroundTime = Time.time;
        }
        if(Input.GetButtonDown("Jump"))
        {
            jumpButtonPressedTime = Time.time;
        }

        if (Time.time - lastGroundTime <= jumpButtonGracePeriod)
        {
            characterController.stepOffset = originalStepOffset;
            ySpeed = -0.5f;
            animator.SetBool("IsGrounded", true);
            isGrounded = true;
            animator.SetBool("IsJumping", false);
            isJumping = false;
            animator.SetBool("IsFalling", false);

            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
            {
                ySpeed = Mathf.Sqrt(jumpHeight * -3 * gravity);
                animator.SetBool("IsJumping", true);
                isJumping = true;
                jumpButtonPressedTime = null;
                lastGroundTime = null;
            }
        }
        else
        {
            characterController.stepOffset = 0;
            animator.SetBool("IsGrounded", false);
            isGrounded = false;

            if ((isJumping && ySpeed < 0) || ySpeed < -2)
            {
                animator.SetBool("IsFalling", true);
            }
        }
        
        
        if(movementDirection != Vector3.zero)
        {
            animator.SetBool("IsMoving", true);
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }

        if(isGrounded == false)
        {
            Vector3 velocity = movementDirection * inputMagnitude * jumpHorizontalSpeed;
            velocity.y = ySpeed;

            characterController.Move(velocity * Time.deltaTime);
        }

        if (Input.GetMouseButtonDown(0))
        {
            int randomIndex = Random.Range(0, attackAnimations.Length);
            animator.SetTrigger(attackAnimations[randomIndex]);

            /*
            
            // Cast a ray to detect the enemy object
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                // Check if the raycast hit the enemy object
                if (hit.collider.CompareTag("Enemy"))
                {
                    ChaseAnimation chaseAnimation = FindObjectOfType<ChaseAnimation>();
                    chaseAnimation.TakeDamage();
                }
            }
            */

        }

        if (health <= 0 && !isDead)
        {
            isDead = true;
            animator.SetTrigger("IsDead");
        }

        if(isDead)
        {
            
        }
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void TakeDamage()
    {
        health -= 10;
        animator.SetTrigger("IsAttacked");
    }

    private void OnAnimatorMove()
    {
        if (isGrounded)
        {
            Vector3 velocity = animator.deltaPosition;
            velocity.y = ySpeed * Time.deltaTime;

            characterController.Move(velocity);
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if(focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    /*
    private void Attack()
    {
        animator.SetTrigger("PunchRight");
    }
    private void Attack2()
    {
        animator.SetTrigger("PunchLeft");
    }
    private void Attack3()
    {
        animator.SetTrigger("Kick");
    }

    private void Attack4()
    {
        animator.SetTrigger("Kick2");
    }
    */
}