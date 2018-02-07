using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
    private void Start()
    {
        Time.timeScale = 1f;
    }

    public void LoadSceneWithName(string name){
        SceneManager.LoadScene(name);
    }
}
