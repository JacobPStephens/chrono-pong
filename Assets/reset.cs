using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reset : MonoBehaviour
{

    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        
        timer -= Time.deltaTime;

        if (timer < 0f) {
            transform.position = new Vector3(0f, 5f, 0f);
            timer = 3f;
        }
    }
}
