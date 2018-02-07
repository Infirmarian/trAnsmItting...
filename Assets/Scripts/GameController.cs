using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//TODO: Get/create artwork for player, obstacles, background, etc
//TODO: Create bosses
//TODO: Test on iOS/Android
//TODO: Create death scenario/win condition
//TODO: Menus
//TODO: UI Elements (score, pause, etc)
//TODO: Player duplication


public class GameController : MonoBehaviour {

    public Transform spawnLocation;
    public Transform bossSpawnLoc;
    public GameObject obstacle;
    public GameObject enemy;
    public GameObject duplicatePickUp;
    public GameObject playerPrefab;
    public GameObject firewall;
    public GameObject heart;
    public GameObject playerCopy;
    //public Canvas canvas;
     

    public int winScore;
    public int beforeBossScore;
    public int scoreFactor;
    public int bossScoreFactor;
    public float minEnemySpawnTime;
    public float maxEnemySpawnTime;
    public float minPickupSpawnTime;
    public float maxPickupSpawnTime;
    public float spawnLocWidthFactor;
    public float enemySpawnMaxX;
    public float minDupSpawnLoc;
    public bool isGameOver;
    public float rightLocHeart = 7.5f;
    public float leftLocHeart;
    public float heartGap = 1.2f;
    public float heartY = -2;
    public Text scoreText, gameOverText, hiScoreText, finalScoreText;
    public Button restartButton, resumeButton, quitButton;

    public static int difficulty = 1;



    private int score;
    private int bossFlag;
    private float countEnemyTime;
    private float countPickupTime;
    private int numPlayers;
    private float enemySpawnTime;
    private float pickupSpawnTime;
    private int leftHeartFlag;
    private int rightHeartFlag;    



    private PlayerController playerController;
    private Camera cam;

    // Use this for initialization
	void Start () {

        Time.timeScale = 1f;
        scoreText.text = "Score: 0";
        gameOverText.enabled = false;
        hiScoreText.enabled = false;
        finalScoreText.enabled = false;

        restartButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);

        isGameOver = false;

        
        isGameOver = false;
        //initializes the HighScore:# key pair if it doesn't exist
        

        if (!PlayerPrefs.HasKey("HighScore"))
            PlayerPrefs.SetInt("HighScore", 0);

        cam = Camera.main;
        numPlayers = 0;
        countEnemyTime = 0;
        countPickupTime = 0;
        score = 0;

