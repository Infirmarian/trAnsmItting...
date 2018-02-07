using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightLeftWall : MonoBehaviour {

    // Use this for initialization
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag != "Player" && other.gameObject.tag != "Firewall")
            Destroy(other.gameObject);
    }
}
