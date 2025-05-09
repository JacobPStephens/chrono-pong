using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class debug : MonoBehaviour
{
    
    [Header("References")]

    public GameObject leftController;
    public GameObject rightController;

    public GameObject ball;
    public Rigidbody ballRb;
    public ball ballScript;
    public InputActionAsset inputActions;
    public float launchSpeed;
    public Vector3 spawnPoint;
    
    public float resetTime = 2f;
    public float resetTimer = 2f;

    public bool resetBall;


    // public float yBounds;

    // Start is called before the first frame update
    void Start()
    {   
        ballRb = ball.GetComponent<Rigidbody>();
        spawnPoint = ball.transform.position;
        LaunchBall();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(rightController.transform.position);
    }



    public void LaunchBall() {
        ballRb.AddForce(new Vector3(0,0,1) * launchSpeed, ForceMode.VelocityChange);
    }

    void ResetBall(){
        ball.transform.position = spawnPoint;
        ball.transform.eulerAngles = Vector3.zero;
        ballRb.velocity = Vector3.zero;
        ballRb.useGravity = false;

        //yield return new WaitForSecondsRealtime(0.00000000001f);

        ballRb.useGravity = true;
        
        LaunchBall();

    }

    void FixedUpdate() {
        if (resetBall) {
            //LaunchBall();
            //StartCoroutine(ResetBall());
            ResetBall();
            resetBall = false;
        }

    }

    //void FixedUpdate() {
    // public void PressSpace(InputAction.CallbackContext context) {
            
    //         if (context.performed) {
    //             ballScript.playerLastTouched = false;
    //             ballScript.playerLastZone = false;
    //             resetBall = true;
    //         }
            
    //     }
    public void pressA(InputAction.CallbackContext context) {
                
            if (context.performed) {

                //Debug.Log("a pressed");
                ballScript.playerLastTouched = false;
                ballScript.playerLastZone = false;
                resetBall = true;
                //Time.timeScale = 0.5f;
            }
        }
    //}
    

}
