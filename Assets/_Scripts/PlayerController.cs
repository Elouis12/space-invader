using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody2D rb;
    public float speed = 8.0f;
    public Transform upperLeftLimit;
    public Transform lowerRightLimit;
    Animator animator;

    public GameObject laser; // Prefab laser to instantiate
    public Transform firePoint; // where/position we'll instantiate it (in front of the ship)
    

    public GameObject laserHitEffect; // laser particle effect prefab

    public static GameObject shield; // shield for the player
    
    public Transform shieldPoint; // where to put the shield

    public float shieldDelay = 1.0f;
    public float shieldTimer;

    public float shotDelay = 0.4f;
    float shotTimer;

    GameObject[] currentWaveEnemies; // to destroy random enemies via powerup

    int randomNumber;
    
    List<int> trackEnemiesDestroyed;

    // enemy related
    static int totalWaves;
    public GameObject[] waves;
    static int currentWave = 0;
    // GameObject[] enemiesInCurrentWave;
    List<GameObject> enemiesInCurrentWave;
    public Transform wavePoint; // where to start waves

    GameController gameController; // our game controller
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // rb.velocity = new Vector2(5.0f, 5.0f);
        animator = GetComponent<Animator>();

        shotTimer = shotDelay;

        gameController = FindObjectOfType<GameController>(); // gets first object of GameController type
        
        shield = GameObject.Find("player_shield");
    }

   
    IEnumerator Countdown(int seconds)
    {
        int count = seconds;
       
        while (count > 0) {
           
            // display something...
            yield return new WaitForSeconds(1);
            count --;
        }
       
        // count down is finished...
       disableShield();
    }
 
    void disableShield()
    {
        shield.GetComponent<SpriteRenderer>().enabled = false;

    }
 

    // Update is called once per frame
    void Update()
    {

        Vector2 moveInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        rb.velocity = moveInput * speed;

        transform.position = new Vector3(

            Mathf.Clamp(transform.position.x, upperLeftLimit.position.x, lowerRightLimit.position.x),
            Mathf.Clamp(transform.position.y, lowerRightLimit.position.y,upperLeftLimit.position.y),
            transform.position.z
        );

        // gives value to the animator so when user moves it displays the idle, left, and right animation(s)
        animator.SetFloat("PlayerMovement", Input.GetAxisRaw("Vertical"));

        // if button is pressed, instanitate a laser
        if( Input.GetButtonDown("Fire1") ){

                        // object , position, rotation
            Instantiate(laser, firePoint.position, firePoint.rotation);
        }

        // give the lasers a delay
        if( Input.GetButton("Fire1") ){

            shotTimer -= Time.deltaTime;
            if( shotTimer <= 0 ){

                Instantiate(laser, firePoint.position, firePoint.rotation);
                shotTimer = shotDelay;
            }

        }
   
    }

    private void OnTriggerEnter2D(Collider2D otherObject){


        shield = GameObject.FindGameObjectWithTag("Player").transform.GetChild(3).gameObject;


    if( !(gameController.getPlayerLives() <= 0 || gameController.getBossLives() <= 0 ) ){ // if game is no over, take or give damage

            // SHIP COLLIDES WITH ASTEROID OR ENEMY SHIP OR ENEMY LASER
        if( otherObject.tag == "Asteroid" ||
            otherObject.tag == "Enemy" ||
            otherObject.tag == "EnemyLaser" ){

            if( !shield.GetComponent<SpriteRenderer>().enabled ){ // IF PLAYER HAS NO SHIELD
                
                Instantiate(laserHitEffect, transform.position, transform.rotation);
                gameController.playerLoseLife(1);

            }
        }

        if( otherObject.tag == "BossLaserGreen" ){

            if( !shield.GetComponent<SpriteRenderer>().enabled ){ // IF PLAYER HAS NO SHIELD
                
                Instantiate(laserHitEffect, transform.position, transform.rotation);

                gameController.playerLoseLife(3);

            }
        }

        if( otherObject.tag == "BossLaserMedium" ){

            if( !shield.GetComponent<SpriteRenderer>().enabled ){ // IF PLAYER HAS NO SHIELD
                
                Instantiate(laserHitEffect, transform.position, transform.rotation);

                gameController.playerLoseLife(2);

            }
        }

        if( otherObject.tag == "BossLaserSmall" ){

            if( !shield.GetComponent<SpriteRenderer>().enabled ){ // IF PLAYER HAS NO SHIELD
                
                Instantiate(laserHitEffect, transform.position, transform.rotation);

                gameController.playerLoseLife(1);

            }
        }

        // PLAYER PICKS UP SHIELD
        if( otherObject.tag == "PowerupShield" ){ // if player gets a powerup shield, give shield

            Destroy(otherObject.gameObject); // destroy the powerup icon
            shield.GetComponent<SpriteRenderer>().enabled = true; // show the sprite (shield) around player

        }

        // SHIELD ON TIMER
        if( shield.GetComponent<SpriteRenderer>().enabled ){ // if shield is on, countdown befoe removing

            StartCoroutine(Countdown(gameController.playerShieldTimer)); // timer before shield gets remove

        }

        // PLAYR PICKS UP HEALTH
        if( otherObject.tag == "ExtraHealth" ){

            if( gameController.getPlayerLives() < 10 ){ // only increase health if health bar not full
                gameController.playerGainLife(1);
                Destroy(otherObject.gameObject);
            }   
            
        }

        }

        // TRACK LIVES
        playerDied();
        
    }


    public bool playerDied(){

        if( gameController.getPlayerLives() <= 0 ){

            Destroy(gameObject);
            // gameObject.GetComponent<SpriteRenderer>().enabled = false;
            return true;
        }

        return false;
    }

}
