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
    public InputActionAsset inputActions;
    public float launchSpeed;
    public Vector3 spawnPoint;
    
    public float resetTime = 2f;
    public float resetTimer = 2f;

    public bool resetBall;
    // Start is called before the first frame update
    void Start()
    {   
        ballRb = ball.GetComponent<Rigidbody>();
        spawnPoint = ball.transform.position;

    }

    // Update is called once per frame
    void Update()
    {

        // Debug.Log("LeftControllerPosition" + leftController.transform.position);
        // Debug.Log("RightControllerPosition" + rightController.transform.position);

        // inputActions.
        // Debug.Log(inputActions);
        // var action = inputActions.actions[0];
        
        // action.performed += context => Debug.Log("performed");
        // if (Input.GetKeyDown(KeyCode.B)) {
        //     ballRb.velocity = new Vector3(0, 4f, 0f);
        // }
        // updated

        //Debug.Log(ballRb.velocity);
    }

    

    public void LaunchBall() {
        //Debug.Log("Ball fly");
        //ballRb.velocity = Vector3.zero;
        ball.transform.eulerAngles = Vector3.zero;
        ballRb.velocity = Vector3.zero;
        ball.transform.position = spawnPoint;
        ballRb.AddForce(ball.transform.forward * launchSpeed, ForceMode.Impulse);
    }

    void FixedUpdate() {

        if (resetBall) {
            LaunchBall();
            resetBall = false;
        }

    }

    //void FixedUpdate() {
    public void PressSpace(InputAction.CallbackContext context) {
            
            if (context.performed) {
                LaunchBall();
            }
            
        }
    public void pressA(InputAction.CallbackContext context) {
                
            if (context.performed) {
                resetBall = true;
            }
        }
    //}
    

}
