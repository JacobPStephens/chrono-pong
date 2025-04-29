using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball_target_test : MonoBehaviour
{

    // public float radians;
    // public float xAngle;
    // public float zComponent;
    // public float yComponent;
    // public float velocity;
    public float degrees;
    public Rigidbody rb;
    public Transform plane;

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log(Mathf.Sin(radians));
        // Debug.Log(Mathf.Cos(radians));
        // zComponent = Mathf.Cos(radians);
        // yComponent = Mathf.Sin(radians);
        rb = GetComponent<Rigidbody>();
        //Vector3 components = GetComponentsGivenAngle(transform.position,plane.position,degrees);
        Vector3 components = GetVelocityGivenAngle(transform.position,plane.position,degrees);

        //rb.AddForce(3.615f, 4.062f, 6.026f, ForceMode.VelocityChange);
        rb.AddForce(components, ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void Update()
    {
        // Jacob's comment
    }

    public Vector3 GetVelocityGivenAngle(Vector3 currentPosition, Vector3 targetPosition, float verticalDegrees) {
        float radians = verticalDegrees * Mathf.Deg2Rad;
        Vector3 diff = targetPosition - currentPosition;

        float horDist = Mathf.Sqrt(diff.x * diff.x + diff.z * diff.z);
        
        float velocity = Mathf.Sqrt(
            9.81f * horDist * horDist /
            2 * Mathf.Cos(radians) * Mathf.Cos(radians) * 
            (horDist * Mathf.Tan(radians) + -diff.y)
        );

        return new Vector3(
        diff.x / horDist * velocity * Mathf.Cos(radians),  // x
        velocity * Mathf.Sin(radians),                     // y
        diff.z / horDist * velocity * Mathf.Cos(radians)); // z
    }


}
