using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float start,end;
    private bool isRight;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    

    // Update is called once per frame
    void Update()
    {
        var positionEnemy = transform.position.x;
        //focusPlayer
        if(player !=null){
        var positionPlayer = player.transform.position.x; 
        if(positionPlayer > start && positionPlayer<end){
            if(positionPlayer < positionEnemy) isRight = false;
            if (positionPlayer > positionEnemy) isRight = true;
           
        }
        }

        if(positionEnemy < start){
            isRight = true;
        }
        if(positionEnemy > end){
            isRight = false;
        }
        Vector2 scale = transform.localScale;
        if(isRight){
            scale.x=-3;
            transform.Translate(Vector3.right*2f*Time.deltaTime);
        }else{
            scale.x=3;
            transform.Translate(Vector3.left*2f*Time.deltaTime);
        }
        transform.localScale = scale;
    }
    public void SetStart (float start)
    {
        this.start = start;
    }
    public void SetEnd(float end)
    {
        this.end=end;
    }
    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }
}