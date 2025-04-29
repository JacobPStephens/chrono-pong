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
        Vector3 components = TestFunction(transform.position,plane.position,degrees);

        Debug.Log(components);
        //rb.AddForce(3.615f, 4.062f, 6.026f, ForceMode.VelocityChange);
        rb.AddForce(components,ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 TestFunction(Vector3 currentPosition, Vector3 targetPosition, float verticalDegrees) {
        float radians = verticalDegrees * Mathf.Deg2Rad;
        Vector3 diff = targetPosition - currentPosition;

        float horDist = Mathf.Sqrt(diff.x * diff.x + diff.z * diff.z);
        
        float numerator = 9.81f * horDist * horDist;
        Debug.Log("numerator" + numerator);
        // SHOULD DIFF Y BE NEGATIVE HERE?
        float denominator = 2 * Mathf.Cos(radians) * Mathf.Cos(radians) * (horDist * Mathf.Tan(radians) + -diff.y);
        Debug.Log("denominator" + denominator);
        float velocity = Mathf.Sqrt(numerator / denominator);
        float horVelocity = velocity * Mathf.Cos(radians);
        float vertVelocity = velocity * Mathf.Sin(radians);
        Debug.Log("velocity" + velocity);
        Debug.Log("horVelocity" + horVelocity);
        Debug.Log("vertVelocity" + vertVelocity);

        float normalizedX = diff.x / horDist;
        float normalizedZ = diff.z / horDist;

        return new Vector3(normalizedX * horVelocity, vertVelocity, normalizedZ * horVelocity);

    }

    public Vector3 GetComponentsGivenAngle(Vector3 currentPosition, Vector3 targetPosition, float verticalDegrees) {
        float radians = verticalDegrees * Mathf.Deg2Rad;
        float[] dir = new float[3]; 
        Vector3 diff = targetPosition - currentPosition;
        Debug.Log("diff=" + diff);
        //dir[0] = Mathf.Atan(diff.z / Mathf.Sqrt(diff.x * diff.x + diff.y * diff.y));
        dir[0] = Mathf.Cos(radians);
        dir[1] = Mathf.Sin(radians);
        dir[2] = Mathf.Cos(radians);
        for (int i = 0; i < 3; i ++) {
            Debug.Log(dir[i]);
        }
        Debug.Log(radians);
        Debug.Log("denom= " + Mathf.Sin(2 * radians));
        Debug.Log("numerator= " + 9.81f * diff.z);
        Debug.Log("total= " + 9.81f * diff.z / Mathf.Sin(2 * radians));
        float scalar = Mathf.Sqrt(9.81f * diff.z / Mathf.Sin(2 * radians));
        float xScalar = Mathf.Sqrt(9.81f * diff.x / Mathf.Sin(2 * radians));
        Debug.Log("velocity=" + scalar);

        Debug.Log(new Vector3(dir[0], dir[1], dir[2]) * scalar);
        return new Vector3(dir[0]*xScalar, dir[1]*scalar, dir[2]*scalar);
    }
}
