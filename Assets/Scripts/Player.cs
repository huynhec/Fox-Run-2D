using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Player player;
    public float jump;
    public float speed; 
    public LayerMask ground;
    public LayerMask deathGround;
    private Rigidbody2D rigidBody;
    private Collider2D playerCollider;
    private Animator animator;
    public AudioSource deathSound;
    public AudioSource jumpSound;
    public float mileStone;
    public float mileStoneCount;
    public float speedMultipier;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        mileStoneCount = mileStone;
    }

    // Update is called once per frame
    void Update()
    {
        bool dead = Physics2D.IsTouchingLayers(playerCollider, deathGround);
        if (dead){
            gameManager.GameOver();
        }
        

        if(transform.position.x > mileStoneCount){
            mileStoneCount += mileStone;
            speed = speed * speedMultipier;
            mileStone += mileStone * speedMultipier;
            Debug.Log("M"+mileStone+", "+", MC"+mileStoneCount+", MS"+speed);

        }
        rigidBody.velocity = new Vector2(speed, rigidBody.velocity.y);

        bool grounded = Physics2D.IsTouchingLayers(playerCollider, ground);
  
        if(Input.GetMouseButton(0)  || Input.GetKey(KeyCode.Space)){
            if(grounded){
                jumpSound.Play();
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, jump);
            }
        }

        animator.SetBool("Grounded", grounded);
    }

     private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.tag == "Eagle"){
        Die();
       gameManager.GameOver();
        } 
    }
    private void Die(){
        animator.SetTrigger("Enemyed");
      
    }
    public void Again(){
                mileStoneCount = 0;
        gameManager.Restart();
    }
}
