using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class opponent : MonoBehaviour
{

    public bool returnBall;
    public Vector3 returnVelocity;
    public GameObject ball; 

    public GameObject target;

    public ball ballScript;

    // Start is called before the first frame update
    void Start()
    {   


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {

        if (returnBall) {

            ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ball.GetComponent<Rigidbody>().AddForce(returnVelocity, ForceMode.VelocityChange);
            //Debug.Log(returnVelocity);
            returnBall = false;

        }
    }

    public void ReturnBall(GameObject ball) {

        Rigidbody ballRb = ball.GetComponent<Rigidbody>(); 
        Vector3 targetPos = GetRandomTarget();
        float shotAngle = GetRandomShotAngle();
        target.transform.position = targetPos;

        //Debug.Log("Launching ball at target " +targetPos+ " with shot angle " +shotAngle);
        Vector3 velocity = GetVelocityGivenAngle(ball.transform.position, targetPos, shotAngle);


        //ballRb.AddForce(velocity, ForceMode.VelocityChange);
        returnBall = true;
        returnVelocity = velocity;
        ballScript.playerLastTouched = false;
        //Debug.Log("Changed velocity to " + velocity);//;+ " Actual Velocity " + ballRb.velocity);

    }

    private void StopBall(Rigidbody ballRb) {
        ballRb.velocity = Vector3.zero;
                
    }
    private Vector3 GetRandomTarget() {
        float backEdge = 2.66f;
        float leftEdge = -0.4f;
        float rightEdge = 0.2f;
        float frontEdge = 2.00f;
        return new Vector3(Random.Range(leftEdge, rightEdge), 1f, Random.Range(frontEdge, backEdge));
    }
    public float GetRandomShotAngle() {
        return Random.Range(45f, 50f);
    }
    public Vector3 GetVelocityGivenAngle(Vector3 currentPosition, Vector3 targetPosition, float verticalDegrees) {
        float radians = verticalDegrees * Mathf.Deg2Rad;
        Vector3 diff = targetPosition - currentPosition;

        float horDist = Mathf.Sqrt(diff.x * diff.x + diff.z * diff.z);
        
        float velocity = Mathf.Sqrt(
            9.81f * horDist * horDist /
            (2 * Mathf.Cos(radians) * Mathf.Cos(radians) * 
            (horDist * Mathf.Tan(radians) + -diff.y))
        );

        return new Vector3(
        diff.x / horDist * velocity * Mathf.Cos(radians),  // x
        velocity * Mathf.Sin(radians),                     // y
        diff.z / horDist * velocity * Mathf.Cos(radians)); // z
    }

    
    void OnTriggerEnter(Collider other) {

        if (other.name != "ball") {
            return;
        }   

        ball = other.gameObject;

        if(ballScript.playerLastZone){
            ballScript.EndRound();
        }

        ReturnBall(ball);
    }
}
