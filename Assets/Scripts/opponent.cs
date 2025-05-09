using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class opponent : MonoBehaviour
{

    public bool returnBall;
    public int hitCounter;
    public int lives;
    public int maxLives;
    public float missChance;
    public Vector3 returnVelocity;
    public GameObject ball; 

    public GameObject target;

    public GameObject oppHearts;
    public SpriteRenderer oppHeartsSR;
    public Sprite[] oppHeartsArray;

    public ball ballScript;

    public face faceScript;
    private Bot bot1;
    private Bot bot2;
    private Bot bot3;

    public Bot[] botArray = new Bot[3]; 

    public Bot currentBot;

    public oppPaddle oppPaddleScript;

    public TMP_Text stageText;

    public TMP_Text livesText;

    public AudioSource takeLifeAudio;
    public AudioSource nextStageAudio;
    public AudioSource dieAudio;
    public AudioSource winAudio;

    public Material aGlow;
    public light lightScript;

    // Start is called before the first frame update
    void Awake() {
        // level, grace, miss
        bot1 = new Bot(0,1,1f);
        bot2 = new Bot(1,1,0.25f);
        bot3 = new Bot(2,3,0.25f);
        botArray[0] = bot1;
        botArray[1] = bot2;
        botArray[2] = bot3;
        //Debug.Log(botArray[0].get_level());
        currentBot = bot1;
        lives = maxLives;
        oppHeartsSR  = oppHearts.GetComponent<SpriteRenderer>();
    }
    
    void Start()
    {   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {

        if (returnBall) {

            ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ball.GetComponent<Rigidbody>().AddForce(returnVelocity, ForceMode.VelocityChange);
            //Debug.Log(returnVelocity);
            returnBall = false;

        }
    }

    public void ReturnBall(GameObject ball) {

        hitCounter++;

        Rigidbody ballRb = ball.GetComponent<Rigidbody>(); 
        Vector3 targetPos = GetRandomTarget();
        float shotAngle = GetRandomShotAngle();
        ball.transform.position = GetRandomPosition();

        target.transform.position = targetPos;

        //Debug.Log("Launching ball at target " +targetPos+ " with shot angle " +shotAngle);
        Vector3 velocity = GetVelocityGivenAngle(ball.transform.position, targetPos, shotAngle);


        //ballRb.AddForce(velocity, ForceMode.VelocityChange);
        returnBall = true;
        returnVelocity = velocity;
        ballScript.playerLastTouched = false;

        ballScript.AudioPlayBounce();

        StartCoroutine(faceScript.HitBall());
        //Debug.Log("Changed velocity to " + velocity);//;+ " Actual Velocity " + ballRb.velocity);

    }

    public Vector3 GetRandomPosition() {
        return new Vector3(ball.transform.position.x, 1.4f, ball.transform.position.z);
    }

    private void StopBall(Rigidbody ballRb) {
        ballRb.velocity = Vector3.zero;
                
    }
    private Vector3 GetRandomTarget() {
        float backEdge = 2.80f;
        float leftEdge = -0.4f;
        float rightEdge = 0.2f;
        float frontEdge = 2.46f;
        return new Vector3(Random.Range(leftEdge, rightEdge), 1f, Random.Range(frontEdge, backEdge));
    }
    public float GetRandomShotAngle() {
        return Random.Range(40f, 41f);
    }
    public Vector3 GetVelocityGivenAngle(Vector3 currentPosition, Vector3 targetPosition, float verticalDegrees) {
        float radians = verticalDegrees * Mathf.Deg2Rad;
        Vector3 diff = targetPosition - currentPosition;

        float horDist = Mathf.Sqrt(diff.x * diff.x + diff.z * diff.z);
        
        float velocity = Mathf.Sqrt(
            9.81f * horDist * horDist /
            (2 * Mathf.Cos(radians) * Mathf.Cos(radians) * 
            (horDist * Mathf.Tan(radians) + -diff.y))
        );

        return new Vector3(
        diff.x / horDist * velocity * Mathf.Cos(radians),  // x
        velocity * Mathf.Sin(radians),                     // y
        diff.z / horDist * velocity * Mathf.Cos(radians)); // z
    }

    
    void OnTriggerEnter(Collider other) {

        if (other.name != "ball") {
            return;
        }   

        ball = other.gameObject;

        if(ballScript.playerLastZone){
            HandlePlayerLost();
        }
        //ReturnBall(ball);

        missChance = Random.Range(0f,1f);

        if (hitCounter <= currentBot.get_grace() || missChance > currentBot.get_miss()){
            oppPaddleScript.MovePaddle(ball.transform.position);
            ReturnBall(ball);
            
        }
        // else {
        //     HandleOpponentLost();
        // }
    }

    public void HandlePlayerLost(){
        currentBot = botArray[0];
        stageText.SetText("Stage "+currentBot.get_level());
        lives = maxLives;
        oppHeartsSR.sprite = oppHeartsArray[lives-1];
        //livesText.SetText("Lives "+lives);

        dieAudio.Play();
        aGlow.SetColor("_EmissionColor", Color.HSVToRGB(0f, 0f, 0f));
        aGlow.DisableKeyword("_EMISSION");
        aGlow.EnableKeyword("_EMISSION");
        lightScript.rainbow = false;
        //Debug.Log("You Lost Bozo");
        //Debug.Log(currentBot);
    }

    public void HandleOpponentLost() { 
        lives -= 1;
        //Debug.Log("I have been shot");
        //livesText.SetText("Lives "+lives);
        if (lives == 0) {
            if (currentBot.get_level()+1 != 3) {
                currentBot = botArray[currentBot.get_level()+1];
                hitCounter = 0;
                //Debug.Log("You have advanced to level "+currentBot.get_level());
                stageText.SetText("Stage "+currentBot.get_level());
                nextStageAudio.Play();

                // handle lights
                Debug.Log(currentBot.get_level());
                if (currentBot.get_level() == 1) {
                    lightScript.rainbow = false;
                    Debug.Log("light level 1");
                    aGlow.SetColor("_EmissionColor", Color.HSVToRGB(0f, 0f, 1f));
                    aGlow.DisableKeyword("_EMISSION");
                    aGlow.EnableKeyword("_EMISSION");

                }
                else if (currentBot.get_level() == 2) {
                    lightScript.rainbow = true;
                    Debug.Log("light level 2");

                    aGlow.SetColor("_EmissionColor", Color.HSVToRGB(0f, 0f, 1f));
                    aGlow.DisableKeyword("_EMISSION");
                    aGlow.EnableKeyword("_EMISSION");
                }

                lives = maxLives;

            }
            else {
                // winning is just losing in a different fashion
                winAudio.Play();
                HandlePlayerLost();
            }            
        }
        //livesText.SetText("Lives "+lives);

        oppHeartsSR.sprite = oppHeartsArray[lives-1];

        takeLifeAudio.Play();
    }
}

public class Bot{
    private int _level;
    private int _grace;
    private float _miss;
    
    public int get_level(){return _level;}
    public int get_grace(){return _grace;}
    public float get_miss(){return _miss;}

    public Bot (int level, int grace, float miss) {
        _level = level;
        _grace = grace; 
        _miss = miss;
    }
}