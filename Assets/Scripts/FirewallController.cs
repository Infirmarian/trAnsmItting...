using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirewallController : MonoBehaviour {

    public GameObject fireball;
    public Transform mouth;

    public int initialHealth;
    public float bounceSpeed = 5;
    public int phaseTime = 20;
    public int betweenPhaseTime = 5;
    public float fireBallSpeed = 5;
    public float fireLag = 2;
    public float amp = 2;
    public float freq = 10;
    public float smallVal = 0.1f;
    public int numShotsPhase = 10;

    private int health;
    private int phase;
    private int moveDir;
    private Rigidbody2D rb;
    private Animator animator;
    private float fireTime;
    private float t;
    //private float t2;
    private int phaseFlag;
    private int phaseStartFlag;
    private int movedFlag;
    private float angle;

    private GameController gameController;
    // Use this for initialization
	void Start () {
        t = 0;
        phaseFlag = 0;
        phaseStartFlag = 0;
        movedFlag = 0;
        fireTime = 0;
        health = initialHealth;
        moveDir = 1;
        //t2 = 0;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        phase = 1;
        angle = 0;

        GameObject game = GameObject.FindGameObjectWithTag("GameController");
        gameController = game.GetComponent<GameController>();
	}

    private void Update()
    {
        if (health <= 0)
        {
            GameController.difficulty++;
            Die();
        }
        else
        {
            if (phase != GameController.difficulty && GameController.difficulty <= 5 && t >= phaseTime)
                incrementPhase();
            Fire(GameController.difficulty);
        }
        
        //fireTime += Time.deltaTime;
    }

    private void Fire(int diff){
        if ((phase == 1 && diff > 5)|| diff == 1)
        {
            //t2 += Time.deltaTime;
            if (phaseFlag == 1)
            {
                t = 0;
                phaseFlag = 0;
            }
            //moveToCentre();
            bounceHor();
            if (fireTime >= fireLag)
            {
                shootPhaseOne();
                fireTime = 0;
            }
        }
        else if ((phase == 2 && diff > 5) || diff == 2)
        {
            //t2 += Time.deltaTime;
            if (phaseFlag == 1)
            {
                t = 0;
                phaseFlag = 0;
            }
            //moveToCentre();
            sineWave();
            if (fireTime >= fireLag)
            {
                shootPhaseTwo();
                fireTime = 0;
            }
        }
        else if ((phase == 3 && diff > 5) || diff == 3)
        {
            //t2 += Time.deltaTime;
            if (phaseFlag == 1)
            {
                t = 0;
                phaseFlag = 0;
            }
            moveToCentre();
            if (fireTime >= fireLag && movedFlag == 1)
            {
                angle += Mathf.PI / numShotsPhase;
                shootPhaseThree(angle);
                fireTime = 0;
            }
            if (t >= phaseTime)
                incrementPhase();
        }
        else if ((phase == 4 && diff > 5) || diff == 4)
        {
            //t2 += Time.deltaTime;
            if (phaseFlag == 1)
            {
                t = 0;
                phaseFlag = 0;
            }
            moveToCentre();
            if (fireTime >= fireLag && movedFlag == 1)
            {
                shootPhaseFour();
                fireTime = 0;
            }
            if (t >= phaseTime)
                incrementPhase();
        }
        else if ((phase == 5 && diff > 5) || diff == 5)
        {
            //t2 += Time.deltaTime;
            if (phaseFlag == 1)
            {
                t = 0;
                phaseFlag = 0;
                angle = -Mathf.PI / 2;
            }
            moveToCentre();
            if (fireTime >= fireLag && movedFlag == 1)
            {
                shootPhasefive(angle);
                fireTime = 0;
                angle += Mathf.PI / numShotsPhase;
            }
            if (t >= phaseTime && angle >= Mathf.PI / 2)
                incrementPhase();
        }
    }





    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            if (moveDir == 1)
                moveDir = -1;
            else
                moveDir = 1;
            incrementPhase();
        }
        
    }

    public void DealDamage(int d){
        health -= d;
    }

    private void incrementPhase()
    {
        if (t >= phaseTime && phase == 5)
        {
            phaseFlag = 1;
            phase = Random.Range(1,6);
            //t2 = 0;
            Debug.Log(phase);
        }
        else if (t >= phaseTime)
        {
            phase++;
            phaseFlag = 1;            
            //t2 = 0;
        }
        movedFlag = 0;
        
    }
    private void Die(){
        //TODO: Animation of death
        //Debug.Log("The firewall was killed");
        rb.velocity = Vector2.zero;

        animator.SetBool("killed", true);

        StartCoroutine(waitUntilAnimeDeath());        

    }

    IEnumerator waitUntilAnimeDeath(){
        yield return new WaitForSeconds(5);
        gameController.KilledBoss();
        Destroy(this.gameObject);
    }
    

    private void Shoot(Vector2 dir)
    {
        GameObject f = Instantiate(fireball,mouth.position,Quaternion.identity);
        f.GetComponent<Rigidbody2D>().velocity = dir.normalized * fireBallSpeed;
    }

    private void bounceHor()
    {
        t += Time.deltaTime;
        fireTime += Time.deltaTime;
        rb.velocity = new Vector2(moveDir * bounceSpeed,0);
    }
    private void shootPhaseOne()
    {
        Shoot(Vector2.down);
    }
    private void sineWave()
    {
        t += Time.deltaTime;
        fireTime += Time.deltaTime;
        rb.velocity = new Vector2(moveDir * bounceSpeed, -amp * Mathf.Cos(t * freq));
    }
    private void shootPhaseTwo()
    {
        Shoot(new Vector2(Mathf.Sin(Mathf.PI / 4), - Mathf.Cos(Mathf.PI/4)));
        Shoot(new Vector2(-Mathf.Sin(Mathf.PI / 4), -Mathf.Cos(Mathf.PI / 4)));
    }
    private void moveToCentre()
    {
        t += Time.deltaTime;
        fireTime += Time.deltaTime;
        if (Mathf.Abs(transform.position.x) > smallVal)
            rb.velocity = new Vector2(moveDir * bounceSpeed, 0);
        else
        {
            rb.velocity = new Vector2(0, 0);
            movedFlag = 1;
        }
    }
    private void shootPhaseThree(float angle)
    {
        for (int i = 0; i < numShotsPhase; i++)
        {
            Shoot(new Vector2(Mathf.Sin(angle), -Mathf.Cos(angle)));
            angle += 2*Mathf.PI / numShotsPhase;
        }
    }
    private void shootPhaseFour()
    {
        angle = - Mathf.PI / 2;
        for (int i = 0; i < numShotsPhase + 1; i++)
        {
            Shoot(new Vector2(Mathf.Sin(angle), -Mathf.Cos(angle)));
            angle += Mathf.PI / numShotsPhase;
        }
    }
    private void shootPhasefive(float angle)
    {
        for (int i = 0; i < numShotsPhase; i++)
        {
            Shoot(new Vector2(Mathf.Sin(angle), -Mathf.Cos(angle)));            
        }
    }

}
