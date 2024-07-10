using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemPicker : MonoBehaviour
{
    private AudioSource gemPickSound;
    private float gemPoints = 15f;
    private ScoreManager scoreManager;
    void Start()
    {
        gemPickSound = GameObject.Find("GemSound").GetComponent<AudioSource>();
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.name =="Player"){
            gameObject.SetActive(false);

            if(gemPickSound.isPlaying){
                gemPickSound.Stop();
            }
            gemPickSound.Play();
            // tang diem gem
            scoreManager.score += gemPoints;
        }
    }
}
