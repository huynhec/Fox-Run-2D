using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{
    public float pointsPerSecond;
    public Text scoreText;
    public Text yourScoreText;

    public float score;
    private float yourscore;

    public bool isScoreIncreasing;
    void Start()
    {   
        
        isScoreIncreasing = true;

        if(PlayerPrefs.HasKey("YourScore"))
            yourscore = PlayerPrefs.GetFloat("YourScore");
    }

    void Update()
    {
        if(isScoreIncreasing)
            score += pointsPerSecond * Time.deltaTime;

        if(score > yourscore){
            yourscore = score;
            PlayerPrefs.SetFloat("YourScore", yourscore);
        }

        scoreText.text = Mathf.Round(score).ToString();
        yourScoreText.text = Mathf.Round(yourscore).ToString();
    }
}
