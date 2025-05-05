using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour
{

    public debug debugScript;

    public time timeScript;

    public bool playerLastZone;

    public bool playerLastTouched;

    public bool round_ended;

    public float doubleBounceBuffer;

    public float doubleBounceTime;

    // Start is called before the first frame update
    void Start()
    {
        // ut oh spagetti oh
    }

    // Update is called once per frame
    void Update()
    {
        if (doubleBounceBuffer >= 0){
            doubleBounceBuffer -= Time.deltaTime;
        }
        if(round_ended){
            playerLastTouched = false;
            playerLastZone = false;
            timeScript.state = new LinkedList<(Vector3, Vector3)>();
            round_ended = false;
        }
    }

    void OnTriggerEnter(Collider zone){
        //Debug.Log(zone.gameObject.name);
        if(zone.gameObject.name == "floor_zone"){
            OutOfBoundsZone();
        }
        if(zone.gameObject.name == "player_table_zone"){
            Debug.Log(playerLastZone);
            TriggerPlayerZone();
        }
        if(zone.gameObject.name == "opponent_table_zone"){
            TriggerOpponentZone();
        }
        if(zone.gameObject.name == "player_net_zone"){
            TriggerPlayerNetZone();
        }
        if(zone.gameObject.name == "opponent_net_zone"){
            TriggerOpponentNetZone();
        }
    }
    public void TriggerPlayerZone(){
        if(playerLastZone && doubleBounceBuffer <=0 || playerLastTouched){
            //Debug.Log("Opponent gets point");
            //Debug.Log("In Trigger");
            EndRound();
        }
        if (!playerLastZone) {
            playerLastZone = true;
            doubleBounceBuffer = doubleBounceTime;
        }
    }
    public void TriggerOpponentZone(){
        if(!playerLastZone && doubleBounceBuffer <=0){
            //Debug.Log("Player gets point");
            EndRound();
        }
        if (playerLastZone) {
            playerLastZone = false;
            doubleBounceBuffer = doubleBounceTime;
        }
    }
    public void TriggerPlayerNetZone(){
        //Debug.Log("Opponent gets point");
        EndRound();
    }
    public void TriggerOpponentNetZone(){
        //Debug.Log("Player gets point");
        EndRound();
    }
    public void OutOfBoundsZone(){
        if(playerLastTouched == playerLastZone){
            if(playerLastTouched){
                //Debug.Log("Opponent gets point");
            }
            else{
                //Debug.Log("Player gets point");
            }
        }
        else{
            if(playerLastTouched){
                //Debug.Log("Player gets point");
            }
            else{
                //Debug.Log("Opponent gets point");
            }
        }

        EndRound();
    }
    public void EndRound(){
        //Debug.Log("Round ended");
        debugScript.resetBall = true;
        round_ended = true;
        //Debug.Log(playerLastZone);
    }
}