        enemySpawnTime = minEnemySpawnTime + Random.Range(0.0f, maxEnemySpawnTime - minEnemySpawnTime);
        pickupSpawnTime = minPickupSpawnTime + Random.Range(0.0f, maxPickupSpawnTime - minPickupSpawnTime);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
            //playerController.setHealth();
            numPlayers++;
            Debug.Log("Found player controller and" + numPlayers);
        }
        else
            Debug.Log("Did not find player controller !");
        
        playerController.printHearts();
        StartCoroutine(AddScore());

	}
	
	// Update is called once per frame
	void Update () {
        if (score >= beforeBossScore && bossFlag == 0)
        {
            bossFlag = 1;
            Instantiate(firewall, bossSpawnLoc.position,Quaternion.identity);
        }
            
        if (countEnemyTime >= enemySpawnTime && bossFlag == 0)
        {
            Vector2 pos = new Vector2(Random.Range(-1.0f, 1.0f) * spawnLocWidthFactor, spawnLocation.position.y);
            int en = Random.Range(0,2);
            if (en == 0)
                Instantiate(obstacle, pos, Quaternion.identity);
            else
            {
                if (pos.x > enemySpawnMaxX)
                    pos.x = enemySpawnMaxX;
                if(pos.x < -enemySpawnMaxX)
                    pos.x = -enemySpawnMaxX;
                Instantiate(enemy, pos, Quaternion.identity);
            }
            enemySpawnTime = minEnemySpawnTime + Random.Range(0.0f, maxEnemySpawnTime - minEnemySpawnTime);
            countEnemyTime = 0;
        }
        if (countPickupTime >= pickupSpawnTime)
        {
            Vector2 pos = new Vector2(Random.Range(-1.0f, 1.0f) * spawnLocWidthFactor, spawnLocation.position.y);
            if (pos.x >= 0 && pos.x <= minDupSpawnLoc)
                pos.x += minDupSpawnLoc;
            else if(pos.x < 0 && pos.x >= minDupSpawnLoc)
                pos.x -= minDupSpawnLoc;
            Instantiate(duplicatePickUp, pos, Quaternion.identity);
            pickupSpawnTime = minPickupSpawnTime + Random.Range(0.0f, maxPickupSpawnTime - minPickupSpawnTime);
            countPickupTime = 0;
        }
        countEnemyTime += Time.deltaTime;
        countPickupTime += Time.deltaTime;

        if (score > winScore)
            GameOver(true);



        //Pause the game
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 pos = Input.mousePosition;
            if (cam.ScreenToWorldPoint(pos).x < -7 && cam.ScreenToWorldPoint(pos).y > 6)
                Pause();
        }

        scoreText.text = "Score: " + score;

	}

    IEnumerator AddScore(){
        while(!isGameOver){
            AddScore(scoreFactor);
            yield return new WaitForSeconds(1);
        }
    }


        public void Pause()
    {
        Debug.Log("Pressed pause");
        if (Time.timeScale < 0.1f)
        {
            if (!isGameOver)
            {
                restartButton.gameObject.SetActive(false);
                resumeButton.gameObject.SetActive(false);
                quitButton.gameObject.SetActive(false);
            }
            Time.timeScale = 1f;
        }
        else
        {
            if (!isGameOver)
            {
                restartButton.gameObject.SetActive(true);
                resumeButton.gameObject.SetActive(true);
                quitButton.gameObject.SetActive(true);
            }
            Time.timeScale = 0f;
        }
    }

    public void AddScore(int hitScore)
    {
        score += hitScore;
    }


    public void GameOver(bool win)
    {
        isGameOver = true;

        //enable restart and quit buttons
        restartButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);

        gameOverText.enabled = true;

        finalScoreText.text = "Score: " + score;
        finalScoreText.enabled = true;
        Time.timeScale = 0f;


        if (win)
            Debug.Log("The player won");
        else
            Debug.Log("The player lost");

        //canvas.enabled = true;

        if (score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", score);

        }

        hiScoreText.text = "Hi Score: " + PlayerPrefs.GetInt("HighScore");
        hiScoreText.enabled = true;

        //Destroy(playerController[1].gameObject);
        //numPlayers = 1;
    }



    public void KillPlayer()
    {
        if (numPlayers == 1)
        {
            GameOver(false);
            return;
        }
        GameObject player2 = GameObject.FindGameObjectWithTag("PlayerCopy");
        Destroy(player2);
        numPlayers = 1;
                
    }

    public void KilledBoss()
    {
        bossFlag = 0;
        AddScore(bossScoreFactor);
        beforeBossScore += score;        
    }
    public bool AddPlayer()
    {
        Debug.Log("started addplayer and " + numPlayers);
        if (numPlayers == 2)
            return false;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector2 pos1 = player.transform.position;
            PlayerController p = player.GetComponent<PlayerController>();
            
            Vector2 screenpos = cam.WorldToScreenPoint(pos1);
            float width = cam.pixelWidth;
            Vector2 newPos = cam.ScreenToWorldPoint(new Vector2(width - screenpos.x, screenpos.y));
            GameObject player2 = Instantiate(playerCopy, newPos, Quaternion.identity);
                        
            numPlayers++;
            Debug.Log("ended addplayer and " + numPlayers);
            return true;
        }
        else
        {
            Debug.Log("Did not find player controller !");
            return false;
        }
    }


    public void DealWithHearts(){
        
    }


    public void Quit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("menu");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
