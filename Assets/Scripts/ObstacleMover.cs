using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMover : MonoBehaviour {

    public float speed;
    public int maxHealth;
    public int hitMoveAmount;
    public int scoreAmt;
    private int health;
    private Rigidbody2D rb;
    private GameController gameController;


	// Use this for initialization
	void Start () {
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0,-speed);
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }
	
	// Update is called once per frame
	void Update () {
        //move each item down at a constant rate
        if (health <= 0)
            DestroyObstacle();
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(("Collided with something"));
        if (collision.gameObject.tag == "One")
        {
            Destroy(collision.gameObject);
        }
        // if zero hits, it is destroyed without affecting the obstacle
        if (collision.gameObject.tag == "Zero")
            Destroy(collision.gameObject);
        if (collision.gameObject.tag == "Player")
        {
            
            collision.gameObject.GetComponent<PlayerController>().TakeDamage();
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "PlayerCopy")
        {
            collision.gameObject.GetComponent<PlayerCopy>().TakeDamage();
        }
    }
    private void DestroyObstacle() {
        gameController.AddScore(scoreAmt);
        Destroy(this.gameObject);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
