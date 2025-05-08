using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


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
    public Vector3 velocity;
    public GameObject ball;
    public XRBaseController xrRight;

    public ball ballScript;

    public time timeScript;
    public Queue<Vector3> lastVelocities = new Queue<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;

        //Debug.Log(parent.name);
    }

    void FixedUpdate() {
        Rigidbody ballRb = ball.GetComponent<Rigidbody>();
        // if (timeScript.timeState == "slow") {
        //     Vector3 velocity = (parent.position - previousPosition) / (Time.fixedDeltaTime/Time.timeScale);
        // }
        // else {
        //     Vector3 velocity = (parent.position - previousPosition) / Time.fixedDeltaTime;
        // }
        Vector3 velocity = ((parent.position - previousPosition) / Time.fixedDeltaTime) * Time.timeScale;
        previousPosition = parent.position;

        //Debug.Log(lastVelocities);
        lastVelocities.Enqueue(velocity);
        if (lastVelocities.Count > 3) {
            lastVelocities.Dequeue();
        }

        if (nextFrame) {
            //Debug.Log(ballRb.velocity);
            // if (ballRb.velocity.z > 0) {
            //     ballRb.velocity = new Vector3(ballRb.velocity.x, ballRb.velocity.y, -ballRb.velocity.z);
            // }
            nextFrame = false;
        }
        if (ballHit) {
            //Debug.Log("BALL HIT");
            HitBall(ball, QueueAverage(lastVelocities));
            //HitBall(ball, velocity);
            ballHit = false;
        }





    }

    private Vector3 QueueAverage(Queue<Vector3> q) {
        
        //Debug.Log("IN QUEUE AVG FUNCTION....");

        Vector3[] tmpArr = q.ToArray();
        Vector3 total = Vector3.zero;
        for (int i = 0; i < q.Count; i++) {
            total += tmpArr[i];
        }
        //Debug.Log(tmpArr);
        //Debug.Log("Last frame velocity is " + tmpArr[tmpArr.Length-1]);

        return (total / q.Count);
    }

    void HitBall(GameObject ball, Vector3 paddleVelocity) {
        Rigidbody ballRb = ball.GetComponent<Rigidbody>();
        //ballRb.velocity = new Vector3(ballRb.velocity.x, ballRb.velocity.y, 0f);
       // Vector3 newVelocity = new Vector3(ballRb.velocity.x + paddleVelocity.x, ballRb.velocity.y + paddleVelocity.y, ballRb.velocity.z + paddleVelocity.z);

        //Debug.Log("HIT BALL. PREVIOUS BALL VELOCITY = " + ballRb.velocity);
        //Debug.Log("HIT BALL. PADDLE VELOCITY = " + paddleVelocity);
        TriggerHaptic();
        ballRb.AddForce(paddleVelocity, ForceMode.Impulse);
        ballScript.playerLastTouched = true;
        ballScript.playerLastZone = true;
        nextFrame = true;

        ballScript.AudioPlayBounce();
        //debugTimer = debugTime;


    }

    public void TriggerHaptic() {
        xrRight.SendHapticImpulse(.5f,.05f);
    }

    void OnCollisionEnter(Collision col) {
        //Debug.Log("paddle collided with " + col.gameObject.name);

        if (col.gameObject.name == "ball") {
            ballHit = true;

            StartCoroutine(DisableCollider());
            
        }

        

    }

    IEnumerator DisableCollider() {

        GetComponent<BoxCollider>().isTrigger = true;
        yield return new WaitForSeconds(0.5f);
        GetComponent<BoxCollider>().isTrigger = false;


    }
    // Update is called once per frame
    void Update()
    {
        transform.position = rightController.transform.position;
        transform.rotation = rightController.transform.rotation * Quaternion.Euler(35f,173f,90f);
    }
}
