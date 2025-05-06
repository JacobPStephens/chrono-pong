using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class light : MonoBehaviour
{
    // Start is called before the first frame update
    public Material mat;
    private float hue;
    private float val;
    public float colorSpeed;
    void Start()
    {
        
        hue = 0f;
        val = 0f;
        mat.SetColor("_EmissionColor", Color.HSVToRGB(0f, 0f, 1f));
        
    }

    // Update is called once per frame
    void Update()
    {

        //val = (val + Time.deltaTime * colorSpeed) % 1f;
        //mat.SetColor("_EmissionColor", Color.HSVToRGB(0f, 0f, val));
        //hue = (hue + Time.deltaTime * colorSpeed) % 1f;
        //Debug.Log(hue);
        
    }
}
