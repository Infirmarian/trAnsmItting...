using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    public float speed;
    public int damage;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, speed);
    }

    // Update is called once per frame
   /* void Update () {
        Vector3 pos = this.transform.position;
        this.transform.position = new Vector3(pos.x, pos.y + (speed * Time.deltaTime), pos.z);
	}*/

    public int ReturnDamage(){
        return damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            collision.gameObject.GetComponent<ObstacleMover>().TakeDamage(damage);
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
            Destroy(this.gameObject);
        }else if (collision.gameObject.tag == "Firewall"){
            collision.gameObject.GetComponent<FirewallController>().DealDamage(damage);
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.tag == "Fireball" && damage ==1)
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }

    }
}
