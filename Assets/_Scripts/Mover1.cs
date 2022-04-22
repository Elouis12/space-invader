using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover1 : MonoBehaviour
{

        public float horizontalSpeed = -2 /*Random.Range(1,11)*/;
        public float verticalSpeed = -2/*Random.Range(1,11)*/;

        // int clockWiseOrAntiClockWise = /*Random.Range(0,2);*/ // 0 = clockWise , 1 == anticlockwise
        // public float rotationSpeed = clockWiseOrAntiClockWise == 0 ? /*Random.Range(2,10)*/ : ( -1 * /*Random.Range(2,10)*/ );
        public float rotationSpeed = 30;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position = new Vector3(
            transform.position.x + (horizontalSpeed * Time.deltaTime),
            transform.position.y + (verticalSpeed * Time.deltaTime),
            transform.position.z
        );

        transform.rotation = Quaternion.Euler(

            0f, 0f, transform.rotation.eulerAngles.z + (rotationSpeed * Time.deltaTime)

        );
    }

    void OnBecameInvisible(){

        Destroy(gameObject);
    }
}
