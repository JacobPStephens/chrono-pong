using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oppPaddle : MonoBehaviour
{
    // Start is called before the first frame update

    private Vector3 restPos;

    public float popSize;
    public float popDuration;
    public float returnSpeed;
    void Start()
    {
        // set restPos to initial position
        restPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }



    public IEnumerator MoveToRest() {

        while (true) {
            transform.position = Vector3.MoveTowards(transform.position, restPos, returnSpeed * Time.deltaTime);
            yield return new WaitForSeconds(0.05f);
        }
    }

    public IEnumerator ResetSize() {

        yield return new WaitForSeconds(popDuration);
        transform.localScale = Vector3.one * 1f;
    }
    // called from opponent script
    public void MovePaddle(Vector3 ballReturnPosition) {
        StopCoroutine(MoveToRest());
        transform.position = ballReturnPosition;
        StartCoroutine(MoveToRest());

        transform.localScale = Vector3.one * popSize;
        StartCoroutine(ResetSize());

    }
}
