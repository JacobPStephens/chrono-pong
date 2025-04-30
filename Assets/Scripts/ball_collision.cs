using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball_collision : MonoBehaviour
{

    public debug debugScript;

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
        Debug.Log(col.gameObject.name);
        if(col.gameObject.name == "floor"){
            debugScript.resetBall = true;
        }
    }

    void OnCollisionExit(Collision collisionInfo){
        if(collisionInfo.gameObject.name == "net"){
            debugScript.resetBall = true;
        }
    }
}
