using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour
{

    public debug debugScript;

    public time timeScript;

    public opponent opponentScript;

    public bool playerLastZone;

    public bool playerLastTouched;

    public bool round_ended;

    public float doubleBounceBuffer;

    public float doubleBounceTime;

    public AudioSource[] bounceAudios;

    // Start is called before the first frame update
    void Start()
    {
        playerLastZone = true;
        playerLastTouched = true;
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
            opponentScript.hitCounter = 0;
            timeScript.state = new LinkedList<(Vector3, Vector3)>();
            timeScript.state.AddLast((debugScript.spawnPoint, Vector3.forward*debugScript.launchSpeed));
            round_ended = false;
        }
    }

    public void AudioPlayBounce() {

        int i = Random.Range(0, bounceAudios.Length-1);
        bounceAudios[i].Play();

    }
    void OnTriggerEnter(Collider zone){
        if(zone.gameObject.name == "floor_zone"){
            OutOfBoundsZone();
        }
        if(zone.gameObject.name == "player_table_zone"){
            AudioPlayBounce();
            TriggerPlayerZone();
        }
        if(zone.gameObject.name == "opponent_table_zone"){
            AudioPlayBounce();
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
            opponentScript.HandlePlayerLost();
            EndRound();
        }
        if (!playerLastZone) {
            playerLastZone = true;
            doubleBounceBuffer = doubleBounceTime;
        }
    }
    public void TriggerOpponentZone(){
        //Debug.Log("e2");
        if(!playerLastZone && doubleBounceBuffer <=0){
            //Debug.Log("e");
            opponentScript.HandleOpponentLost();
            EndRound();
        }
        if (playerLastZone) {
            playerLastZone = false;
            doubleBounceBuffer = doubleBounceTime;
        }
    }
    public void TriggerPlayerNetZone(){
        opponentScript.HandlePlayerLost();
        EndRound();
    }
    public void TriggerOpponentNetZone(){
        //Debug.Log("b");
        opponentScript.HandleOpponentLost();
        EndRound();
    }
    public void OutOfBoundsZone(){
        if(playerLastTouched == playerLastZone){
            if(playerLastTouched){
                opponentScript.HandlePlayerLost();
                EndRound();            
            }
            else{
                //Debug.Log("c");
                opponentScript.HandleOpponentLost();
            }
        }
        else{
            if(playerLastTouched){
                //Debug.Log("d");
                opponentScript.HandleOpponentLost();
            }
            else{
                opponentScript.HandlePlayerLost();
                EndRound();
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
