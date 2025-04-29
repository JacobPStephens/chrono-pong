using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rightPaddle : MonoBehaviour
{

    public GameObject rightController;
    public Vector3 previousPosition;
    public bool ballHit;
    public Transform parent;

    public float recentMax;
    public float debugTimer;
    public float debugTime;
    public bool nextFrame;
    public GameObject ball;
    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;

        //Debug.Log(parent.name);
    }

    void FixedUpdate() {
        Rigidbody ballRb = ball.GetComponent<Rigidbody>();
        Vector3 velocity = (parent.position - previousPosition) / Time.fixedDeltaTime;
        previousPosition = parent.position;

        if (nextFrame) {
            Debug.Log(ballRb.velocity);
            // if (ballRb.velocity.z > 0) {
            //     ballRb.velocity = new Vector3(ballRb.velocity.x, ballRb.velocity.y, -ballRb.velocity.z);
            // }
            nextFrame = false;
        }
        if (ballHit) {
            HitBall(ball, velocity);
            ballHit = false;
        }





    }

    void HitBall(GameObject ball, Vector3 paddleVelocity) {
        Rigidbody ballRb = ball.GetComponent<Rigidbody>();
        //ballRb.velocity = new Vector3(ballRb.velocity.x, ballRb.velocity.y, 0f);
       // Vector3 newVelocity = new Vector3(ballRb.velocity.x + paddleVelocity.x, ballRb.velocity.y + paddleVelocity.y, ballRb.velocity.z + paddleVelocity.z);

        Debug.Log("HIT BALL. PREVIOUS BALL VELOCITY = " + ballRb.velocity);
        Debug.Log("HIT BALL. PADDLE VELOCITY = " + paddleVelocity);
        ballRb.AddForce(paddleVelocity, ForceMode.Impulse);
        nextFrame = true;
        //debugTimer = debugTime;


    }

    void OnCollisionEnter(Collision col) {
        //Debug.Log("paddle collided with " + col.gameObject.name);

        if (col.gameObject.name != "ball") {
            return;
        }

        ballHit = true;

    }
    // Update is called once per frame
    void Update()
    {
        // transform.position = rightController.transform.position;
        // transform.rotation = rightController.transform.rotation;
    }
}
