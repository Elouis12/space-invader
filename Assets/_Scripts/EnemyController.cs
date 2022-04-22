using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody2D rb;
    public float speed = 8.0f;
    // Animator animator;

    public GameObject laser; // Prefab laser to instantiate
    public Transform firePoint; // where/position we'll instantiate it (in front of the ship)
    
    public bool gameOver = false;

    public GameObject laserHitEffect; // laser particle effect prefab

    // public Transform enenmyShipStartBound; // when ship enters camera view, it can start with lasers

    public float shotDelay = 2f;
    float shotTimer;

    GameController gameController;

    Transform enemyShipStartBound; // make sure enemy doesn't start with lasers before the bound

    Transform destroyBound; // destroys the enemy objects when past

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // rb.velocity = new Vector2(5.0f, 5.0f);
        // animator = GetComponent<Animator>();

        shotTimer = shotDelay;

        gameController = FindObjectOfType<GameController>();

    }

    // Update is called once per frame
    void Update()
    {
        gameOver = gameController.getPlayerLives() <= 0 ? true : false;
        enemyShipStartBound = gameController.enemyShipStartBound;

        if( !(gameController.getPlayerLives() <= 0 || gameController.getBossLives() <= 0)/*!gameOver || !gameController.playerWin()*/ ){ // keep sending lasers if game not over

            shotTimer -= Time.deltaTime;
            if( shotTimer <= 0 && enemyShipStartBound.position.x >= transform.position.x ){
                // enemy only starts with laser when enters the scene, not before
                Instantiate(laser, firePoint.position, firePoint.rotation);
                shotTimer = shotDelay;
            }

            if( gameController.destroyBound.position.x >= transform.position.x ){ // when enemy or asteroid passes the destroy bound, destroy it

                // remove that enemy object from the list of enemies when destroyed
                for(  int x = 0; x < gameController.enemiesInWave.Count; x+=1 ){
                    
                    if( gameObject.name == gameController.enemiesInWave[x].name ){
                        gameController.enemiesInWave.RemoveAt(x);
                    }
                }
                Destroy(gameObject);
            }
        }
        

        // OnBecameVisible();
        // gives value to the animator so when user moves it displays the idle, left, and right animation(s)
        // animator.SetFloat("PlayerMovement", Input.GetAxisRaw("Vertical"));
   
    }

    private void OnTriggerEnter2D(Collider2D otherObject){


    }

    private void OnBecameInvisible(){

        // destroy the game objects (lasers) once not visible, and outside of camera view

        // remove that enemy object from the list of enemies
        for(  int x = 0; x < gameController.enemiesInWave.Count; x+=1 ){
            
            if( gameObject.name == gameController.enemiesInWave[x].name ){
                gameController.enemiesInWave.RemoveAt(x);
            }
        }
        Destroy(gameObject);
    }

    private void OnBecameVisible(){

        shotTimer -= Time.deltaTime;
        if( shotTimer <= 0 /*&& enenmyShipStartBound.position.x <= transform.position.x*/ ){

            Instantiate(laser, firePoint.position, firePoint.rotation);
            shotTimer = shotDelay;
        }

        // Instantiate(laser, firePoint.position, firePoint.rotation);
    }
}
