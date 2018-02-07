using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixText : MonoBehaviour {
    private string randNums;
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < 20; i++)
            randNums += Random.Range(0, 10);
	}
}
