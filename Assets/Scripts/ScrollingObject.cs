using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour {

    Rigidbody rb;
    public float speed;
    public GameController gameController;

    private void Start(){
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, speed, 0);
    }

    private void Update()
    {
        if (gameController.isGameOver)
        {
            Debug.Log("Game is over, background isn't moving");
            rb.velocity = Vector3.zero;
        }
    }



}
