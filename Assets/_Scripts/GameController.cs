using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int playerLives = 10;
    public int bossLives = 3;
    public Transform enemyShipStartBound; // when ship enters camera view, it can start with lasers
    public Transform bossBound;
    public Transform destroyBound;
    public Transform upperLeftLimit;
    public Transform lowerRightLimit;

    // enemy related
    // static int totalWaves;
    public GameObject[] waves;
    public int currentWave = 0;
    // GameObject[] enemiesInCurrentWave;
    public List<GameObject> enemiesInWave;
    public List<GameObject> asteroidsInWave;
    public Transform wavePoint; // where to start waves

    public List<GameObject> playerRedHearts = new List<GameObject>(20);
    public List<GameObject> playerBlackHearts = new List<GameObject>(20);

    public List<GameObject> bossRedHearts = new List<GameObject>(20);
    public List<GameObject> bossBlackHearts = new List<GameObject>(20);

    public GameObject redHeart;
    public GameObject blackHeart;

    public int playerShieldTimer;

    public GameObject logo; // logo for the player
    public GameObject gameOverLogo;
    public GameObject youWinLogo;

    public bool gameOver;
    // TextAlignment 
    // Start is called before the first frame update
    void Start()
    {    
        playerShieldTimer = 10;
        logo = GameObject.FindGameObjectWithTag("Logo").transform.gameObject;
        gameOverLogo = GameObject.FindGameObjectWithTag("GameOver").transform.gameObject;
        youWinLogo = GameObject.FindGameObjectWithTag("YouWin").transform.gameObject;
        StartCoroutine( Countdown(3) );

        // getNextWave(); // retireves all the waves to be used
        getEnemiesForCurrentWave( waves[currentWave] ); // get all of the enemies of that (1st) wave and put them into the array
        getAsteroidsForCurrentWave( waves[currentWave] );
        // instatiate the current wave
        Instantiate( waves[currentWave], wavePoint.position, wavePoint.rotation );

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
        disableLogo();
       
    }

    public bool gameIsOver(){

        if( ( getPlayerLives() <= 0 ? true : false ) ){
            enableGameOverLogo();
        }

        return ( getPlayerLives() <= 0 ? true : false ) ;
    }

    public void enableBossHealthBar(){

        if( currentWave == 2 ){

            for( int x = 0; x < ( bossRedHearts.Count ); x+=1 ){
                
                bossRedHearts[x].GetComponent<SpriteRenderer>().enabled = true;
                bossBlackHearts[x].GetComponent<SpriteRenderer>().enabled = true;
            }
        
        }
    }

    public void disableLogo(){


        logo.GetComponent<SpriteRenderer>().enabled = false;

    }

    public void disableGameOverLogo(){

        gameOverLogo.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void enableGameOverLogo(){

        gameOverLogo.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void enableYouWinLogo(){

        youWinLogo.GetComponent<SpriteRenderer>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

        playerWin(); // checks if player won
        gameIsOver(); // checks if game is over
        enableBossHealthBar();
        if( allEnemiesGone() && currentWave < waves.Length && !gameIsOver() ){ // enemies gone from current wave and there are more waves

            // for( int x = 0; x < asteroidsInWave.Count; x+=1 ){ // destroy all asteroids;

            //     Destroy( asteroidsInWave[x] );
            // }
            
            getNextWave(); // get next wave
        }
    }

    public void getNextWave(){

        enableBossHealthBar();
        getEnemiesForCurrentWave( waves[++currentWave] ); // get all of the enemies of that (1st) wave and put them into the array
        getAsteroidsForCurrentWave( waves[currentWave] );
        // enableBossHealthBar(); // if on the last wave, show boss health
        // instatiate the current wave
        Instantiate( waves[currentWave], wavePoint.position, wavePoint.rotation );
    }

    public void getEnemiesForCurrentWave(GameObject waveCurrentlyOn){ // puts all enemies in the current wave into an array

        int numberOfEnemies = waveCurrentlyOn.transform.GetChild(1).transform.childCount;

        enemiesInWave = new List<GameObject>();

        for( int x = 0; x < numberOfEnemies; x+=1 ){

            enemiesInWave.Add( waveCurrentlyOn.transform.GetChild(1).GetChild(x).gameObject );
        }
    }
    
    public void getAsteroidsForCurrentWave(GameObject waveCurrentlyOn){ // puts all enemies in the current wave into an array

        int numberOfAsteroids = waveCurrentlyOn.transform.GetChild(0).transform.childCount;

        asteroidsInWave = new List<GameObject>();

        for( int x = 0; x < numberOfAsteroids; x+=1 ){

            asteroidsInWave.Add( waveCurrentlyOn.transform.GetChild(0).GetChild(x).gameObject );
        }
    }

    public bool allEnemiesGone(){ // no remaining enemies on the scene, so the new wave can start

        return enemiesInWave.Count <= 0;
    }

    public void playerLoseLife(int decreaseBy){

        int tempLives = playerLives;
        playerLives -= decreaseBy;

        for( int x = 0; x < decreaseBy; x+=1 ){

            playerRedHearts[--tempLives].GetComponent<SpriteRenderer>().enabled = false;
            // Destroy(playerRedHearts[--tempLives]);
        }        
    }

    public void playerGainLife(int increaseBy){

        int tempLives = playerLives;
        playerLives += increaseBy;

        for( int x = 0; x < increaseBy; x+=1 ){

            // add heart into the game object hearts array
            playerRedHearts[tempLives].GetComponent<SpriteRenderer>().enabled = true;
            // Instantiate( redHeart, playerBlackHearts[tempLives].transform.position, playerBlackHearts[tempLives].transform.rotation  );
            tempLives++;
        }

    }

    public void bossLoseLife(int decreaseBy){

        int tempLives = bossLives;
        bossLives -= decreaseBy;

        for( int x = 0; x < decreaseBy; x+=1 ){

            // bossRedHearts[--tempLives].GetComponent<SpriteRenderer>().enabled = false;

            Destroy(bossRedHearts[--tempLives]);
        }
        
    }

    public bool playerWin(){

        if( getPlayerLives() > 0 && getBossLives() <= 0 ){

            enableYouWinLogo();
        }

        return getPlayerLives() > 0 && getBossLives() <= 0;
    }

    public int getBossLives(){

        return bossLives;
    }

    public int getPlayerLives(){

        return playerLives;
    }
}
