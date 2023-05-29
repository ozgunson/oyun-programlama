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

    private float horizontalInput;
    private float verticalInput;
    private Vector3 movementDirection;
    private float inputMagnitude;
    private float gravity;


    void Update()
    {
        HandleInput();
        UpdateMovement();
        UpdateJump();
        UpdateGravity();
        UpdateAnimation();
        //CheckAttack();
        CheckDeath();
        //OnAnimatorMove();
    }

    void HandleInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        inputMagnitude = Mathf.Clamp01(movementDirection.magnitude) / 2;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            inputMagnitude *= 2;
        }

        animator.SetFloat("Input Magnitude", inputMagnitude, 0.05f, Time.deltaTime);
    }

    void UpdateMovement()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.Normalize();

        if (movementDirection != Vector3.zero)
        {
            animator.SetBool("IsMoving", true);
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            //
            // Kameranýn yukarý/aþaðý dönüþünü kontrol et
            //float rotationX = cameraTransform.rotation.eulerAngles.x - verticalInput * rotationSpeed * Time.deltaTime;
            //rotationX = Mathf.Clamp(rotationX, -90f, 90f); // Kameranýn sýnýrlý bir açýyla dönmesini saðla

            // Kameranýn yönünü güncelle
            //cameraTransform.rotation = Quaternion.Euler(rotationX, cameraTransform.rotation.eulerAngles.y, cameraTransform.rotation.eulerAngles.z);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }

        if (isGrounded == false)
        {
            Vector3 velocity = movementDirection * inputMagnitude * jumpHorizontalSpeed;
            velocity.y = ySpeed;

            characterController.Move(velocity * Time.deltaTime);
        }

    }

    void UpdateJump()
    {
        if (characterController.isGrounded)
        {
            lastGroundTime = Time.time;
        }

        if (Input.GetButtonDown("Jump"))
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
    }

    void UpdateGravity()
    {
        gravity = Physics.gravity.y * gravityMultiplier;

        if (isJumping && ySpeed > 0 && !Input.GetButton("Jump"))
        {
            gravity *= 2;
        }

        ySpeed += gravity * Time.deltaTime;
    }

    void UpdateAnimation()
    {
        if (health <= 0 && !isDead)
        {
            isDead = true;
            animator.SetTrigger("IsDead");
        }

        if (isDead)
        {
            // Handle death animation
        }
    }
    /*
    void CheckAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            enemyTarget = GameObject.FindGameObjectWithTag("Enemy").transform;
            float distanceToTarget = Vector3.Distance(transform.position, enemyTarget.position);

            if (distanceToTarget <= attackRange && Time.time >= nextAttackTime)
            {
                int randomIndex = Random.Range(0, attackAnimations.Length);
                animator.SetTrigger(attackAnimations[randomIndex]);
                enemyTarget.GetComponent<ChaseAnimation>().TakeDamage();

                nextAttackTime = Time.time + 1f / attackRate;
            }
            
        }
    }*/

    void CheckDeath()
    {
        if (health <= 0 && !isDead)
        {
            isDead = true;
            animator.SetTrigger("IsDead");
        }

        if (isDead)
        {
            // Handle death animation
        }
    }




    public bool IsDead()
    {
        return isDead;
    }

    public void TakeDamage()
    {
        health -= 20;
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