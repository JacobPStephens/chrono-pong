using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class light : MonoBehaviour
{
    // Start is called before the first frame update
    public Material mat;
    private float hue;
    public float colorSpeed;

    public bool rainbow;
    void Start()
    {
        rainbow = false;
        mat.SetColor("_EmissionColor", Color.HSVToRGB(0f, 0f, 0f));
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!rainbow) {
            return;
        }

        Debug.Log("light in update");
        mat.SetColor("_EmissionColor", Color.HSVToRGB(hue, 1f, 1f));
        hue = (hue + Time.deltaTime * colorSpeed) % 1f;
        
    }

}
