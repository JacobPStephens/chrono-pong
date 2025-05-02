using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour
{

    public debug debugScript;

    public bool playerLastZone;

    public bool playerLastTouched;

    // Start is called before the first frame update
    void Start()
    {
        // ut oh spagetti oh
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider zone){
        //Debug.Log(zone.gameObject.name);
        if(zone.gameObject.name == "floor_zone"){
            OutOfBoundsZone();
        }
        if(zone.gameObject.name == "player_table_zone"){
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
        if(playerLastZone){
            //Debug.Log("Player gets point");
            EndRound();
        }
        else{
            playerLastZone = true;
        }
    }
    public void TriggerOpponentZone(){
        if(!playerLastZone){
            //Debug.Log("Opponent gets point");
            EndRound();
        }
        else{
            playerLastZone = false;
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
        playerLastTouched = false;
        playerLastZone = false;
        debugScript.resetBall = true;
    }
}
