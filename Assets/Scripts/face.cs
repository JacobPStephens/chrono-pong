using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class face : MonoBehaviour
{
    // Start is called before the first frame update
    private SpriteRenderer sr;
    public Sprite[] faces;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = faces[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator HitBall() {
        Debug.Log("Switching face.");
        sr.sprite = faces[1];
        yield return new WaitForSeconds(0.5f);
        sr.sprite = faces[0];

    }

}
