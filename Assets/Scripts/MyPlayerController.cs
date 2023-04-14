using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerController : MonoBehaviour
{
    Animator playerAnim;
    float playerSpeed;
    float axisZ;
    Camera mainCam;
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            playerSpeed = 0.3f;
            axisZ = playerSpeed * Input.GetAxis("Vertical");
            //playerAnim.SetFloat("speed", 0.4f);
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
            {
                playerSpeed = 1f;
                axisZ = playerSpeed * Input.GetAxis("Vertical");
                //playerAnim.SetFloat("speed", 1f);
            }
        }
        else
        {
            playerSpeed = 0f;
            axisZ = playerSpeed * Input.GetAxis("Vertical");
            //playerAnim.SetFloat("speed", 0f);
        }

        if(Input.GetKey(KeyCode.S))
        {
            playerSpeed = -0.3f;
            axisZ = playerSpeed * Input.GetAxis("Vertical");
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            playerAnim.SetBool("walkLeft", true);
        }
        if(Input.GetKeyUp(KeyCode.A))
        {
            playerAnim.SetBool("walkLeft", false);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            playerAnim.SetBool("walkRight", true);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            playerAnim.SetBool("walkRight", false);
        }

        Vector3 vector = new Vector3(0, 0, axisZ);

        // transition
        playerAnim.SetFloat("speed", Vector3.ClampMagnitude(vector, 1f).magnitude, 1f, Time.deltaTime * 3f);
        //playerAnim.SetFloat("speed", playerSpeed);


        Vector3 cameraWay = mainCam.transform.forward;
        cameraWay.y = 0f;
        transform.forward = cameraWay;
    }
}
