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
    public Queue<Vector3> pos = new Queue<Vector3>();
    public int maxRecordings;
    public float recordInterval;
    public float playbackInterval;

    // Start is called before the first frame update
    void Start()
    {
        timeState = "normal";
        InvokeRepeating("RecordPositions", 0f, recordInterval);
    }
    void Update()
    {   
        // go until slow
        if ((timeState == "normal") && (bufferTimer <= 0f && bufferTimer > -50f)) {
            ChangeTimeScale(0.5f);
            timeState = "slow";
        }
        if (timeState == "rewind" && rewindTimer <= 0f) {
            Debug.Log("Stop rewind debug");
            StopRewind();
        }

        UpdateDebugTask();
        HandleMeter();
        HandleTimers();
    }

    void RecordPositions() {

        pos.Enqueue(transform.position);
        if (pos.Count > maxRecordings) {
            pos.Dequeue();
        }
        for (int i = 0; i < maxRecordings; i++) {
            Debug.Log(pos.ToArray()[i]);
        }
    }


    // Maybe use a timer instead because if you exit early then coroutine might not work
    // IEnumerator Rewind() {

    //     timeState = "rewind";
    //     GetComponent<Rigidbody>().isKinematic = false;

    //     yield return new WaitForSeconds(rewindDuration);

    //     timeState = "normal";
    //     GetComponent<Rigidbody>().isKinematic = true;  

    // }

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

    void StartRewind() {
        rewindTimer = rewindTime;
        GetComponent<Rigidbody>().isKinematic = true;
        Debug.Log("Start rewind.");
    }
    void StopRewind() {
        GetComponent<Rigidbody>().isKinematic = false;
        timeState = "normal";
        Debug.Log("Stop rewind debug.");
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

    void FixedUpdate() {
        if (reset) {
            ResetBall();
            SimulateHit();
            reset = false;
        }

    }

    void ResetBall() {            

        transform.position = Vector3.zero;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
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
                timeState = "rewind";
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
