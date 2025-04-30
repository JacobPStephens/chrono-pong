using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour
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
        if(col.gameObject.name == "floor"){
            debugScript.resetBall = true;
        }
        if(col.gameObject.name == "net"){
            debugScript.resetBall = true;
        }
    }

    void OnTriggerEnter(Collider zone){
        
    }
}
