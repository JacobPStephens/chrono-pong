using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class debug : MonoBehaviour
{


    public GameObject leftController;
    public GameObject rightController;

    public GameObject ball;
    public Rigidbody ballRb;
    public InputActionAsset inputActions;
    public float launchSpeed;
    // Start is called before the first frame update
    void Start()
    {   
        ballRb = ball.GetComponent<Rigidbody>();

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
    }


    void LaunchBall() {
        ballRb.velocity = new Vector3(0f, launchSpeed, 0f);
    }

    public void PressSpace(InputAction.CallbackContext context) {
        
        if (context.performed) { Debug.Log("space pressed.");
            LaunchBall();
        }
        
    }
    public void pressA(InputAction.CallbackContext context) {
        
        if (context.performed) {
            Debug.Log("right primary pressed.");
        }
    }

}
