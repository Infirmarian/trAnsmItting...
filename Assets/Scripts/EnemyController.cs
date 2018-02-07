using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyController : MonoBehaviour {

    public int maxHealth;
    public float minAmp;
    public float maxAmp;
    public float minFrequency;
    public float maxFrequency;
    public float smallFrequecy;
    public float smallAmp;
    public float minSpeed;
    public float maxSpeed;
    public float hitMoveAmount;
    public int scoreAmt;

    private float amplitude;
    private float frequency;
    private float speed;
    private float t;
    private Vector3 centre;
    private int health;
    private Rigidbody2D rb;
    private GameController gameController;

    // Use this for initialization
	void Start () {
        t = 0;
        rb = GetComponent<Rigidbody2D>();
        amplitude = UnityEngine.Random.Range(minAmp,maxAmp);
        frequency = UnityEngine.Random.Range(minFrequency,maxFrequency);
        speed = UnityEngine.Random.Range(minSpeed, maxSpeed);
        centre = this.transform.position;
        health = maxHealth;
        try
        {
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }catch(Exception e){
            
        }
	}
	
	// Update is called once per frame
	void Update () {
        rb.velocity = new Vector2(amplitude * Mathf.Sin(t * frequency) + 
            smallAmp * Mathf.Sin(t * smallFrequecy),
             -speed);
        t += Time.deltaTime;
        if (health <= 0)
            DestroyEnemy();

	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(("Collided with something"));
        if (collision.gameObject.tag == "One")
        {
            //health -= collision.gameObject.GetComponent<BulletScript>().ReturnDamage();
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
    private void DestroyEnemy()
    {
        gameController.AddScore(scoreAmt);
        //Debug.Log("Enemy Destroyed");
        Destroy(this.gameObject);
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
