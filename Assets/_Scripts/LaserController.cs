 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public float speed = 10.0f;
    
    public Transform enenmyShipStartBound;
    public static GameObject shield; // shield for the player


    public GameObject laserHitEffect; // laser particle effect prefab

    GameController gameController;
    Transform enemyShipStartBound;

    void Start()
    {
        gameController = FindObjectOfType<GameController>(); // gets first object of GameController type

    }

    // Update is called once per frame
    void Update()
    {
        // gets the laser going
        transform.position = new Vector3(
            transform.position.x + speed * Time.deltaTime,
            transform.position.y,
            transform.position.z
        );
    }

    // when you enter into a collision that has 'isTriggered'
    private void OnTriggerEnter2D(Collider2D otherObject){

        // otherObject would be the asteroid
        // gameObject = the object the script is attached to (this case the laser)
        // transform = the transform of the object the script is attached to (this case the laser's transform)
                  // (particle effect)prefab, position where laser had hit, rotation of the laser
        

        shield = GameObject.FindGameObjectWithTag("Player").transform.GetChild(3).gameObject;

        enemyShipStartBound = gameController.enemyShipStartBound; // bound transform that's on camera object

        if( otherObject.tag == "Asteroid" || otherObject.tag == "Enemy" ){ // if laser hits an asteroid or enemy

            if( enemyShipStartBound.position.x >= transform.position.x){ // if ship is not outside of the startbounds for the shit, don't destroy them until in view

                // remove that enemy object from the list of enemies
                for(  int x = 0; x < gameController.enemiesInWave.Count; x+=1 ){
                    
                    if( otherObject.name == gameController.enemiesInWave[x].name ){
                        gameController.enemiesInWave.RemoveAt(x);
                    }
                }

                Instantiate(laserHitEffect, transform.position, transform.rotation);
                Destroy(gameObject); // destroy laser
                Destroy(otherObject.gameObject); // destroy asteroid or enemy 
            }
        }

        if( otherObject.tag == "Boss" ){
            
            if( enemyShipStartBound.position.x >= transform.position.x){ // if ship is not outside of the startbounds for the shit, don't destroy them until in view

                Instantiate(laserHitEffect, transform.position, transform.rotation);
                Destroy(gameObject); // destroy laser
                gameController.bossLoseLife(1); // decrease boss health
            }
        }

    }

    private void OnBecameInvisible(){

        // destroy the game objects (lasers) once not visible, and outside of camera view
        Destroy(gameObject);
    }
}
