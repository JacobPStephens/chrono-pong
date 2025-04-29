using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball_collision : MonoBehaviour
{

    public debug debugScript;

    public GameObject net;

    // Start is called before the first frame update
    void Start()
    {
        // ut oh spagetti oh
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision col){
        if(col.gameObject.name == "floor"){
            debugScript.resetBall = true;
        }
    }

    void OnCollisionExit(Collision collisionInfo){
        if(collisionInfo.gameObject.name == "net" && transform.position.z >= 0.331f){
            debugScript.resetBall = true;
        }
    }
}
