using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Rendering;

public class time : MonoBehaviour
{

    private bool reset;
    private bool left;
    private bool right;
    public string timeState; // "rewind", "slow", or "normal" ONLY 
    public float meter;
    public float maxMeter;
    public float meterOnHitRegen;
    public float meterPassiveRegen;
    public float bufferTimer;
    public float bufferTime;
    public float rewindTime;
    public float rewindTimer;
    public InputActionAsset inputActions;
    public LinkedList<(Vector3, Vector3)> state = new LinkedList<(Vector3, Vector3)>();

    public int maxRecordings;
    public float recordInterval;
    public float playbackInterval;
    public GameObject ball;
    private Rigidbody ballRb;

    public float slowDecrement;

    
    
    private (Vector3, Vector3) mostRecentState;
    private bool record; // public for debug only

    public GameObject rightMeterUI;
    public GameObject leftMeterUI;
    // Start is called before the first frame update
    void Start()
    {
        ballRb = ball.GetComponent<Rigidbody>();
        timeState = "normal";
        InvokeRepeating("SetRecordTrue", 0f, recordInterval);
        meter = maxMeter;
    }
    void Update()
    {   

        //Debug.Log(timeState);

        // go until slow
        if ((timeState == "normal") && (bufferTimer <= 0f && bufferTimer > -50f)) {
            ChangeTimeScale(0.5f);
            timeState = "slow";
        }
        if (timeState == "rewind" && rewindTimer <= 0f) {
            //Debug.Log("Stop rewind debug");
            StopRewind();
        }

        HandleMeter();
        HandleMeterUI(rightMeterUI);
        HandleMeterUI(leftMeterUI);
        HandleTimers();
        

    }
    void FixedUpdate() {
        if (reset) {
            //SimulateHit();
            reset = false;
        }
        if (record) {
            //Record();
            record = false;
        }

    }
    void HandleMeterUI(GameObject meterUI) {
        float meterPercent = meter / maxMeter;
        meterUI.transform.localScale = new Vector3(0.99f*meterPercent, 1.1f, 0.99f*meterPercent);
    }
    void Record() {
        state.AddLast((ball.transform.position, ballRb.velocity));
        if (state.Count > maxRecordings) {
            state.RemoveFirst();
        }
    }
    void SetRecordTrue() {
        if (timeState == "rewind") {
            return;
        }
        record = true;
    }

    void HandleMeter() {
        if (timeState == "normal") {
            meter += meterPassiveRegen * Time.deltaTime;
        }
        if (timeState == "slow") {
            meter -= Time.deltaTime * slowDecrement;
            if (meter <= 0f) {
                timeState = "normal";
                ChangeTimeScale(1.0f);
                bufferTimer = -100f;
            }
        }
        if (timeState == "rewind") {
            meter = 0f;
        }

        meter = Mathf.Max(0f, meter);
        meter = Mathf.Min(meter, maxMeter);

    }

    IEnumerator Playback() {

        //Debug.Log("inside rewind");
        while (timeState == "rewind") {

            if (state.Count > 0) {
                mostRecentState = state.Last.Value;
                state.RemoveLast();
                ball.transform.position = Vector3.Lerp(ball.transform.position, mostRecentState.Item1, 100*Time.deltaTime);
            }

            //Debug.DrawLine(transform.position, transform.position + mostRecentState.Item2);
            //Debug.Log("rewinding...");
            yield return new WaitForSeconds(playbackInterval);
        }


    }

    void StartRewind() {
        timeState = "rewind";
        rewindTimer = rewindTime;
        StartCoroutine(Playback());
        ballRb.isKinematic = true;
        //Debug.Log("Start rewind.");
    }
    void StopRewind() {

        timeState = "normal";
        ballRb.isKinematic = false;

        ballRb.velocity = mostRecentState.Item2;    
        state.RemoveLast();
        //Debug.Log("Stop rewind debug.");
    }


    void HandleTimers() {
        if (bufferTimer > 0f) {
            bufferTimer -= Time.deltaTime;
        }
        if (rewindTimer > 0f) {
            rewindTimer -= Time.deltaTime;
        }
        
    }

    void ChangeTimeScale(float spd) {
        Time.timeScale = spd;
    }

    void SimulateHit() {
        if (timeState == "rewind") { return; }
        meter += meterOnHitRegen;
        if (timeState == "slow") {
            meter -= meterOnHitRegen / 2f;
        }
    }


    // Update is called once per frame

    public void PressSpace(InputAction.CallbackContext context) {
        
        if (context.performed) {
            //Debug.Log("Space pressed.");
            reset = true;
        }

        if (context.canceled) {
            //Debug.Log("Space context canceled.");
        }
        
    }

    void OnPress() {

        //Debug.Log("Action pressed");
    
        if (bufferTimer > 0f) {
            if (meter >= 0.95f * maxMeter){
                //StartRewind();
            }
            else {
                bufferTimer = -100f;
            }
        }
        else {
            bufferTimer = bufferTime;
        }
    }
    void OnRelease() {
        bufferTimer = -100f;
        rewindTimer = 0f;
        if (!left && !right && timeState == "slow") {
            ChangeTimeScale(1.0f);
            timeState = "normal";
        }
    }

    public void PressLeftGrip(InputAction.CallbackContext context) {
        if (context.performed) { 
            left = true;
            OnPress();
        }
        if (context.canceled) { 
            left = false;
            OnRelease();
        }
    }

    public void PressRightGrip(InputAction.CallbackContext context) {
        if (context.performed) { 
            right = true;
            OnPress();
        }
        if (context.canceled) { 
            right = false;
            OnRelease();
        }
    }
}
