using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{

    public Player player;
    public ScoreManager scoreManager;
    public AudioSource backgroundSound;
    public AudioSource deathSound;

    private Vector3 playerStaringPoint;
    private Vector3 groundGenerationStartPoint;

    public GroundGenerator groundGenerator;
    public GameObject largeGround;
    public GameObject mediumGround;
    public GameObject gameOverScreen;
    public float mileStone;
    private float mileStoneCount;

    void Start()
    {
        playerStaringPoint = player.transform.position;
        groundGenerationStartPoint = groundGenerator.transform.position;
        gameOverScreen.SetActive(false);
        
    }



    public void GameOver(){
        player.gameObject.SetActive(false);
        gameOverScreen.SetActive(true);
        scoreManager.isScoreIncreasing = false; 
        backgroundSound.Stop();
        deathSound.Play();


    }

    public void Quit(){
        Application.Quit();
    }

    public void Restart(){
       
        SceneManager.LoadScene(SceneManager.GetActiveScene().name );
    }
        // GroundDestroyed[] destroyer = FindObjectsOfType<GroundDestroyed>();
        // for (int i = 0; i<destroyer.Length; i++){
        //     destroyer[i].gameObject.SetActive(false);
        // }
        // largeGround.SetActive(true);
        // mediumGround.SetActive(true);
        // player.transform.position = playerStaringPoint;
        // groundGenerator.transform.position = groundGenerationStartPoint;
        // gameOverScreen.SetActive(false);
        // player.gameObject.SetActive(true);
        // backgroundSound.Play();
        // scoreManager.score = 0;
        // scoreManager.isScoreIncreasing = true;
    //}

}
