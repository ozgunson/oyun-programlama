using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // 5
    [SerializeField]
    private float maximumSpeed;
    // 720
    [SerializeField]
    private float rotationSpeed;
    // 5
    [SerializeField]
    private float jumpSpeed;
    // this is the amount of second leeway give our player, 0.2s
    [SerializeField]
    private float jumpButtonGracePeriod;

    [SerializeField]
    private Transform cameraTransform;


    private Animator animator;

    private CharacterController characterController;
    // increase when jump, decrease it over time according to gravity.
    private float ySpeed;
    // we prevent a glitch by using this.
    private float originalStepOffset;
    // nullable, we clear the value when our character jumps
    private float? lastGroundTime;
    // nullable
    private float? jumpButtonPressedTime;
    // Start is called before the first frame update
    void Start()
    {
        // get access to the animator.
        animator = GetComponent<Animator>();
        // get access to the controller.
        characterController = GetComponent<CharacterController>();
        // original stepoffset of our character
        originalStepOffset = characterController.stepOffset;
    }

    // Update is called once per frame
    void Update()
    {
        // Input from the player horizontal and vertical axis, this will result -1 and 1, by default -1 is left(a) and 1 is right(d) , same thing for w and s
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // we need to 3 dimensional vector to direction we want to move
        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        // we make sure our magnitude never above 1 with clamp01 method, limit magnitude 0 and 1, divided by two for the walking.
        // we'll use it to set the animator parameter.
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude) / 2;
        // when we press shift, it will run.  1 is running 0.5 is walking 0 is idle
        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            inputMagnitude *= 2;
        }
        // damping ensures the animation blending doesn't change too quickly.
        animator.SetFloat("Input Magnitude", inputMagnitude, 0.05f, Time.deltaTime);

        // 
        float speed = inputMagnitude * maximumSpeed;
        // change the movement direction based on the camera rotation, around the y axis. this will apply the y rotation of the camera to the movement direction.
        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        // normalizing a vector maintains the direction but sets the magnitude to 1.
        movementDirection.Normalize();

        // getting the physics gravity value and adding this amount to our y speed every second. Time.deltaTime is the amount of seconds that have passed since the last frame.
        // default physics gravity value is -9.81
        ySpeed += Physics.gravity.y * Time.deltaTime;


        // JUMPING


        // check the character is on the ground.
        if (characterController.isGrounded)
        {
            // number of seconds since the game started.
            lastGroundTime = Time.time;
        }
        // check jumpbutton is pressed
        if(Input.GetButtonDown("Jump"))
        {
            jumpButtonPressedTime = Time.time;
        }
        // presses buttton slightly too early or too late it will still count and make the jump
        // if it was on the ground in the grace period.
        if (Time.time - lastGroundTime <= jumpButtonGracePeriod)
        {
            // when it's on the ground we set it again the original value.
            characterController.stepOffset = originalStepOffset;
            // instead of resetting the y speed to zero when it's on the ground, we set -0.5, this amount is enough to keep the character on the ground.
            ySpeed = -0.5f;
            // amount of time has passed since the button has been pressed then check if it is less or equal to grace period.
            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
            {
                ySpeed = jumpSpeed;
                // we reset our nullable fields when the character jumps, so it doesn't jump repeatedly while in the grace period.
                jumpButtonPressedTime = null;
                lastGroundTime = null;
                animator.SetBool("IsJumping", true);
            }
        }
        else
        {
            // stepoffset 0 when the character is not on the ground.
            characterController.stepOffset = 0;
            animator.SetBool("IsJumping", false);
        }
        // use our yspeed when moving the character
        Vector3 velocity = movementDirection * speed;
        velocity.y = ySpeed;

        characterController.Move(velocity * Time.deltaTime);
        
        // is character moving?
        if(movementDirection != Vector3.zero)
        {
            // if our character moving, set true the parameter IsMoving on the animator. it's not necessary after blendtree
            //animator.SetBool("IsMoving", true);


            // quaternion variable, type specifically for storing rotations, lookrotation method to create a rotation looking in the desired direction.
            // set to the forward direction to the movement direction, and up direction to the y axis.
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            // change the rotation of our character we use rotatetowards method to rotate from our current rotation towards the desired direction, rotationspeed for how quickly our character rotates.
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        /*
        else
        {
            // if it's not moving
            animator.SetBool("IsMoving", false);
        }*/


        // attack key conditions
        if (Input.GetKeyDown(KeyCode.F))
        {
            Attack();
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            Attack2();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            Attack3();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            Attack4();
        }

    }
    // hide the cursor when the application gets focus.
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

    // attack trigger methods
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
}
