using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Rendering;

public class timeTest : MonoBehaviour
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
    public Rigidbody rb;
    
    private (Vector3, Vector3) mostRecentState;
    private bool record; // public for debug only

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        timeState = "normal";
        InvokeRepeating("SetRecordTrue", 0f, recordInterval);
    }
    void Update()
    {   

        // go until slow
        if ((timeState == "normal") && (bufferTimer <= 0f && bufferTimer > -50f)) {
            ChangeTimeScale(0.5f);
            timeState = "slow";
        }
        if (timeState == "rewind" && rewindTimer <= 0f) {
            //Debug.Log("Stop rewind debug");
            StopRewind();
        }

        UpdateDebugTask();
        HandleMeter();
        HandleTimers();
    }
    void FixedUpdate() {
        if (reset) {
            ResetBall();
            SimulateHit();
            reset = false;
        }
        if (record) {
            Record();
            record = false;
        }

    }

    void Record() {
        state.AddLast((transform.position, rb.velocity));
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

    void PrintQueue(Queue<Vector3> q) {
        string res = "";

        for (int i = 0; i < q.Count; i++) {
            res += q.ToArray()[i] + " ";
        }
        Debug.Log("Queue: " + res);
        
    }


    void HandleMeter() {
        if (timeState == "normal") {
            meter += meterPassiveRegen * Time.deltaTime;
        }
        if (timeState == "slow") {
            meter -= Time.deltaTime;
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

        Debug.Log("inside sewind");
        while (timeState == "rewind") {

            if (state.Count > 0) {
                mostRecentState = state.Last.Value;
                state.RemoveLast();
                transform.position = mostRecentState.Item1;
            }

            //Debug.DrawLine(transform.position, transform.position + mostRecentState.Item2);
            Debug.Log("rewinding...");
            yield return new WaitForSeconds(playbackInterval);
        }


    }

    void StartRewind() {
        timeState = "rewind";
        rewindTimer = rewindTime;
        GameObject.FindGameObjectWithTag("light").GetComponent<Light>().enabled = false;

        StartCoroutine(Playback());
        GetComponent<Rigidbody>().isKinematic = true;
        Debug.Log("Start rewind.");
    }
    void StopRewind() {


        timeState = "normal";
        GameObject.FindGameObjectWithTag("light").GetComponent<Light>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;

        rb.velocity = mostRecentState.Item2;    
        state.RemoveLast();
        //Debug.Log("Stop rewind debug.");
    }
    void UpdateDebugTask() {
        GameObject txtObj = GameObject.FindGameObjectWithTag("text");
        txtObj.GetComponent<TMPro.TextMeshProUGUI>().text = meter.ToString("F2");
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



    void ResetBall() {            

        transform.position = Vector3.zero; 
        
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        rb.velocity = Vector3.up * 5f;
    }

    // Update is called once per frame

    public void PressSpace(InputAction.CallbackContext context) {
        
        if (context.performed) {
            Debug.Log("Space pressed.");
            reset = true;
        }

        if (context.canceled) {
            Debug.Log("Space context canceled.");
        }
        
    }

    void OnPress() {
        if (bufferTimer > 0f) {
            if (meter >= 0.95f * maxMeter){
                StartRewind();
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

    public void PressLeftButton(InputAction.CallbackContext context) {
        if (context.performed) { 
            left = true;
            OnPress();
        }
        if (context.canceled) { 
            left = false;
            OnRelease();
        }
    }

    public void PressRightButton(InputAction.CallbackContext context) {
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
