using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody2D rb;
    public float horizontalSpeed = 120.0f;
    public float verticalSpeed = 8.0f;
    // Animator animator;

    public GameObject greenLaser; // Prefab laser to instantiate
    public GameObject mediumLaser;
    public GameObject smallLaser;

    public Transform greenLaserFirePoint;
    public Transform mediumLaserFirePoint1;
    public Transform mediumLaserFirePoint2;
    public Transform smallLaserFirePoint1;
    public Transform smallLaserFirePoint2;

    public bool gameOver = false;

    public GameObject laserHitEffect; // laser particle effect prefab

    // public Transform enenmyShipStartBound; // when ship enters camera view, it can start with lasers

    public float greenLaserDelay = 0.4f;
    float greenLaserTimer;

    public float mediumLaserDelay = 0.3f;
    float mediumLaserTimer;

    public float smallLaserDelay = 0.2f;
    float smallLaserTimer;

    GameController gameController;
    GameObject mover2;

    Transform enemyShipStartBound; // make sure enemy doesn't start with lasers before the bound

    Transform bossBound; // so the boss can stop there
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // rb.velocity = new Vector2(5.0f, 5.0f);
        // animator = GetComponent<Animator>();

        greenLaserTimer = greenLaserDelay;
        mediumLaserTimer = mediumLaserDelay;
        smallLaserTimer = smallLaserDelay;

        gameController = FindObjectOfType<GameController>();

        mover2 = GameObject.FindGameObjectWithTag("Boss");

    }

    // Update is called once per frame
    void Update()
    {

        enemyShipStartBound = gameController.enemyShipStartBound;
        bossBound = gameController.bossBound;

        if( !(gameController.getPlayerLives() <= 0 || gameController.getBossLives() <= 0 ) ){ // keep sending lasers if game not over

            
        transform.position = new Vector3(

            transform.position.x ,
            transform.position.y + verticalSpeed * Time.deltaTime,
            transform.position.z
        );
        // moves the boss up and down
        if( gameController.upperLeftLimit.position.y <= transform.position.y ){
            
            // Debug.Log("went up");
            verticalSpeed = -verticalSpeed;

        }else if( gameController.lowerRightLimit.position.y >= transform.position.y ){
            // Debug.Log("went down");
            verticalSpeed = -verticalSpeed;
        }
            greenLaserTimer -= Time.deltaTime;
            if( greenLaserTimer <= 0 && enemyShipStartBound.position.x >= transform.position.x ){
                // enemy only starts with laser when enters the scene, not before
                Instantiate(greenLaser, greenLaserFirePoint.position, greenLaserFirePoint.rotation);
                greenLaserTimer = greenLaserDelay;
            }

            mediumLaserTimer -= Time.deltaTime;
            if( mediumLaserTimer <= 0 && enemyShipStartBound.position.x >= transform.position.x ){
                // enemy only starts with laser when enters the scene, not before

                Instantiate(mediumLaser, mediumLaserFirePoint1.position, mediumLaserFirePoint1.rotation);
                Instantiate(mediumLaser, mediumLaserFirePoint2.position, mediumLaserFirePoint2.rotation);
                mediumLaserTimer = mediumLaserDelay;
            }

            smallLaserTimer -= Time.deltaTime;
            if( smallLaserTimer <= 0 && enemyShipStartBound.position.x >= transform.position.x ){
                // enemy only starts with laser when enters the scene, not before

                Instantiate(smallLaser, smallLaserFirePoint1.position, smallLaserFirePoint1.rotation);
                Instantiate(smallLaser, smallLaserFirePoint2.position, smallLaserFirePoint2.rotation);
                smallLaserTimer = smallLaserDelay;
            }

        }

        // if reached at players right bottom bound ... stay there            
        if( bossBound.position.x >= transform.position.x ){
            
            gameObject.GetComponent<Mover2>().horizontalMovement.speed = 0.0f;
            gameObject.GetComponent<Mover2>().verticalMovement.speed = 0;
            gameObject.GetComponent<Mover2>().rotationMovement.speed = 0;

        }
        
    }

    

    private void OnTriggerEnter2D(Collider2D otherObject){


    }

    private void OnBecameInvisible(){

        // destroy the game objects (lasers) once not visible, and outside of camera view

        Destroy(gameObject);
    }

    private void OnBecameVisible(){

        // shotTimer -= Time.deltaTime;
        // if( shotTimer <= 0 /*&& enenmyShipStartBound.position.x <= transform.position.x*/ ){

        //     Instantiate(laser, firePoint.position, firePoint.rotation);
        //     shotTimer = shotDelay;
        // }

        // Instantiate(laser, firePoint.position, firePoint.rotation);
    }
}
