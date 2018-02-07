using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineFloater : MonoBehaviour {


    private Vector3 initPos;
    private float time;
	// Use this for initialization
	void Start () {
        initPos = this.transform.position;
        time = 0;
	}

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        this.transform.position = new Vector3(initPos.x, initPos.y + 0.2f * Mathf.Cos(time*2f));
    }
}
