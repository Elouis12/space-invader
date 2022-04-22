using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Mover2 : MonoBehaviour
{

        public float horizontalSpeed;
        public float verticalSpeed;
        public float rotationSpeed;

        public enum HorizontalDirection { Left, Right, None };
        public enum VerticalDirection { Up, Down, None };
        public enum RotationDirection { Clockwise, Anticlockwise, None };
        
        [System.Serializable] // allows it to show in the inspector
        public struct HorizontalMovement{

            public float speed;
            public HorizontalDirection direction;
        } 

        [System.Serializable]
         public struct VerticalMovement{

            public float speed;
            public VerticalDirection direction;
        } 

        [System.Serializable]
        public struct RotationMovement{

            public float speed;
            public RotationDirection direction;
        } 
        // [System.Serializable]



    // Start is called before the first frame update
    void Start()
    {
        
    }
    public HorizontalMovement horizontalMovement;
    public VerticalMovement verticalMovement;
    public RotationMovement rotationMovement;

    GameController gameController;
    Transform enemyShipStartBound; // make sure enemy doesn't start moving before the bound

    // Update is called once per frame
    void Update()
    {

        gameController = FindObjectOfType<GameController>(); // gets first object of GameController type

        horizontalSpeed = horizontalMovement.speed;
        verticalSpeed = verticalMovement.speed;
        rotationSpeed = rotationMovement.speed;


        if( horizontalMovement.direction == HorizontalDirection.Left ){
            horizontalSpeed = -horizontalSpeed;
        }

        if( verticalMovement.direction == VerticalDirection.Down ){
            verticalSpeed = -verticalSpeed;
        }

        if( rotationMovement.direction == RotationDirection.Clockwise ){
            rotationSpeed = -rotationSpeed;
        }

        enemyShipStartBound = gameController.enemyShipStartBound;

        if( gameController.logo.GetComponent<SpriteRenderer>().enabled != false ){ // logo is still on the screen

            transform.position = new Vector3(
                transform.position.x + (0.0f * Time.deltaTime),
                transform.position.y + (0.0f * Time.deltaTime),
                transform.position.z
            );

        }else if( enemyShipStartBound.position.x >= transform.position.x ){

            transform.position = new Vector3(
                transform.position.x + (horizontalSpeed * Time.deltaTime),
                transform.position.y + (verticalSpeed * Time.deltaTime),
                transform.position.z
            );

            transform.rotation = Quaternion.Euler(

                0f, 0f, transform.rotation.eulerAngles.z + (rotationSpeed * Time.deltaTime)

            );

        }else{ // go straight until reach bounds

            transform.position = new Vector3(
                transform.position.x + (-1f * Time.deltaTime),
                transform.position.y + (0.0f * Time.deltaTime),
                transform.position.z
            );
        }
        

    }

    void OnBecameInvisible(){

        Destroy(gameObject);
    }

    private void OnValidate(){

        horizontalMovement.speed = Mathf.Clamp(horizontalMovement.speed, 0f, 15f);
        verticalMovement.speed = Mathf.Clamp(verticalMovement.speed, 0f, 15f);
        rotationMovement.speed = Mathf.Clamp(rotationMovement.speed, 0f, 200f);

        if(horizontalMovement.direction == HorizontalDirection.None){

            horizontalMovement.speed = 0;
        }

        if(verticalMovement.direction == VerticalDirection.None){

            verticalMovement.speed = 0;
        }

        if(rotationMovement.direction == RotationDirection.None){

            rotationMovement.speed = 0;
        }
    }
}
